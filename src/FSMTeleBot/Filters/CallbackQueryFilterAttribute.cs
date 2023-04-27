﻿using FSMTeleBot.Callbacks;
using Telegram.Bot.Types;

namespace FSMTeleBot.Filters;
public class CallbackQueryFilterAttribute : FilterAttribute
{
    public Type? QueryType { get; set; }

    public override bool IsMatch(object argument, IServiceProvider provider)
    {
        if (argument is not CallbackQuery query)
            throw new ArgumentException(
                "Argument is not instance of CallbackQuery.", nameof(argument));
        return IsMatch(query, provider);
    }

    public bool IsMatch(CallbackQuery query, IServiceProvider provider)
    {
        if(QueryType is not null)
        {
            var serializer = (ICallbackSerializer)provider
                .GetService(typeof(ICallbackSerializer))!;
            var callback = serializer.DeserializeAsType(query.Data
                ?? throw new ArgumentNullException(nameof(query.Data)),
                QueryType);
            if (callback is null)
                return false;
        }
        return true;
    }
}
