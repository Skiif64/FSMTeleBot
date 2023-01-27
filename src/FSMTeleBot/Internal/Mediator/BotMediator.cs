using FSMTeleBot.Handlers.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using Telegram.Bot.Types;

namespace FSMTeleBot.Internal.Mediator;

public class BotMediator : IBotMediator
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ConcurrentDictionary<Type, List<HandlerWrapper>> _handlers = new();
    public BotMediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }    

    public async Task Send<T>(T argument, CancellationToken cancellationToken = default)
    {
        var wrappers = _handlers.GetOrAdd(typeof(T),
            t =>
            {
                var services = _serviceProvider.GetServices(typeof(IHandler<T>));                
                var list = new List<HandlerWrapper>();
                if (!services.Any())
                    return list;
                foreach (var service in services)
                {
                    list.Add(new HandlerWrapper<T>((IHandler<T>)service));
                }
                return list;
            });
        var wrapper = wrappers.FirstOrDefault(w => w.CanHandle(argument));
        if(wrapper is null)
            return; //TODO: Exception?
        await wrapper.Handle(argument, _serviceProvider, cancellationToken);
    }
}
