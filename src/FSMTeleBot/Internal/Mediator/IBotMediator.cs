using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("FSMTeleBot.Tests")]
namespace FSMTeleBot.Internal.Mediator;

internal interface IBotMediator
{
    Task Send(object argument, CancellationToken cancellationToken = default);
}
