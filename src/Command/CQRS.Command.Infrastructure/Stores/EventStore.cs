using CQRS.Command.Domain.Aggregates;
using CQRS.Core.Domain;
using CQRS.Core.Events;
using CQRS.Core.Exceptions;
using CQRS.Core.Infrastructure;

namespace CQRS.Command.Infrastructure.Stores;

public class EventStore : IEventStore
{
    private readonly IEventStoreRepository _eventStoreRepository;

    public EventStore(IEventStoreRepository eventStoreRepository)
    {
        _eventStoreRepository = eventStoreRepository;
    }
    
    public async Task SaveEventsAsync(Guid aggregateId, IEnumerable<BaseEvent> events, int expectedVersion)
    {
        var eventStream = await _eventStoreRepository.FindByAggregateId(aggregateId);
        
        if(expectedVersion != -1 && eventStream[^1].Version != expectedVersion) 
            throw new ConcurrencyException("Incorrect post ID provided!");

        var version = expectedVersion;
        
        foreach (var @event in events)
        {
            version++;
            @event.Version = version;
            var eventType = @event.GetType().Name;
            var eventModel = new EventModel
            {
                TimeStamp = DateTime.Now,
                AggregateIdentifier = aggregateId,
                AggregateType = nameof(PostAggregate),
                Version = version,
                EventType = eventType,
                EventData = @event
            };

            await _eventStoreRepository.SaveAsync(eventModel);
        }
    }

    public async Task<List<BaseEvent>> GetEventsAsync(Guid aggregateId)
    {
        var eventStream =  await _eventStoreRepository.FindByAggregateId(aggregateId);

        if (eventStream.Any()) throw new AggregateNotFoundException("Incorrect post ID provided!");

        return eventStream
            .OrderBy(events => events.Version)
            .Select(events => events.EventData).ToList();
    }
}