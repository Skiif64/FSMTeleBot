using FSMTeleBot.Handlers.Abstractions;
using System.Reflection;
using Telegram.Bot.Types;

namespace FSMTeleBot;

public interface IHandlersStorage
{
    void Register(Assembly assembly);
    IHandler<T>? GetHandlerByUpdateType<T>(T argument) where T : Message;
}
