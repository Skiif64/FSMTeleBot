﻿using FSMTeleBot.Abstractions;
using Telegram.Bot.Types.Enums;

namespace FSMTeleBot.Filters;

[AttributeUsage(AttributeTargets.Class, Inherited = true)]
public abstract class FilterAttribute : Attribute
{
    public ChatMemberStatus? Allowed { get; init; }

    public FilterAttribute()
    {

    }

    public abstract bool IsMatch(object argument, IServiceProvider provider);

    protected bool IsAllowed(long chatId, long userId, IServiceProvider provider)
    {
        if (Allowed is not null)
        {
            var memberService = (IChatMemberService)provider.GetService(typeof(IChatMemberService))!;
            if (memberService.GetStatusAsync(chatId, userId).Result > Allowed) //TODO: refactor this
                return false;
        }

        return true;
    }

}
