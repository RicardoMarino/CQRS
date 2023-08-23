using CQRS.Core.Commands;

namespace CQRS.Command.Api.Commands;

public class NewPostCommand : BaseCommand
{
    public string Author { get; set; }
    public string Message { get; set; }
}