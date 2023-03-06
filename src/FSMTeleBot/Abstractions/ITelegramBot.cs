using Microsoft.Extensions.Hosting;
using Telegram.Bot.Types;

namespace FSMTeleBot.Abstractions;

public interface ITelegramBot : IHostedService
{
    public ReceivingMode ReceivingMode { get; }
}
