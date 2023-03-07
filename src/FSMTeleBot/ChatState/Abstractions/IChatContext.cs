namespace FSMTeleBot.ChatState.Abstractions;

public interface IChatContext
{
    IChatState CurrentState { get; }

    Task FinishStateAsync(CancellationToken cancellationToken = default);
    Task SetStateAsync(IChatState state, CancellationToken cancellationToken = default);    
}
