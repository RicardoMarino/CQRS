using CQRS.Core.Commands;

namespace CQRS.Command.Api.Commands;

public class AddCommentCommand : BaseCommand
{
    public string Comment { get; set; }
    public string UserName { get; set; }
}