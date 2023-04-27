using FSMTeleBot.Handlers.Abstractions;
using Telegram.Bot;

namespace FSMTeleBot.Handlers.Contexts;
public class HandlerContext<TData> : IHandlerContext<TData> //TODO: abstract
{
    public TData Data { get; }
    public ITelegramBotClient Client { get; }
    public HandlerContext(TData data, ITelegramBotClient client)
    {
        Data = data;
        Client = client;
    }
}
