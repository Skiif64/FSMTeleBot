using FSMTeleBot.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace FSMTeleBot;

public class BotBuilder : IBotBuilder
{
    private readonly IServiceCollection _services;
    private readonly BotBuilderConfiguration _configuration;

    public BotBuilder(IServiceCollection services, BotBuilderConfiguration configuration)
    {
        _services = services;
        _configuration = configuration;
    }

    public ITelegramBot Build()
    {
        throw new NotImplementedException();
    }
}
