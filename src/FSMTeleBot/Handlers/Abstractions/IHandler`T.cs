
namespace FSMTeleBot.Handlers.Abstractions;

public interface IHandler<T>
{
    Task HandleAsync(T data, CancellationToken cancellationToken = default);
}