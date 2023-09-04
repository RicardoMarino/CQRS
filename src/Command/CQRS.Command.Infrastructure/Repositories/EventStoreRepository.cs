using CQRS.Command.Infrastructure.Configurations;
using CQRS.Core.Domain;
using CQRS.Core.Events;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CQRS.Command.Infrastructure.Repositories;

public class EventStoreRepository : IEventStoreRepository
{
    private readonly IMongoCollection<EventModel> _eventStoreCollection;

    public EventStoreRepository(IOptions<MongoConfiguration> configuration)
    {
        var mongoClient = new MongoClient(configuration.Value.COnntectionString);
        var mongoDataBase = mongoClient.GetDatabase(configuration.Value.DataBase);
        _eventStoreCollection = mongoDataBase.GetCollection<EventModel>(configuration.Value.Collection);
    }
    
    public async Task SaveAsync(EventModel @event) => 
        await _eventStoreCollection.InsertOneAsync(@event).ConfigureAwait(false);
    

    public async Task<List<EventModel>> FindByAggregateId(Guid aggregateId) =>
        await _eventStoreCollection
            .Find(events => events.AggregateIdentifier == aggregateId)
            .ToListAsync()
            .ConfigureAwait(false);
    
}