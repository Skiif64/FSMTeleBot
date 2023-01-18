namespace FSMTeleBot.FSM.Abstractions;

public interface IStateStorage
{
    public Task<StateGroupBase> GetOrCreateAsync(long chatId);
    public Task UpdateAsync(long chatId, StateGroupBase stateGroup);
}
