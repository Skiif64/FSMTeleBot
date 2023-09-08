using System.Reflection;
using System.Text;

namespace FSMTeleBot.Callbacks;
internal class CallbackSerializer : ICallbackSerializer
{
    private const string Separator = ";";

    public bool CanDeserializeTo(string callbackData, Type type)
    {
        if (!type.IsAssignableTo(typeof(ICallbackQuery)))
            return false;

        var info = Deserialize(callbackData);
        var query = (ICallbackQuery)Activator.CreateInstance(type)!;
        return query.Header == info.Header;
    }

    public CallbackInfo Deserialize(string callbackData)
    {
        var args = callbackData.Split(Separator, StringSplitOptions.RemoveEmptyEntries);
        var header = args[0];
        var data = args[1..];
        return new CallbackInfo(header, data);
    }      
   

    public string Serialize<T>(T data) where T : ICallbackQuery
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
