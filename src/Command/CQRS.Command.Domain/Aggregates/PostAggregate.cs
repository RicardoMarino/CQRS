using CQRS.Common.Events;
using CQRS.Core.Domain;

namespace CQRS.Command.Domain.Aggregates;

public class PostAggregate : AggregateRoot
{
    private bool _active;
    private string _author;
    private readonly Dictionary<Guid, Tuple<string, string>> _comments = new();

    public bool Active => _active;

    public PostAggregate()
    {
        
    }
    public PostAggregate(Guid id, string author, string message)
    {
        RiseEvent(new PostCreatedEvent
        {  
            Id = id,
            Author = author,
            Message = message,
            DatePosted = DateTime.Now
        });
    }

    public void Apply(PostCreatedEvent @event)
    {
        _id = @event.Id;
        _active = true;
        _author = @event.Author;
    }

    public void EditMessage(string message)
    {
        if (!_active) throw new InvalidOperationException("You cannot edit the message of in inactive Post!");

        if (string.IsNullOrWhiteSpace(message))
            throw new InvalidOperationException(
                $"The value of {nameof(message)} cannot be null or empty. Please provide  a valid {nameof(message)}!");
        
        RiseEvent( new MessageUpdatedEvent
        {
            Id = _id,
            Message = message
        });
    }

    public void LikePost()
    {
        if (!_active) throw new InvalidOperationException("You cannot like an inactive Post!");
        
        RiseEvent(new PostLikedEvent
        {
            Id = _id
        });
    }

    public void Apply(PostLikedEvent @event)
    {
        _id = @event.Id;
    }

    public void AddComment(string comment, string userName)
    {
        if (!_active) throw new InvalidOperationException("You cannot add a comment to an inactive Post!");
        
        if (string.IsNullOrWhiteSpace(comment))
            throw new InvalidOperationException(
                $"The value of {nameof(comment)} cannot be null or empty. Please provide  a valid {nameof(comment)}!");
        
        RiseEvent(new CommentAddedEvent{
            Id = _id,
            CommentId = Guid.NewGuid(),
            Comment = comment,
            UserName = userName
        });
    }

    public void Apply(CommentAddedEvent @event)
    {
        _id = @event.Id;
        _comments.Add(@event.CommentId, new Tuple<string, string>(@event.Comment, @event.UserName));
    }

    public void EditComment(Guid commentId, string comment, string userName)
    {
        if (!_active) throw new InvalidOperationException("You cannot edit a comment of an inactive Post!");

        if (!_comments[commentId].Item2.Equals(userName, StringComparison.CurrentCultureIgnoreCase))
            throw new InvalidOperationException(
                "You a not allowed to edit a comment that was made by another user!");
        
        RiseEvent(new CommentUpdatedEvent
        {
            Id = _id,
            CommentId = commentId,
            Comment = comment,
            UserName = userName,
            EditDate = DateTime.Now
        });
    }

    public void Apply(CommentUpdatedEvent @event)
    {
        _id = @event.Id;
        _comments[@event.CommentId] = new Tuple<string, string>(@event.Comment, @event.UserName);
    }

    public void RemoveComment(Guid commentId, string userName)
    {
        if (!_active) throw new InvalidOperationException("You cannot remove a comment of an inactive Post!");

        if (!_comments[commentId].Item2.Equals(userName, StringComparison.CurrentCultureIgnoreCase))
            throw new InvalidOperationException(
                "You a not allowed to remove a comment that was made by another user!");
        
        RiseEvent(new CommentRemovedEvent
        {
            Id = _id,
            CommentId = commentId
        });
    }

    public void Apply(CommentRemovedEvent @event)
    {
        _id = @event.Id;
        _comments.Remove(@event.CommentId);
    }

    public void DeletePost(string userName)
    {
        if (!_active) throw new InvalidOperationException("The post has been removed!");

        if (!_author.Equals(userName, StringComparison.CurrentCultureIgnoreCase))
            throw new InvalidOperationException("You a not allowed to delete post that was made by someone else");
        
        RiseEvent(new PostRemoveEvent
        {
            Id = _id
        });
    }

    public void Apply(PostRemoveEvent @event)
    {
        _id = @event.Id;
        _active = false;
    }
}