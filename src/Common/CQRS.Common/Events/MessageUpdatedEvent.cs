using CQRS.Core.Events;

namespace CQRS.Common.Events;

public class MessageUpdatedEvent : BaseEvent
{
    public MessageUpdatedEvent() : base(nameof(MessageUpdatedEvent))
    {
    }

    public string Message { get; set; }
}