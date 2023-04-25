using FSMTeleBot.Handlers.Abstractions;
using System.Collections.Concurrent;

namespace FSMTeleBot.Internal.Dispatcher;

public class BotDispatcher : IBotDispatcher
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ConcurrentDictionary<Type, List<HandlerWrapper>> _wrappers = new();
    public BotDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task SendAsync<T>(T argument, CancellationToken cancellationToken = default)
    {
        if (argument is null)
            throw new NullReferenceException(nameof(argument));

        var wrappers = _wrappers.GetOrAdd(typeof(T),
            t =>
            {
                var handlers = (IEnumerable<IHandler<T, IHandlerContext<T>>>)_serviceProvider
                .GetService(typeof(IEnumerable<IHandler<T, IHandlerContext<T>>>))!;
                var list = new List<HandlerWrapper>();
                if (!handlers.Any())
                    return list;
                foreach (var handler in handlers)
                {
                    list.Add(new HandlerDescriptor<T>(handler, _serviceProvider));
                }
                return list;
            });
        var wrapper = wrappers.FirstOrDefault(w => w.CanHandle(argument));
        if (wrapper is null)
            return;
        await wrapper.HandleAsync(argument, _serviceProvider, cancellationToken);
    }
}
