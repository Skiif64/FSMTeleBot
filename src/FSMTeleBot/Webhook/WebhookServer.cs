using EmbedIO;
using EmbedIO.WebApi;
using Newtonsoft.Json;
using Telegram.Bot.Types;

namespace FSMTeleBot.Webhook;
public class WebhookServer
{
    private readonly IServiceProvider _serviceProvider;

    public WebhookServer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public WebServer Create(WebhookOptions options)
    {
        var server = new WebServer(opt => opt
        .WithUrlPrefix(options.Url)        
        .WithMode(HttpListenerMode.EmbedIO))
            .WithWebApi("/api", cfg => cfg
            .WithController(() => (UpdateController)_serviceProvider
            .GetService(typeof(UpdateController))!));

        return server;
    }
}
