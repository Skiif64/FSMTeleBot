
namespace FSMTeleBot.Handlers.Abstractions;

public interface IHandler<TData, out TContext>
    where TContext : IHandlerContext<TData>
{    
    Task HandleAsync(object context, CancellationToken cancellationToken = default);
}