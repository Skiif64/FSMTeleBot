using FSMTeleBot.ChatState;
using FSMTeleBot.ChatState.Abstractions;
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

        public static IServiceCollection AddStateGroups(this IServiceCollection services, params Assembly[] assemblies) 
        {
            var stateTypes = assemblies
                .SelectMany(assembly => assembly.DefinedTypes)                
                .Where(type => type.IsAssignableTo(typeof(IChatStateGroup)));

            foreach(var stateType in stateTypes)
            {
                var state = (ChatStateGroup)Activator.CreateInstance(stateType)!;
                state.InitState(state);
                services.AddSingleton(typeof(IChatStateGroup), state);
            }  

            return services;
        }
    }
}
