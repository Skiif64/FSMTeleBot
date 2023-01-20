using FSMTeleBot.Tests.Fakes;
using FSMTeleBot.Tests.TestHandlers;
using System.Reflection;
using Telegram.Bot.Types;

namespace FSMTeleBot.Tests.HandlersStorageTests;

public class HandlersStorageTests
{
    [Test]
    public void GetHandler_StartMessage_ShouldReturnStartMessageHandler()
    {
        var storage = new HandlersStorage();
        var message = new Message
        {
            Text = "/start"
        };

        storage.Register(typeof(StartMessageHandler).Assembly);

        var handler = storage.GetHandler(message);

        Assert.NotNull(handler);
        Assert.That(handler, Is.TypeOf(typeof(StartMessageHandler)));
    }

    [Test]
    public void GetHandler_CancelMessage_ShouldReturnCancelMessageHandler()
    {
        var storage = new HandlersStorage();
        var message = new Message
        {
            Text = "/cancel"
        };

        storage.Register(typeof(CancelMessageHandler).Assembly);

        var handler = storage.GetHandler(message);

        Assert.NotNull(handler);
        Assert.That(handler, Is.TypeOf(typeof(CancelMessageHandler)));
    }
}
