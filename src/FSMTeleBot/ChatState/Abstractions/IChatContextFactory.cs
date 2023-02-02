namespace FSMTeleBot.ChatState.Abstractions;

public interface IChatContextFactory
{
    Task<ChatContext> GetContextAsync(long chatId, long userId, CancellationToken cancellationToken = default);
}
