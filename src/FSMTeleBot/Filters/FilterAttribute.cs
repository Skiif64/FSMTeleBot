namespace FSMTeleBot.Filters;

public class FilterAttribute : Attribute
{
    public string? ContainsCommand { get; set; }
    public string? Contains { get; set; }
    public string? Regexp { get; set; }
    public bool RequireAdmin { get; set; }

    public FilterAttribute(string? containsCommand = null,
        string? contains = null,
        string? regexp = null,
        bool requireAdmin = false)
    {
        ContainsCommand = containsCommand;
        Contains = contains;
        Regexp = regexp;
        RequireAdmin = requireAdmin;
    }
}
