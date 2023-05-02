using EmbedIO;
using EmbedIO.WebApi;
using Newtonsoft.Json;
using Telegram.Bot.Types;

namespace FSMTeleBot.Webhook;
public class WebhookServer
{
    private readonly WebServer _server;

    public WebhookServer(WebServer server)
    {
        _server = server;
    }

    public static WebhookServer Create(WebhookOptions options)
    {
        var server = new WebServer(opt => opt
        .WithUrlPrefix(options.Url)        
        .WithMode(HttpListenerMode.EmbedIO))
            .WithWebApi("/api", cfg => cfg
            .WithController<UpdateController>());

        return new WebhookServer(server);
    }
}
