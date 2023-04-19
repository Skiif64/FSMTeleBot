using FSMTeleBot.Handlers.Abstractions;
using System.Collections.Concurrent;

namespace FSMTeleBot.Internal.Dispatcher;

public class BotDispatcher : IBotDispatcher
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ConcurrentDictionary<Type, List<HandlerDescriptor>> _descriptors = new();
    public BotDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task SendAsync<T>(T argument, CancellationToken cancellationToken = default)
    {
        if (argument is null)
            throw new NullReferenceException(nameof(argument));

        var descriptors = _descriptors.GetOrAdd(typeof(T),
            t =>
            {
                var handlers = (IEnumerable<IHandler<T>>)_serviceProvider.GetService(typeof(IEnumerable<IHandler<T>>))!;
                var list = new List<HandlerDescriptor>();
                if (!handlers.Any())
                    return list;
                foreach (var handler in handlers)
                {
                    list.Add(new HandlerDescriptor<T>(handler, _serviceProvider));
                }
                return list;
            });
        var descriptor = descriptors.FirstOrDefault(w => w.CanHandle(argument));
        if (descriptor is null)
            return;
        await descriptor.HandleAsync(argument, _serviceProvider, cancellationToken);
    }
}
