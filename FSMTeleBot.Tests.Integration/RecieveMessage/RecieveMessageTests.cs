using Microsoft.Extensions.DependencyInjection;
using FSMTeleBot.Extensions;
using dotenv.net;

namespace FSMTeleBot.Tests.Integration.RecieveMessage;
internal class RecieveMessageTests : IFix
{
}

internal class RecieveMessageHostBuilder : HostBuilder
{
    public override IServiceProvider BuildServiceProvider()
    {
        var collection = new ServiceCollection();
        collection.AddFSMTeleBot(config =>
        {
            config.AddAssemblyFrom<RecieveMessageHostBuilder>();
            config.UseBuiltinOptions(new TelegramBotOptions(false,
                new Telegram.Bot.TelegramBotClientOptions
                {
                    Token = 
                }))
        });
    }
}
