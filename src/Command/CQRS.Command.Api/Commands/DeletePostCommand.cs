using CQRS.Core.Commands;

namespace CQRS.Command.Api.Commands;

public class DeletePostCommand : BaseCommand
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
}