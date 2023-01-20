using FSMTeleBot.Handlers.Abstractions;
using Telegram.Bot.Types;

namespace FSMTeleBot;

public interface IHandlersStorage
{
    IHandler<T>? GetHandler<T>(T argument) where T : Message;
}
