using FSMTeleBot.ChatState.Abstractions;

namespace FSMTeleBot.Abstractions;

public interface IChatStateStorage
{
    Task<IChatState> GetOrAddAsync(long chatId, long userId, IChatState toAddState, CancellationToken cancellationToken = default);
    Task UpdateAsync(long chatId, long userId, IChatState state, CancellationToken cancellationToken = default);
}
