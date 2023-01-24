using FSMTeleBot.Tests.Base;
using FSMTeleBot.Tests.Fakes;
using FSMTeleBot.Tests.TestHandlers;
using System.Reflection;
using Telegram.Bot.Types;

namespace FSMTeleBot.Tests.HandlersStorageTests;

public class HandlersStorageTests : HandlersStorageBase
{
    [Test]
    public void GetHandler_StartMessage_ShouldReturnStartMessageHandler()
    {        
        var message = new Message
        {
            Text = "/start"
        };        

        var handler = UnderTest.GetHandlerByUpdateType(message);

        Assert.NotNull(handler);
        Assert.That(handler, Is.TypeOf(typeof(StartCommandMessageHandler)));
    }

    [Test]
    public void GetHandler_CancelMessage_ShouldReturnCancelMessageHandler()
    {        
        var message = new Message
        {
            Text = "/cancel"
        };        

        var handler = UnderTest.GetHandlerByUpdateType(message);

        Assert.NotNull(handler);
        Assert.That(handler, Is.TypeOf(typeof(CancelCommandMessageHandler)));
    }
}
