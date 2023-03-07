using FSMTeleBot.Abstractions;
using FSMTeleBot.Internal.DependencyInjectionExtensions;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;

namespace FSMTeleBot.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFSMTeleBot(this IServiceCollection services, Action<BotBuilderConfiguration> configAction)
    {
        var configuration = new BotBuilderConfiguration();
        configAction.Invoke(configuration);
        services.AddHandlers(configuration);
        services.AddRequiredServices(configuration);
        return services;
    }

    private static IServiceCollection AddHandlers(this IServiceCollection services, BotBuilderConfiguration configuration)
    {
        services.AddHandlers(configuration.Assemblies);
        return services;
    }

    private static IServiceCollection AddRequiredServices(this IServiceCollection services, BotBuilderConfiguration configuration)
    {
        services.AddTransient(typeof(ITelegramBotClient), configuration.TelegramBotClientImplementationType);
        services.AddScoped(typeof(IChatStateStorage), configuration.StateStorageImplementationType);
        services.AddScoped(typeof(IChatMemberService), configuration.MemberServiceImplementationType);
        services.AddHostedService<TelegramBot>();
        return services;
    }
}
