using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("FSMTeleBot.Tests")]
namespace FSMTeleBot.Internal.Mediator;

internal interface IBotMediator
{
    /// <summary>
    /// Send Telegram update to single handler
    /// </summary>
    /// <typeparam name="T">Telegram update type</typeparam>
    /// <param name="argument">Telegram update message</param>    
    /// <returns></returns>
    Task Send<T>(T argument, CancellationToken cancellationToken = default);
}
