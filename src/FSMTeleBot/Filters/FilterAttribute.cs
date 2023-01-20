namespace FSMTeleBot.Filters;

public class FilterAttribute : Attribute
{
    public string? ContainsCommand { get; init; }
    public string? Contains { get; init; }
    public string? Regexp { get; init; }
    public bool RequireAdmin { get; init; }

    public FilterAttribute()
    {
        
    }
}
