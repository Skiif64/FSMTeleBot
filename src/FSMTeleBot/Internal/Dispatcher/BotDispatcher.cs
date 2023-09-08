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
                    list.Add(new HandlerWrapper<T>(handler, _serviceProvider));
                }
                return list;
            });
        var wrapper = wrappers.FirstOrDefault(w => w.CanHandle(argument));
        if (wrapper is null)
            return;
        var context = await BuildContextAsync(argument, cancellationToken);
        await wrapper.HandleAsync(context, _serviceProvider, cancellationToken);
    }

    private async Task<IHandlerContext<TData>> BuildContextAsync<TData>(
        TData data, CancellationToken cancellationToken)
    {//TODO: normal Context
        var factory = _serviceProvider
            .GetService(typeof(IHandlerContextFactory<TData>))
            as IHandlerContextFactory<TData>;
        if (factory is null)
            throw new ArgumentException("Cannot resolve context factory for type.", nameof(TData));

        return await factory.CreateAsync(data, cancellationToken);
    }
}
