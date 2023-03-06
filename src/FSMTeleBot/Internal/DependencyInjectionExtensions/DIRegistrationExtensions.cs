using FSMTeleBot.Handlers.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Telegram.Bot.Types;

namespace FSMTeleBot.Internal.DependencyInjectionExtensions
{
    internal static class DIRegistrationExtensions
    {
        public static IServiceCollection AddHandlers(this IServiceCollection services, params Assembly[] assemblies)
        {
            var handlerTypes = new[]
            {
                typeof(IHandler<Message>)
            };
            foreach (var handlerType in handlerTypes)
            {
                var handlers = assemblies
                    .SelectMany(assembly => typeof(IHandler<>)
                    .GetAllImplementationOfGenericInterface(assembly));
                foreach (var handler in handlers)
                {
                    services.AddTransient(handlerType, handler);
                }
            }

            return services;
        }        
    }
}
