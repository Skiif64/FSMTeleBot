using FSMTeleBot.States.Abstractions;
using Telegram.Bot;

namespace FSMTeleBot.Handlers.Abstractions;

public class HandlerBase<TMessage> : IHandler<TMessage> //TODO: implement
{
    private readonly IChatContextFactory<TMessage> _factory;
    private IChatContext _context;
    protected ITelegramBotClient Client { get; }
    //protected IChatContext Context => _context ??=
        //_factory.GetContextAsync().Result; //TODO: refactor

    public HandlerBase(ITelegramBotClient client, IChatContextFactory<TMessage> contextFactory)
    {
        Client = client;
        _factory = contextFactory;            
    }

    public Task HandleAsync(TMessage data, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
