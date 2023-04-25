using FSMTeleBot.Handlers.Abstractions;

namespace FSMTeleBot.Handlers.Contexts;
public class HandlerContext<TData> : IHandlerContext<TData>
{
    public TData Data { get; }
    public HandlerContext(TData data)
    {
        Data = data;
    }
}
