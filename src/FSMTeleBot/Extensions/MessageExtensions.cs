using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FSMTeleBot.Extensions;

public static class MessageExtensions
{
    public static Task AnswerAsync(this Message message, ITelegramBotClient client, string text, CancellationToken cancellationToken = default)
        => client.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: text,
            cancellationToken: cancellationToken);
    public static Task AnswerAsync(this Message message, ITelegramBotClient client, string text, IReplyMarkup markup, CancellationToken cancellationToken = default)
        => client.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: text,
            replyMarkup: markup,
            cancellationToken: cancellationToken);

    public static Task ReplyAsync(this Message message, ITelegramBotClient client, string text, CancellationToken cancellationToken = default)
        => client.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: text,
            replyToMessageId: message.MessageId,
            cancellationToken: cancellationToken);
    public static Task ReplyAsync(this Message message, ITelegramBotClient client, string text, IReplyMarkup markup, CancellationToken cancellationToken = default)
        => client.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: text,
            replyMarkup: markup,
            replyToMessageId: message.MessageId,
            cancellationToken: cancellationToken);
}
