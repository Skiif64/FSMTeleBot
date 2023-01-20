using FSMTeleBot.Handlers.Abstractions;
using FSMTeleBot.RouteHandlers.Abstractions;
using Telegram.Bot.Types;

namespace FSMTeleBot.RouteHandlers;

public class MessageRouteHandler : IRouteHandler<Message>
{
    private readonly IHandlersStorage _handlersStorage;

    public MessageRouteHandler(IHandlersStorage handlersStorage)
    {
        _handlersStorage = handlersStorage;
    }

    public async Task RouteAsync(Message message, CancellationToken cancellationToken = default)
    {
        var handler = (IHandler<Message>)_handlersStorage.GetHandler<Message>(message);
        await handler.HandleAsync(message, cancellationToken);
    }
}
