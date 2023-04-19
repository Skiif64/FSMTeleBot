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
    internal async Task HandleInternalAsync(TMessage data, CancellationToken cancellationToken = default)
    {
        //TODO: implement
        await HandleAsync(data, cancellationToken)
            .ConfigureAwait(false);
    }
}
