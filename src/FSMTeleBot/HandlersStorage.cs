using FSMTeleBot.Filters;
using FSMTeleBot.Handlers.Abstractions;
using System.Collections.Concurrent;
using System.Reflection;
using System.Text.RegularExpressions;
using Telegram.Bot.Types;

namespace FSMTeleBot;

public class HandlersStorage : IHandlersStorage
{
    private readonly ConcurrentDictionary<Type, List<HandlerSource>> _registered = new();
    public void Register(Assembly assembly)
    {
        var handlers = GetAllImplementationOfGenericInterface(typeof(IHandler<>), assembly);
        foreach (var handler in handlers)
        {
            var key = GetGenericFromImplementedInterface(handler, typeof(IHandler<>));
            if (key is null)
                continue;
            var value = _registered.GetOrAdd(key, new List<HandlerSource>());
            value.Add(new HandlerSource(handler));
        }
    }
    public IHandler<T>? GetHandlerByUpdateType<T>(T argument) where T: Message
    {
        var source = _registered[argument.GetType()].FirstOrDefault(h => IsMatchToAttribute(h.HandlerType, argument));
        var handler = source?.Source.Invoke(new object[0]);
        return (IHandler<T>?)handler;
    }

    private static Type[] GetAllImplementationOfGenericInterface(Type baseType, Assembly assembly)
    {
        return (from x in assembly.GetTypes()
                from z in x.GetInterfaces()
                let y = x.BaseType
                where
                (
                 y != null && y.IsGenericType &&
                 baseType.IsAssignableFrom(y.GetGenericTypeDefinition())
                ) ||
                (
                 z.IsGenericType &&
                 baseType.IsAssignableFrom(z.GetGenericTypeDefinition())
                 )
                 && (!x.IsInterface && !x.IsAbstract)
                select x)
                .ToArray();
    }

    private static Type? GetGenericFromImplementedInterface(Type type, Type interfaceType)
    {
        var interfaces = type.GetInterfaces();
        foreach (var i in interfaces)
        {
            if (i.IsGenericType && i.GetGenericTypeDefinition() == interfaceType.GetGenericTypeDefinition())
                return i.GetGenericArguments()[0];
        }
        return null;
    }

    private static bool IsMatchToAttribute(Type handlerType, Message message)
    {
        //var attribute = handlerType.GetCustomAttribute<FilterAttribute>();
        //if (attribute is null)
        //    return false;
        
        //if (attribute.Contains is not null
        //    && !message.Text.Contains(attribute.Contains, StringComparison.InvariantCultureIgnoreCase))
        //    return false;
        //if(attribute.ContainsCommand is not null
        //    && !message.Text.Contains("/"+attribute.ContainsCommand, StringComparison.InvariantCultureIgnoreCase))
        //    return false;
        //if (attribute.Regexp is not null
        //    && !Regex.IsMatch(message.Text, attribute.Regexp))
        //    return false;
        
        return true;
    }
}
