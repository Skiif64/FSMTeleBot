using FSMTeleBot.Internal.Mediator;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace FSMTeleBot.Services;

internal class DefaultUpdateHandler : IUpdateHandler
{
    private readonly IBotMediator _mediator;

    public DefaultUpdateHandler(IBotMediator mediator)
    {
        _mediator = mediator;
    }

    public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        var action = update.Type switch
        {
            UpdateType.Message => _mediator.Send(update.Message, cancellationToken),
            _ => Task.CompletedTask
        };
        await action;
    }
}
