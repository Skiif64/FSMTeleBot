namespace FSMTeleBot.Abstractions;

public interface IBotBuilder
{
    Task<ITelegramBot> BuildAsync(CancellationToken cancellationToken = default);
}
