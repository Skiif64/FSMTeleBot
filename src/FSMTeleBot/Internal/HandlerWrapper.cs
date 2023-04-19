using FSMTeleBot.Filters;
using FSMTeleBot.Handlers.Abstractions;
using System.ComponentModel;
using System.Reflection;

namespace FSMTeleBot.Internal;

internal abstract class HandlerWrapper
{
    public abstract Task HandleAsync(object argument, IServiceProvider provider, CancellationToken cancellationToken = default);
    public abstract bool CanHandle(object argument);
}
internal class HandlerWrapper<TMessage> : HandlerWrapper
{
    private readonly Type _handlerType;
    private readonly IHandler<TMessage> _handler;
    private readonly FilterAttribute? _filter;
    private readonly IServiceProvider _serviceProvider;

    public HandlerWrapper(IHandler<TMessage> handler, IServiceProvider serviceProvider)
    {
        //TODO: Validation
        _handler = handler;
        _handlerType = handler.GetType();
        _filter = TypeDescriptor
            .GetAttributes(_handler)
            .OfType<FilterAttribute>()
            .SingleOrDefault();
        _serviceProvider = serviceProvider;
    }
    //TODO: remove IServiceProvider?
    public override Task HandleAsync(object argument, IServiceProvider provider, CancellationToken cancellationToken = default)
    {        
        if(argument is not TMessage message)
            throw new ArgumentException(nameof(argument));

        return HandleAsync(message, provider, cancellationToken);
    }
    
    public Task HandleAsync(TMessage message, IServiceProvider provider, CancellationToken cancellationToken = default)
    {        
        return _handler.HandleAsync(message, cancellationToken);
    }
    
    public override bool CanHandle(object argument)
    {
        if (argument is not TMessage message)
            throw new ArgumentException(nameof(argument));
        return CanHandle(message);
    }

    public bool CanHandle(TMessage message)
    {
        if (_filter is null)
            return true;
        return _filter.IsMatch(message, _serviceProvider);
    }
}
