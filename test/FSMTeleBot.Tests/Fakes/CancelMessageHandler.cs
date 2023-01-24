﻿using FSMTeleBot.Filters;
using FSMTeleBot.Handlers.Abstractions;
using Telegram.Bot.Types;

namespace FSMTeleBot.Tests.Fakes;

[Filter(Contains = "cancel")]
public class CancelMessageHandler : IHandler<Message>
{
    public Task HandleAsync(Message data, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}