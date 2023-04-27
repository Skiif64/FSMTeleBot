using Telegram.Bot;

namespace FSMTeleBot.Handlers.Abstractions;
public interface IHandlerContext<TData>
{
    TData Data { get; }
    ITelegramBotClient Client { get; }
}
