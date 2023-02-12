using FSMTeleBot.Abstractions;
using FSMTeleBot.ChatState.Abstractions;
using System.Text.RegularExpressions;
using Telegram.Bot.Types;

namespace FSMTeleBot.Filters;

public class MessageFilterAttribute : FilterAttribute
{
    public string? ContainsCommand { get; init; }
    public string? Contains { get; init; }
    public string? Regexp { get; init; }
    public IChatState? RequiredState { get; init; }
    public override bool IsMatch(object argument, IServiceProvider provider)
    {
        if (argument is not Message message)
            throw new ArgumentException($"{nameof(argument)} is not assignable to Message type");
        return IsMatch(message, provider);
    }

    public bool IsMatch(Message message, IServiceProvider provider)
    {
        if (Contains is not null
            && !message.Text.Contains(Contains, StringComparison.InvariantCultureIgnoreCase))
            return false;
        if (ContainsCommand is not null
            && !message.Text.Contains("/" + ContainsCommand, StringComparison.InvariantCultureIgnoreCase))
            return false;
        if (Regexp is not null
            && !Regex.IsMatch(message.Text, Regexp))
            return false;
        if (!IsAllowed(message.Chat.Id, message.From.Id, provider))
            return false;
        if(RequiredState is not null)
        {
            var context = (IChatContext)provider.GetService(typeof(IChatContext))!;
            if (context.CurrentState != RequiredState)
                return false;
        }

        return true;

    }
}
