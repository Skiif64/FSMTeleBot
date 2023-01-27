namespace FSMTeleBot.Filters;

public abstract class FilterAttribute : Attribute
{   
    public bool RequireAdmin { get; init; }

    public FilterAttribute()
    {
        
    }

    public abstract bool IsMatch(object argument);
    
}
