using FSMTeleBot.FSM.Abstractions;

namespace FSMTeleBot.FSM;

public class FsmContext
{
    private readonly long _chatId;
    private readonly IStateStorage _stateStorage;
    private StateGroupBase _currentStateGroup;
    public StateBase CurrentState { get; private set; } = null!;

    public FsmContext(long chatId, IStateStorage stateStorage)
    {
        _chatId = chatId;
        _stateStorage = stateStorage;
        _currentStateGroup = _stateStorage.GetOrCreateAsync(_chatId).Result;
        CurrentState = _currentStateGroup.CurrentState;
    }

    public async Task SetState(StateGroupBase group)
    {
        //TODO: Validate
        await _stateStorage.UpdateAsync(_chatId, group);
        _currentStateGroup = group;
    }

    public async Task Next()
    {
        CurrentState = _currentStateGroup.Next();
        await _stateStorage.UpdateAsync(_chatId, _currentStateGroup);
    }

    public void Previous()
    {

    }

    public void Finish()
    {

    }

    public void SetValue(object value)
    {

    }

    public object[] GetValues()
    {
        throw new NotImplementedException();
    }
}
