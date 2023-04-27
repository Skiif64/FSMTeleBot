namespace FSMTeleBot.Callbacks;
public interface ICallbackSerializer
{
    CallbackInfo Deserialize(string callbackData);
    T DeserializeAs<T>(string callbackData) where T: ICallbackQuery, new();
    ICallbackQuery DeserializeAs(string callbackData, Type type);
    string Serialize<T>(T data) where T: ICallbackQuery, new();
}
