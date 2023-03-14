﻿using FSMTeleBot.Handlers.Abstractions;
using FSMTeleBot.Internal.UpdateDescriptors;
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
                typeof(IHandler<Message>)
            };
            foreach (var handlerType in handlerTypes)
            {
                var handlers = assemblies
                    .SelectMany(assembly => typeof(IHandler<>) //TODO: Fix registration
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
    }
}
