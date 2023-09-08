using System.Reflection;

namespace FSMTeleBot.Callbacks;
public static class CallbackSerializerExtensions
{
    public static T DeserializeAs<T>(this ICallbackSerializer serializer, string callbackData) 
        where T : ICallbackQuery, new()
    {
        var info = serializer.Deserialize(callbackData);
        var type = typeof(T);
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        var query = Activator.CreateInstance<T>();

        var dataProperties = properties
            .Where(prop => prop.Name != nameof(ICallbackQuery.Header));

        int i = 0;
        foreach (var property in dataProperties)
        {//TODO: add value converters
            property.SetValue(query, info.Data[i]);
            i++;
        }

        return query!;
    }

    public static bool CanDeserializeTo<T>(this ICallbackSerializer serializer, string callbackData)
        where T: ICallbackQuery
    {
        var type = typeof(T);
        if (!type.IsAssignableTo(typeof(ICallbackQuery)))
            return false;

        var info = serializer.Deserialize(callbackData);
        var query = (ICallbackQuery)Activator.CreateInstance(type)!;
        return query.Header == info.Header;
    }
}
