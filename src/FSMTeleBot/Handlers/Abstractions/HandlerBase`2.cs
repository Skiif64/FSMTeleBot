namespace FSMTeleBot.Handlers.Abstractions;
public abstract class HandlerBase<TData, TContext> : IHandler<TData, TContext>
    where TContext : IHandlerContext<TData>
{
    public TContext Context { get; internal set; } = default!;

    public abstract Task HandleAsync(CancellationToken cancellationToken = default);
    
}
