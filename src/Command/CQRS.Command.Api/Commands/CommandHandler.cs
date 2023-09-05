using CQRS.Command.Domain.Aggregates;
using CQRS.Core.Handlers;

namespace CQRS.Command.Api.Commands;

public class CommandHandler : ICommandHandler
{
    private readonly IEventSourcingHandler<PostAggregate> _eventSourcingHandler;

    public CommandHandler(IEventSourcingHandler<PostAggregate> eventSourcingHandler)
    {
        _eventSourcingHandler = eventSourcingHandler;
    }

    public async Task HandlerAsync(NewPostCommand command)
    {
        var aggregate = new PostAggregate(command.Id, command.Author, command.Message);
        await _eventSourcingHandler.SaveAsync(aggregate);
    }

    public async Task HandlerAsync(DeletePostCommand command)
    {        
        var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
        aggregate.DeletePost(command.UserName);
        
        await _eventSourcingHandler.SaveAsync(aggregate);
    }

    public async Task HandlerAsync(EditCommentCommand command)
    {
        var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
        aggregate.EditComment(command.CommentId, command.Comment, command.UserName);
        
        await _eventSourcingHandler.SaveAsync(aggregate);
    }

    public async Task HandlerAsync(LikePostCommand command)
    {
        var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
        aggregate.LikePost();
        
        await _eventSourcingHandler.SaveAsync(aggregate);
    }

    public async Task HandlerAsync(AddCommentCommand command)
    {
        var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
        aggregate.AddComment(command.Comment, command.UserName);
        
        await _eventSourcingHandler.SaveAsync(aggregate);
    }

    public async Task HandlerAsync(RemoveCommentCommand command)
    {
        var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
        aggregate.RemoveComment(command.CommentId, command.UserName);
        
        await _eventSourcingHandler.SaveAsync(aggregate);
    }

    public async Task HandlerAsync(EditMessageCommand command)
    {
        var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
        aggregate.EditMessage(command.Message);
        
        await _eventSourcingHandler.SaveAsync(aggregate);
    }
}