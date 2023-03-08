using FSMTeleBot.Abstractions;
using FSMTeleBot.ChatState.Abstractions;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
namespace FSMTeleBot.Services;

public class InMemoryStateStorage : IChatStateStorage
{
    private readonly struct ChatUserId
    {        
        public long ChatId { get; }
        public long UserId { get; }
        public ChatUserId(long chatId, long userId)
        {
            ChatId = chatId;
            UserId = userId;
        }

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if(obj is not ChatUserId other) 
                return false;
            return ChatId.Equals(other.ChatId)
                && UserId.Equals(other.UserId);
        }
        public override int GetHashCode()
        {
            return unchecked(ChatId.GetHashCode() + UserId.GetHashCode());
        }
    }
    private static readonly IChatState _initialState = new ChatState.ChatState();
    private readonly ConcurrentDictionary<ChatUserId, IChatState> _storage = new();
    public Task<IChatState> GetOrInit(long chatId, long userId, CancellationToken cancellationToken = default)
    {
        var key = new ChatUserId(chatId, userId);
        var newState = _storage.GetOrAdd(key, _initialState);
        return Task.FromResult(newState);
    }

    public Task UpdateAsync(long chatId, long userId, IChatState state, CancellationToken cancellationToken = default)
    {
        var key = new ChatUserId(chatId, userId);
        _storage.AddOrUpdate(key, state, (key, value) => state);
        return Task.CompletedTask;
    }
}
