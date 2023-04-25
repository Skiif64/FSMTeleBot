using FSMTeleBot.Filters;
using FSMTeleBot.Handlers.Abstractions;
using FSMTeleBot.Handlers.Contexts;
using System.ComponentModel;


namespace FSMTeleBot.Internal;

internal abstract class HandlerWrapper
{
    public abstract Task HandleAsync(object argument, IServiceProvider provider, CancellationToken cancellationToken = default);
    public abstract bool CanHandle(object argument);
}
internal class HandlerDescriptor<TData> : HandlerWrapper
{
    private readonly Type _handlerType;
    private readonly IHandler<TData, IHandlerContext<TData>> _handler;
    private readonly FilterAttribute? _filter;
    private readonly IServiceProvider _serviceProvider;

    public HandlerDescriptor(IHandler<TData, IHandlerContext<TData>> handler, IServiceProvider serviceProvider)
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
    public override Task HandleAsync(object data, IServiceProvider provider, CancellationToken cancellationToken = default)
    {
        if (data is not TData message)
            throw new ArgumentException(nameof(data));

        return HandleAsync(message, provider, cancellationToken);
    }

    public Task HandleAsync(TData data, IServiceProvider provider, CancellationToken cancellationToken = default)
    {
        var context = BuildContext(data);
        return _handler.HandleAsync(context, cancellationToken);

    }

    public override bool CanHandle(object argument)
    {
        if (argument is not TData message)
            throw new ArgumentException(nameof(argument));
        return CanHandle(message);
    }

    public bool CanHandle(TData data)
    {
        if (_filter is null)
            return true;
        if (data is null)//TODO: false???
            return false;
        return _filter.IsMatch(data, _serviceProvider);
    }

    private IHandlerContext<TData> BuildContext(TData data)
    {//TODO: normal Context
        return new HandlerContext<TData>(data);
    }
}
