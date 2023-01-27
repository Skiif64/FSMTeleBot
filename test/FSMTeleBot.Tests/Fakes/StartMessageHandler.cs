using FSMTeleBot.Filters;
using FSMTeleBot.Handlers.Abstractions;
using Telegram.Bot.Types;

namespace FSMTeleBot.Tests.TestHandlers;

[MessageFilter(Contains = "start")]
public class StartMessageHandler : IHandler<Message>
{
    public Task HandleAsync(Message data, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
