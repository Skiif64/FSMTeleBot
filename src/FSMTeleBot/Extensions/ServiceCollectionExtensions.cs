using FSMTeleBot.Abstractions;
using FSMTeleBot.Internal.DependencyInjectionExtensions;
using FSMTeleBot.Internal.Mediator;
using FSMTeleBot.States;
using FSMTeleBot.States.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Polling;

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
        return services;
    }

    private static IServiceCollection AddHandlers(this IServiceCollection services, BotBuilderConfiguration configuration) 
        =>services.AddHandlers(configuration.Assemblies);
    private static IServiceCollection AddUpdateDescriptors(this IServiceCollection services, BotBuilderConfiguration configuration)
        => services.AddUpdateDescriptors(configuration.Assemblies);



    private static IServiceCollection AddRequiredServices(this IServiceCollection services, BotBuilderConfiguration configuration)
    {
        services.AddTransient(typeof(ITelegramBotClient), sp => new TelegramBotClient(
            sp.GetRequiredService<TelegramBotOptions>()
            .BotOptions.Token)); //TODO: implement custom telegram client, maybe a factory
        services.AddScoped(typeof(IChatStateStorage), configuration.StateStorageImplementationType);
        services.AddScoped(typeof(IChatMemberService), configuration.MemberServiceImplementationType);
        services.AddSingleton(typeof(IUpdateHandler), configuration.UpdateHandlerImplementationType);
        services.AddSingleton<IBotMediator, BotMediator>(); //TODO: Singleton???
        services.AddTransient<IChatContext>(sp => sp
        .GetRequiredService<IChatContextFactory>().GetContextAsync(default).Result);
        services.AddHostedService<TelegramBot>();
        return services;
    }
}
