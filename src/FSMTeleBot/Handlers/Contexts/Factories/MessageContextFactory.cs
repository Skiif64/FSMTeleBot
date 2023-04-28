using FSMTeleBot.Handlers.Abstractions;
using FSMTeleBot.States.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace FSMTeleBot.Handlers.Contexts.Factories;
public class MessageContextFactory : IHandlerContextFactory<Message>
{
    private readonly IChatContextFactory _chatContextFactory;
    private readonly ITelegramBotClient _client;
    public MessageContextFactory(IChatContextFactory chatContextFactory, ITelegramBotClient client)
    {
        _chatContextFactory = chatContextFactory;
        _client = client;
    }
    public async Task<IHandlerContext<Message>> CreateAsync(Message data, CancellationToken cancellationToken = default)
    {
        var chatContext = await _chatContextFactory.GetContextAsync(data, cancellationToken);

        return new MessageContext(data, _client, chatContext);
    }
}
