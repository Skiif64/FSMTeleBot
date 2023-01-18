namespace FSMTeleBot.RouteHandlers.Abstractions;

public interface IRouteHandler<T>
{
    Task RouteAsync(T data, CancellationToken cancellationToken = default);
}
