using Telegram.Bot.Types.Enums;

namespace FSMTeleBot.Filters;

public abstract class FilterAttribute : Attribute
{
    public ChatMemberStatus? Allowed { get; init; }

    public FilterAttribute()
    {
        
    }

    public abstract bool IsMatch(object argument, IServiceProvider provider);
    
}
