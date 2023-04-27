using FSMTeleBot.Callbacks;
using FSMTeleBot.Handlers.Abstractions;
using FSMTeleBot.Handlers.Contexts;
using FSMTeleBot.States.Abstractions;
using System.Collections.Concurrent;
using System.Xml.Linq;
using Telegram.Bot;
using Telegram.Bot.Types;

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
        var client = (ITelegramBotClient)_serviceProvider.GetService(typeof(ITelegramBotClient))!;
              
        if(data is Message message)
        {
            var contextFactory = (IChatContextFactory)_serviceProvider.GetService(typeof(IChatContextFactory))!;
            var chatContext = await contextFactory.GetContextAsync(data, cancellationToken);
            return (IHandlerContext<TData>)new MessageContext(message, client, chatContext);
        }
        //else if(data is CallbackQuery query)
        //{
        //    var serializer = (ICallbackSerializer)_serviceProvider.GetService(typeof(ICallbackSerializer))!;
        //    var callback = serializer.DeserializeAs(query.Data, wrapper.)
        //    return (IHandlerContext<TData>)new CallbackQueryContext(message, client);
        //}
        return new HandlerContext<TData>(data, client);        
    }
}
