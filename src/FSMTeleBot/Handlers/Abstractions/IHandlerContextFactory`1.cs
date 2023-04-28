namespace FSMTeleBot.Handlers.Abstractions;
public interface IHandlerContextFactory<T>
{
    Task<IHandlerContext<T>> CreateAsync(T data, CancellationToken cancellationToken = default);
}
