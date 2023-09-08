using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.InputFiles;

namespace FSMTeleBot;

public class TelegramBotOptions
{
    public bool UseWebhook { get; init; }
    public TelegramBotClientOptions BotOptions { get; init; }
    public ReceiverOptions ReceiverOptions { get; init; } = new();
    public WebhookOptions? WebhookOptions { get; init; }

    public TelegramBotOptions(bool useWebhook, TelegramBotClientOptions botOptions, ReceiverOptions receiverOptions)
    {
        UseWebhook = useWebhook;
        BotOptions = botOptions;
        ReceiverOptions = receiverOptions;
    }

    public TelegramBotOptions(bool useWebhook, TelegramBotClientOptions botOptions, ReceiverOptions receiverOptions, WebhookOptions webhookOptions)
        : this(useWebhook, botOptions, receiverOptions)
    {
        WebhookOptions = webhookOptions;
    }
}

public class WebhookOptions
{
    public string Url { get; init; }
    public InputFileStream? Certificate { get; init; }
    public string? IpAdress { get; init; }
    public int? MaxConnections { get; init; }

    public WebhookOptions(string url)
    {
        Url = url;
    }

    public WebhookOptions(string url,
                          InputFileStream? certificate = null,
                          string? ipAdress = null,
                          int? maxConnections = null) : this(url)
    {
        Certificate = certificate;
        IpAdress = ipAdress;
        MaxConnections = maxConnections;
    }
}