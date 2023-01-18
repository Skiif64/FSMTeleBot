
using FSMTeleBot.FSM;
using FSMTeleBot.FSM.Abstractions;
using System.Runtime.Serialization;
using System.Text.Json;

var stateGroup = new StateGroupBase(new List<StateBase>
{
    new StateBase("state12"),
    new StateBase("state22"),
    new StateBase("state32"),
    new StateBase("state42")
});

var fsmContext = new FsmContext(12345, new StateStorage());
await fsmContext.Next();
await fsmContext.Next();
fsmContext.Previous();
await fsmContext.SetState(stateGroup);

Console.ReadLine();


class StateStorage : IStateStorage
{
    private StateGroupBase stateGroup = new StateGroupBase(new List<StateBase>
{
    new StateBase("state1"),
    new StateBase("state2"),
    new StateBase("state3"),
    new StateBase("state4")
});
    private readonly Dictionary<long, StateGroupBase> states = new Dictionary<long, StateGroupBase>();

    public StateStorage()
    {
        states.Add(12345, stateGroup);
    }
    
    public Task<StateGroupBase> GetOrCreateAsync(long chatId)
    {
        return Task.FromResult(states[chatId]);
    }

    public Task UpdateAsync(long chatId, StateGroupBase stateGroup)
    {
        states[chatId] = stateGroup;
        return Task.CompletedTask;
    }
}

