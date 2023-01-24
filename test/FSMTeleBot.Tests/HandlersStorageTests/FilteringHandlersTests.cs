using FSMTeleBot.Filters;
using FSMTeleBot.Tests.Base;
using FSMTeleBot.Tests.Fakes;
using FSMTeleBot.Tests.TestHandlers;
using System.Reflection;
using Telegram.Bot.Types;

namespace FSMTeleBot.Tests.HandlersStorageTests;

public class FilteringHandlersTests : HandlersStorageBase
{
    [TestCase("start", typeof(StartMessageHandler))]
    [TestCase("cancel", typeof(CancelMessageHandler))]
    public void GetHandler_ContainsText_ShouldReturnRequiredHandler(string text, Type expectedType)
    {       
        var message = new Message
        {
            Text = text
        };        

        var handler = UnderTest.GetHandlerByUpdateType(message);

        Assert.IsNotNull(handler);
        Assert.That(handler.GetType().GetCustomAttribute<FilterAttribute>(), Is.Not.Null );
        Assert.That(handler.GetType().GetCustomAttribute<FilterAttribute>().Contains, Is.EqualTo(text));
        Assert.That(handler, Is.TypeOf(expectedType));
    }

    [TestCase("/самшит")]
    public void GetHandler_ContainsText_ShouldReturnNull(string text)
    {
        var message = new Message
        {
            Text = text
        };

        var handler = UnderTest.GetHandlerByUpdateType(message);

        Assert.IsNull(handler);        
    }

    [TestCase("start", typeof(StartCommandMessageHandler))]
    [TestCase("cancel", typeof(CancelCommandMessageHandler))]
    public void GetHandler_ContainsCommand_ShouldReturnRequiredHandler(string command, Type expectedType)
    {        
        var message = new Message
        {
            Text = "/"+command
        };        

        var handler = UnderTest.GetHandlerByUpdateType(message);

        Assert.IsNotNull(handler);
        Assert.That(handler.GetType().GetCustomAttribute<FilterAttribute>(), Is.Not.Null);
        Assert.That(handler.GetType().GetCustomAttribute<FilterAttribute>().ContainsCommand, Is.EqualTo(command));
        Assert.That(handler, Is.TypeOf(expectedType));
    }

    [TestCase("самшит")]
    public void GetHandler_ContainsCommand_ShouldReturnNull(string command)
    {
        var message = new Message
        {
            Text = "/" + command
        };

        var handler = UnderTest.GetHandlerByUpdateType(message);

        Assert.IsNull(handler);        
    }

    [TestCase("https://localhost:8000", typeof(UrlMessageHandler))]
    [TestCase("+78005553535", typeof(PhoneNubmerMessageHandler))]
    public void GetHandler_Regexp_ShouldReturnRequiredHandler(string text, Type expectedType)
    {
        var message = new Message
        {
            Text = text
        };

        var handler = UnderTest.GetHandlerByUpdateType(message);

        Assert.IsNotNull(handler);
        Assert.That(handler.GetType().GetCustomAttribute<FilterAttribute>(), Is.Not.Null);
        Assert.That(handler, Is.TypeOf(expectedType));
    }

    [TestCase("толстожопый-снеговик")]
    public void GetHandler_Regexp_ShouldReturnNull(string text)
    {
        var message = new Message
        {
            Text = text
        };

        var handler = UnderTest.GetHandlerByUpdateType(message);

        Assert.IsNull(handler);        
    }
}
