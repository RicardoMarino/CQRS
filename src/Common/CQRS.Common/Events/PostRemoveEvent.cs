using CQRS.Core.Events;

namespace CQRS.Common.Events;

public class PostRemoveEvent : BaseEvent
{
    public PostRemoveEvent() : base(nameof(PostRemoveEvent))
    {
    }
    
    
}