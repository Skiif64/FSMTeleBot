using FSMTeleBot.Handlers.Abstractions;
using FSMTeleBot.Handlers.Contexts;
using System.Collections.Concurrent;
using System.Xml.Linq;

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
        var context = BuildContext(argument);
        await wrapper.HandleAsync(context, _serviceProvider, cancellationToken);
    }

    private IHandlerContext<TData> BuildContext<TData>(TData data)
    {//TODO: normal Context
        return new HandlerContext<TData>(data);
    }
}
