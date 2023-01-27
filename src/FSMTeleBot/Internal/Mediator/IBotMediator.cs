using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("FSMTeleBot.Tests")]
namespace FSMTeleBot.Internal.Mediator;

internal interface IBotMediator
{
    Task Send<T>(T argument, CancellationToken cancellationToken = default);
}
