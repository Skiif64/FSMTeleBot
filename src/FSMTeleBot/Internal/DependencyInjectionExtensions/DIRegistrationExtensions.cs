using FSMTeleBot.Handlers.Abstractions;
using FSMTeleBot.States.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Telegram.Bot.Types;

namespace FSMTeleBot.Internal.DependencyInjectionExtensions
{
    internal static class DIRegistrationExtensions
    {
        public static IServiceCollection AddHandlers(this IServiceCollection services, IEnumerable<Assembly> assemblies)
        {
            var handlerTypes = new[]
            {
                typeof(IHandler<Message, IHandlerContext<Message>>)
            };
            foreach (var handlerType in handlerTypes)
            {
                var handlers = assemblies
                    .SelectMany(assembly => typeof(IHandler<,>) //TODO: Fix registration
                    .GetAllImplementationOfGenericInterface(assembly));
                foreach (var handler in handlers)
                {
                    services.AddTransient(handlerType, handler);
                }
            }

            return services;
        }

        public static IServiceCollection AddUpdateDescriptors(this IServiceCollection services, IEnumerable<Assembly> assemblies)
        {
            var descriptors = typeof(IUpdateDescriptor).GetConcreteImplementationOfInterface(typeof(IUpdateDescriptor).Assembly);
            foreach (var descriptor in descriptors)
            {
                services.AddTransient(typeof(IUpdateDescriptor), descriptor);         
            }
            return services;
        }

        public static IServiceCollection AddHandlerContextFactories(this IServiceCollection services, IEnumerable<Assembly> assemblies)
        {
            var factories = assemblies
                .SelectMany(ass => typeof(IHandlerContextFactory<>)
                .GetAllImplementationOfGenericInterface(ass));
            foreach(var factory in factories)
            {
                var generic = factory.GetGenericArguments()[0];
                services.AddTransient(
                    typeof(IHandlerContextFactory<>).MakeGenericType(generic), factory);
            }
            return services;
        }
    }
}
