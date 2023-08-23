using CQRS.Core.Commands;

namespace CQRS.Command.Api.Commands;

public class EditMessageCommand : BaseCommand
{
    public string Message { get; set; }
}