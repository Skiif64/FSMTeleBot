using FSMTeleBot.Abstractions;
using FSMTeleBot.Internal;
using FSMTeleBot.Internal.DependencyInjectionExtensions;
using FSMTeleBot.Internal.Dispatcher;
using FSMTeleBot.Services;
using FSMTeleBot.States;
using FSMTeleBot.States.Abstractions;
using FSMTeleBot.Webhook;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace FSMTeleBot.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFSMTeleBot(this IServiceCollection services, Action<BotBuilderConfiguration> configAction)
    {
        var configuration = new BotBuilderConfiguration();
        configAction.Invoke(configuration);
        configuration.OptionsRegistration?.Invoke(services);
        services.AddHandlers(configuration);
        services.AddRequiredServices(configuration);
        services.AddUpdateDescriptors(configuration);
        services.AddHandlerContextFactories(configuration);
        return services;
    }

    private static IServiceCollection AddHandlers(
        this IServiceCollection services, BotBuilderConfiguration configuration) 
        =>services.AddHandlers(configuration.Assemblies);
    private static IServiceCollection AddUpdateDescriptors(
        this IServiceCollection services, BotBuilderConfiguration configuration)
        => services.AddUpdateDescriptors(configuration.Assemblies);

    private static IServiceCollection AddHandlerContextFactories(
        this IServiceCollection services, BotBuilderConfiguration configuration)
        => services.AddHandlerContextFactories(configuration.Assemblies);



    private static IServiceCollection AddRequiredServices(this IServiceCollection services, BotBuilderConfiguration configuration)
    {
        services.AddTransient(typeof(ITelegramBotClient), sp => new TelegramBotClient(
            sp.GetRequiredService<TelegramBotOptions>()
            .BotOptions)); //TODO: implement custom telegram client, maybe a factory
        services.AddScoped(typeof(IChatStateStorage), configuration.StateStorageImplementationType);
        services.AddScoped(typeof(IChatMemberService), configuration.MemberServiceImplementationType);
        services.AddSingleton(typeof(IUpdateHandler), configuration.UpdateHandlerImplementationType);
        services.AddSingleton<IBotDispatcher, BotDispatcher>(); //TODO: Singleton???
        services.AddScoped<IChatContextFactory, ChatContextFactory>();
        services.AddScoped<InternalUpdateHandler>();//TODO: remove?
        services.AddHostedService<TelegramBot>();
        services.AddTransient<WebhookServerFactory>();
        return services;
    }
}
