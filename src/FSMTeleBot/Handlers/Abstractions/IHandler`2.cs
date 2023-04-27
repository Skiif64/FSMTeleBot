
namespace FSMTeleBot.Handlers.Abstractions;

public interface IHandler<TData, TContext>
    where TContext : IHandlerContext<TData>
{
    Task HandleAsync(TContext context, CancellationToken cancellationToken = default);
}