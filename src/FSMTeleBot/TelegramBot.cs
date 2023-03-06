using FSMTeleBot.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace FSMTeleBot;
public enum ReceivingMode
{
    LongPolling,
    Webhook
}
public class TelegramBot : ITelegramBot
{
    private readonly TelegramBotOptions _options;
    private readonly IUpdateHandler _updateHandler;
    private readonly IServiceProvider _serviceProvider;
    public ReceivingMode ReceivingMode { get; }

    public TelegramBot(TelegramBotOptions options, IUpdateHandler updateHandler, IServiceProvider serviceProvider)
    {
        _options = options;
        _updateHandler = updateHandler;
        _serviceProvider = serviceProvider;

        if (_options.UseWebhook)
            ReceivingMode = ReceivingMode.Webhook;
        else
            ReceivingMode = ReceivingMode.LongPolling;
    }

    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var client = (ITelegramBotClient)scope.ServiceProvider.GetService(typeof(ITelegramBotClient))!;
        if (ReceivingMode == ReceivingMode.LongPolling)
        {
            client.StartReceiving(_updateHandler, _options.ReceiverOptions, cancellationToken);
        }
        else
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
            //TODO: Start a server
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken = default)
    {
        if (ReceivingMode == ReceivingMode.Webhook)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var client = (ITelegramBotClient)scope.ServiceProvider.GetService(typeof(ITelegramBotClient))!;
            await client.DeleteWebhookAsync(_options.ReceiverOptions.ThrowPendingUpdates,
                cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
