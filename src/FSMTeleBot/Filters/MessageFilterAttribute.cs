﻿using FSMTeleBot.Abstractions;
using FSMTeleBot.States;
using FSMTeleBot.States.Abstractions;
using System.Text.RegularExpressions;
using Telegram.Bot.Types;

namespace FSMTeleBot.Filters;

public class MessageFilterAttribute : FilterAttribute
{
    public string? ContainsCommand { get; init; }
    public string? Contains { get; init; }
    public string? Regexp { get; init; }
    public string? RequiredState { get; init; }
    public override bool IsMatch(object argument, IServiceProvider provider)
    {
        if (argument is not Message message)
            throw new ArgumentException($"{nameof(argument)} is not assignable to Message type");
        return IsMatch(message, provider);
    }

    public bool IsMatch(Message message, IServiceProvider provider)
    {
        if (Contains is not null
            && !message.Text.Contains(Contains, StringComparison.InvariantCultureIgnoreCase))
            return false;
        if (ContainsCommand is not null
            && !message.Text.StartsWith("/" + ContainsCommand, StringComparison.InvariantCultureIgnoreCase))
            return false;
        if (Regexp is not null
            && !Regex.IsMatch(message.Text, Regexp))
            return false;
        if (!IsAllowed(message.Chat.Id, message.From.Id, provider))
            return false;
        if(RequiredState is not null)
        {
            var contextFactory = (IChatContextFactory)provider.GetService(typeof(IChatContextFactory))!;
            var context = contextFactory.GetContextAsync(message).Result;
            var currentState = context.CurrentState;
            if (currentState is null || !currentState.Equals(new ChatState(RequiredState)))
                return false;
        }

        return true;

    }
}
