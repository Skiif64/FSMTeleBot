﻿using FSMTeleBot.Filters;
using FSMTeleBot.Handlers.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace FSMTeleBot.Internal;

internal abstract class HandlerWrapper
{
    public abstract Task Handle(object argument, IServiceProvider provider, CancellationToken cancellationToken = default);
    public abstract bool CanHandle(object argument);
}
internal class HandlerWrapper<TMessage> : HandlerWrapper
{
    private readonly Type _handlerType;
    private readonly FilterAttribute? _filter;

    public HandlerWrapper(Type handlerType)
    {
        //TODO: Validation
        _handlerType = handlerType;
        _filter = _handlerType.GetCustomAttribute<FilterAttribute>();            
    }

    public override Task Handle(object argument, IServiceProvider provider, CancellationToken cancellationToken = default)
    {        
        if(argument is not TMessage message)
            throw new ArgumentException(nameof(argument));

        return Handle(message, provider, cancellationToken);
    }
    
    public Task Handle(TMessage message, IServiceProvider provider, CancellationToken cancellationToken = default)
    {
        var handler = (IHandler<TMessage>)Activator.CreateInstance(_handlerType); //TODO: Remove this boilerplate
        return handler.HandleAsync(message, cancellationToken);
    }
    
    public override bool CanHandle(object argument)
    {
        if (argument is not TMessage message)
            throw new ArgumentException(nameof(argument));
        return CanHandle(message);
    }

    public bool CanHandle(TMessage message)
    {
        //TODO: Validation
        return true;
    }
}
