using FSMTeleBot.Abstractions;

namespace FSMTeleBot;

public class BotBuilder : IBotBuilder
{

    public Task<ITelegramBot> BuildAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
