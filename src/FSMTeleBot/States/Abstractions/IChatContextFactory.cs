namespace FSMTeleBot.States.Abstractions;

public interface IChatContextFactory<TMessage>
{
    Task<IChatContext> GetContextAsync(TMessage message, CancellationToken cancellationToken = default);
}
