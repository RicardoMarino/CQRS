using CQRS.Core.Commands;

namespace CQRS.Command.Api.Commands;

public class EditCommentCommand : BaseCommand
{
    public Guid CommentId { get; set; }
    public string Comment { get; set; }
    public string UserName { get; set; }
}