using FSMTeleBot.States.Abstractions;
using Telegram.Bot;

namespace FSMTeleBot.Handlers.Abstractions;

public abstract class HandlerBase<TMessage> : IHandler<TMessage>
{
    protected ITelegramBotClient Client { get; }
    protected internal IChatContext StateContext { get; internal set; } = null!; //TODO: inject this

    public HandlerBase(ITelegramBotClient client)
    {
        Client = client;      
    }

    public abstract Task HandleAsync(TMessage data, CancellationToken cancellationToken = default);
    internal async Task HandleInternalAsync(TMessage data, IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
    {
        if(StateContext is null)
            await SetStateContextAsync(data, serviceProvider, cancellationToken);
        
        await HandleAsync(data, cancellationToken)
            .ConfigureAwait(false);
    }

    private async Task SetStateContextAsync(TMessage data, IServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        var contextFactory = (IChatContextFactory)serviceProvider.GetService(typeof(IChatContextFactory))!;
        StateContext = await contextFactory.GetContextAsync(data, cancellationToken);
    }
}
