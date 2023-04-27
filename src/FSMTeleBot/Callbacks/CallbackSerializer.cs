using System.Reflection;
using System.Text;

namespace FSMTeleBot.Callbacks;
internal class CallbackSerializer : ICallbackSerializer
{
    private const string Separator = ";";

    public CallbackInfo Deserialize(string callbackData)
    {
        var args = callbackData.Split(Separator, StringSplitOptions.RemoveEmptyEntries);
        var header = args[0];
        var data = args[1..];
        return new CallbackInfo(header, data);
    }

    public T DeserializeAs<T>(string callbackData) where T : ICallbackQuery, new()
        => (T)DeserializeAs(callbackData, typeof(T));

    public ICallbackQuery DeserializeAs(string callbackData, Type type)
    {//TODO: Property caching?
        if (!type.IsAssignableTo(typeof(ICallbackQuery)))
            throw new ArgumentException("Type is not a ICallbackQuery", nameof(type));

        var info = Deserialize(callbackData);

        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        var query = Activator.CreateInstance(type);
        var dataProperties = properties[1..];

        int i = 0;
        foreach (var property in dataProperties)
        {//TODO: add value converters
            property.SetValue(query, info.Data[i]);
            i++;
        }

        return (ICallbackQuery)query!;
    }

    public string Serialize<T>(T data) where T : ICallbackQuery, new()
    {
        var type = typeof(T);
        var builder = new StringBuilder(64);
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var headerProperty = properties[0];
        var dataProperties = properties[1..];
        builder.Append(headerProperty.GetValue(data));
        foreach (var property in dataProperties)
        {
            builder.Append(';');
            builder.Append(property.GetValue(data));
        }
        return builder.ToString();
    }
}
