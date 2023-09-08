using FSMTeleBot.Callbacks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace FSMTeleBot.Handlers.Contexts;
internal class CallbackQueryContext<TCallback> : HandlerContext<CallbackQuery>
    where TCallback : ICallbackQuery
{
    public TCallback Query { get; }
    public CallbackQueryContext(CallbackQuery data, ITelegramBotClient client, TCallback query) : base(data, client)
    {
        Query = query;
    }
}
