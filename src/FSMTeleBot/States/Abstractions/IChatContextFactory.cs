namespace FSMTeleBot.States.Abstractions;

public interface IChatContextFactory
{
    Task<IChatContext> GetContextAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default);
}
