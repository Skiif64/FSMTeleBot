
namespace FSMTeleBot.Handlers.Abstractions;

public interface IHandler<TData, out TContext>
    where TContext : IHandlerContext<TData>
{
    TContext Context { get; }
    Task HandleAsync(CancellationToken cancellationToken = default);
}