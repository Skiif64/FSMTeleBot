namespace FSMTeleBot.Handlers.Abstractions;
public abstract class HandlerBase<TData, TContext> : IHandler<TData, TContext>
    where TContext : IHandlerContext<TData>
{  
    public Task HandleAsync(object context, CancellationToken cancellationToken = default)
    {
        if(context is not TContext ctx)
        {
            throw new ArgumentException(nameof(context));
        }
        return HandleAsync(ctx, cancellationToken);
    }

    public abstract Task HandleAsync(TContext context, CancellationToken cancellationToken = default);
    
}
