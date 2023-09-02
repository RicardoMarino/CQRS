using CQRS.Core.Events;

namespace CQRS.Core.Domain;

public abstract class AggregateRoot
{
    private readonly List<BaseEvent> _changes = new();
    protected Guid _id;
    public Guid Id => _id;
    public int Version { get; set; } = -1;
    public IEnumerable<BaseEvent> GetUncommittedChanges() => _changes;

    public void MarkChangesAsCommitted() => _changes.Clear();

    public void ReplaceEvent(IEnumerable<BaseEvent> events)
    {
        foreach (var @event in events)
        {
            ApplyChange(@event, false);
        }
    }

    private void ApplyChange(BaseEvent @event, bool isNew)
    {
        var method = this.GetType().GetMethod("Apply", new Type[] { @event.GetType() });
        
        if (method == null) throw new ArgumentNullException(nameof(method),
            $"The apply method was not found in the aggregate for {@event.GetType().Name}!");
        
        method.Invoke(this, new Object[] { @event });

        if(isNew) _changes.Add(@event);
    }

    protected void RiseEvent(BaseEvent @event)
    {
        ApplyChange(@event, true);
    }
    
    
}