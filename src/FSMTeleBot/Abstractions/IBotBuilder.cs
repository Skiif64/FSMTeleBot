namespace FSMTeleBot.Abstractions;

public interface IBotBuilder
{
    ITelegramBot BuildAsync(CancellationToken cancellationToken = default);
}
