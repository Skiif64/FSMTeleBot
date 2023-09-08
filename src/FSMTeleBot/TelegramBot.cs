using FSMTeleBot.Abstractions;
using FSMTeleBot.Webhook;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace FSMTeleBot;
public class TelegramBot : ITelegramBot
{
    private readonly TelegramBotOptions _options;    
    private readonly IUpdateHandler _updateHandler;
    private readonly IServiceProvider _serviceProvider;
   

    public TelegramBot(TelegramBotOptions options, IUpdateHandler updateHandler, IServiceProvider serviceProvider)
    {
        _options = options;
        _updateHandler = updateHandler;
        _serviceProvider = serviceProvider;        
    }

    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var client = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();
        if (_options.UseWebhook)
        {
            if (_options.WebhookOptions is null)
                throw new ArgumentNullException(nameof(_options.WebhookOptions));

            await client.SetWebhookAsync(_options.WebhookOptions.Url,
                _options.WebhookOptions.Certificate,
                _options.WebhookOptions.IpAdress,
                _options.WebhookOptions.MaxConnections,
                _options.ReceiverOptions.AllowedUpdates,
                _options.ReceiverOptions.ThrowPendingUpdates,
                cancellationToken)
                .ConfigureAwait(false);

            var serverFactory = scope.ServiceProvider.GetRequiredService<WebhookServerFactory>();
            using var server = serverFactory.Create(_options.WebhookOptions);
            await server.RunAsync(cancellationToken)
                .ConfigureAwait(false);
        }
        else
        {
            client.StartReceiving(_updateHandler, _options.ReceiverOptions, cancellationToken);
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken = default)
    {
        if (_options.UseWebhook)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var client = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();
            await client.DeleteWebhookAsync(_options.ReceiverOptions.ThrowPendingUpdates,
                cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
