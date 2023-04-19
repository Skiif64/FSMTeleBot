using FSMTeleBot.Handlers.Abstractions;
using System.Collections.Concurrent;

namespace FSMTeleBot.Internal.Dispatcher;

public class BotDispatcher : IBotDispatcher
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ConcurrentDictionary<Type, List<HandlerDescriptor>> _handlers = new();
    public BotDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task SendAsync<T>(T argument, CancellationToken cancellationToken = default)
    {
        if (argument is null)
            throw new NullReferenceException(nameof(argument));

        var wrappers = _handlers.GetOrAdd(typeof(T),
            t =>
            {
                var services = (IEnumerable<IHandler<T>>)_serviceProvider.GetService(typeof(IEnumerable<IHandler<T>>))!;
                var list = new List<HandlerDescriptor>();
                if (!services.Any())
                    return list;
                foreach (var service in services)
                {
                    list.Add(new HandlerWrapper<T>(service, _serviceProvider));
                }
                return list;
            });
        var wrapper = wrappers.FirstOrDefault(w => w.CanHandle(argument));
        if (wrapper is null)
            return; //TODO: Exception?
        await wrapper.HandleAsync(argument, _serviceProvider, cancellationToken);
    }
}
