namespace FSMTeleBot.Internal;

internal static class IEnumerableExtensions
{
    public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
    {
        if (enumerable == null)
            throw new ArgumentNullException(nameof(enumerable));
        if (action == null)
            throw new ArgumentNullException(nameof(action));
        foreach(var item in enumerable)
        {
            action(item);
            yield return item;
        }
    }
}
