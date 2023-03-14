namespace FSMTeleBot.States.Abstractions;

public interface IChatContextFactory
{
    Task<ChatContext> GetContextAsync(CancellationToken cancellationToken = default);
}
