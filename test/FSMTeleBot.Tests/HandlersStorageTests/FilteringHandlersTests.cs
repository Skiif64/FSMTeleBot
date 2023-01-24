using FSMTeleBot.Filters;
using FSMTeleBot.Tests.Base;
using FSMTeleBot.Tests.TestHandlers;
using System.Reflection;
using Telegram.Bot.Types;

namespace FSMTeleBot.Tests.HandlersStorageTests;

public class FilteringHandlersTests : HandlersStorageBase
{
    [TestCase("start")]
    [TestCase("cancel")]
    public void GetHandler_ContainsText_ShouldReturnRequiredHandler(string text)
    {       
        var message = new Message
        {
            Text = text
        };
        

        var handler = UnderTest.GetHandler(message);

        Assert.NotNull(handler);
        Assert.That(handler.GetType().GetCustomAttribute<FilterAttribute>(), Is.Not.Null );
        Assert.That(handler.GetType().GetCustomAttribute<FilterAttribute>().Contains, Is.EqualTo(text));
    }

    [TestCase("start")]
    [TestCase("cancel")]
    public void GetHandler_ContainsCommand_ShouldReturnRequiredHandler(string command)
    {        
        var message = new Message
        {
            Text = "/"+command
        };        

        var handler = UnderTest.GetHandler(message);

        Assert.NotNull(handler);
        Assert.That(handler.GetType().GetCustomAttribute<FilterAttribute>(), Is.Not.Null);
        Assert.That(handler.GetType().GetCustomAttribute<FilterAttribute>().ContainsCommand, Is.EqualTo(command));
    }
}
