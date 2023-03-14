using FSMTeleBot.States.Abstractions;
using Telegram.Bot;

namespace FSMTeleBot.Handlers.Abstractions;

public class HandlerBase<TMessage> : IHandler<TMessage>
{
    private readonly IChatContextFactory _contextFactory;    
    protected ITelegramBotClient Client { get; }
    protected IChatContext Context { get; private set; } = null!;

    public HandlerBase(ITelegramBotClient client, IChatContextFactory contextFactory)
    {
        Client = client;
        _contextFactory = contextFactory;            
    }

    public Task HandleAsync(TMessage data, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    internal async Task InitAsync(TMessage data, CancellationToken cancellationToken = default)
    {
        Context = await _contextFactory.GetContextAsync(data, cancellationToken);
    }
}
