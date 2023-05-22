using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;


namespace FSMTeleBot.Services;

internal class PollingUpdateHandler : IUpdateHandler //TODO: Move to internal folder
{
    private readonly InternalUpdateHandler _updateHandler; //TODO: use interface

    public PollingUpdateHandler(InternalUpdateHandler updateHandler)
    {
        _updateHandler = updateHandler;
    }

    public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        Console.WriteLine(exception.ToString());
        return Task.CompletedTask;
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        => await _updateHandler.HandleUpdateAsync(update, cancellationToken);
    
}
