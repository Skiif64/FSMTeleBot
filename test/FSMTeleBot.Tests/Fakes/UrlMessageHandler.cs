using FSMTeleBot.Filters;
using FSMTeleBot.Handlers.Abstractions;
using Telegram.Bot.Types;

namespace FSMTeleBot.Tests.Fakes;

[MessageFilter(Regexp = @"^http[s]?://\w")]
public class UrlMessageHandler : IHandler<Message>
{
    public Task HandleAsync(Message data, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
