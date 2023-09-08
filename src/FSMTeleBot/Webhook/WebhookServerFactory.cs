using EmbedIO;
using EmbedIO.WebApi;
using Newtonsoft.Json;
using Telegram.Bot.Types;

namespace FSMTeleBot.Webhook;
public class WebhookServerFactory //TODO: add interface
{
    private readonly IServiceProvider _serviceProvider;

    public WebhookServerFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public WebServer Create(WebhookOptions options)
    {
        var server = new WebServer(opt => opt
        .WithUrlPrefix("http://localhost:8080/")
        .WithMode(HttpListenerMode.EmbedIO))
            .WithWebApi("/api", cfg => cfg            
            .WithController<UpdateController>());

        return server;
    }
}
