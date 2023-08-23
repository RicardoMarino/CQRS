using CQRS.Core.Commands;

namespace CQRS.Command.Api.Commands;

public class RemoveCommentCommand : BaseCommand
{
    public Guid CommentId { get; set; }
    public string UserName { get; set; }
}