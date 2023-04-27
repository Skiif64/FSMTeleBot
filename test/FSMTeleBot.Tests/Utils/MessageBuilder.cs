using Telegram.Bot.Types;

namespace FSMTeleBot.Tests.Utils;
internal class MessageBuilder
{
    public Message Message { get; }
    private MessageBuilder()
    {
        Message = new Message
        {
            Text = string.Empty,            
        };
    }    

    public static MessageBuilder Create() 
        => new();

    public MessageBuilder WithText(string text)
    {
        Message.Text = text;
        return this;
    }

    public MessageBuilder WithChat(long chatId)
    {
        Message.Chat = new Chat
        {
            Id = chatId
        };
        return this;
    }

    public MessageBuilder WithUser(long userId)
    {
        Message.From = new User
        {
            Id = userId
        };
        return this;
    }

}
