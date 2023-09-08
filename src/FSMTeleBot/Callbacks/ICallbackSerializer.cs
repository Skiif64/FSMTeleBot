namespace FSMTeleBot.Callbacks;
public interface ICallbackSerializer
{
    CallbackInfo Deserialize(string callbackData);    
    bool CanDeserializeTo(string callbackData, Type type);
    string Serialize<T>(T data) where T: ICallbackQuery;
}
