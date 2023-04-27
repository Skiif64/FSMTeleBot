using FSMTeleBot.Handlers.Abstractions;
using Telegram.Bot;

namespace FSMTeleBot.Handlers.Contexts;
public abstract class HandlerContext<TData> : IHandlerContext<TData>
{
    public TData Data { get; }
    public ITelegramBotClient Client { get; }
    public HandlerContext(TData data, ITelegramBotClient client)
    {
        Data = data;
        Client = client;
    }
}
