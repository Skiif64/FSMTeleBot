using FSMTeleBot.Callbacks;
using Telegram.Bot.Types;

namespace FSMTeleBot.Filters;
public class CallbackQueryFilterAttribute : FilterAttribute
{
    public string? QueryHeader { get; set; }

    public override bool IsMatch(object argument, IServiceProvider provider)
    {
        if (argument is not CallbackQuery query)
            throw new ArgumentException(
                "Argument is not instance of CallbackQuery.", nameof(argument));
        return IsMatch(query, provider);
    }

    public bool IsMatch(CallbackQuery query, IServiceProvider provider)
    {
        if(QueryHeader is not null)
        {
            var serializer = (ICallbackSerializer)provider
                .GetService(typeof(ICallbackSerializer))!;
            var callback = serializer.Deserialize(query.Data
                ?? throw new ArgumentNullException(nameof(query.Data)));

            if (callback.Header != QueryHeader)
                return false;
        }
        return true;
    }
}
