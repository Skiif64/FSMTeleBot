using System.Text.RegularExpressions;
using Telegram.Bot.Types;

namespace FSMTeleBot.Filters;

public class MessageFilterAttribute : FilterAttribute
{
    public string? ContainsCommand { get; init; }
    public string? Contains { get; init; }
    public string? Regexp { get; init; }
    public override bool IsMatch(object argument)
    {
        if (argument is not Message message)
            throw new ArgumentException($"{nameof(argument)} is not assignable to Message type");
        return IsMatch(message);
    }

    public bool IsMatch(Message message)
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

        return true;

    }
}
