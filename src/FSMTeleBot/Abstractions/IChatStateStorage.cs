using FSMTeleBot.ChatState.Abstractions;

namespace FSMTeleBot.Abstractions;

public interface IChatStateStorage
{
    Task<IChatState> GetOrInit(long chatId, long userId, CancellationToken cancellationToken = default);
    Task UpdateAsync(long chatId, long userId, IChatState state, CancellationToken cancellationToken = default);
}
