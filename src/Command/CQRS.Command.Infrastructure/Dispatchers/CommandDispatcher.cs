using CQRS.Core.Commands;
using CQRS.Core.Infrastructure;

namespace CQRS.Command.Infrastructure.Dispatchers;

public class CommandDispatcher : ICommandDispatcher
{
    private readonly Dictionary<Type, Func<BaseCommand, Task>?> _handlers = new();
    public void RegisterHandler<T>(Func<T, Task> handler) where T : BaseCommand
    {
        if (_handlers.ContainsKey(typeof(T))) 
            throw new IndexOutOfRangeException("You cannot register the same command handler twice!");
        
        _handlers.Add(typeof(T), fun =>  handler((T) fun));
    }

    public async Task SendAsync(BaseCommand command)
    {
        if (_handlers.TryGetValue(command.GetType(), out Func<BaseCommand, Task>? handler))
            await handler?.Invoke(command)!;
        else
            throw new ArgumentNullException(nameof(handler), "No command handler was registered");
    }
}