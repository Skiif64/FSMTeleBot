using FSMTeleBot.Callbacks;
using FSMTeleBot.Filters;
using FSMTeleBot.Tests.Callback;
using FSMTeleBot.Tests.Utils;
using Moq;
using Telegram.Bot.Types;

namespace FSMTeleBot.Tests.Filters;
internal class CallbackQueryFilterAttributeTests
{
    private readonly IServiceProvider _serviceProvider;

    private readonly Mock<ICallbackSerializer> _serializerMock;
    private readonly CallbackQueryFilterAttribute _filterAttributeWithQueryType;
    private readonly CallbackQueryFilterAttribute _filterAttributeEmpty;

    public CallbackQueryFilterAttributeTests()
    {
        _serializerMock = new Mock<ICallbackSerializer>();

        _serviceProvider = new ServiceProviderBuilder()
            .Add(_serializerMock.Object)
            .ServiceProvider;

        _filterAttributeWithQueryType = new() { QueryType = typeof(TestCallbackQuery) };
        _filterAttributeEmpty = new();
    }

    [SetUp]
    public void ClearMocks()
    {
        _serializerMock.Reset();
    }

    [Test]
    public void WhenFilterWithQueryTypeIsMatch_TestQueryString_ThenShouldReturnTrue()
    {
        var queryString = "Test;1;2";
        _serializerMock.Setup(x => x.CanDeserializeTo(queryString, typeof(TestCallbackQuery)))
            .Returns(true);
        var callback = new CallbackQuery
        {
            Data = queryString,
        };
        var actual = _filterAttributeWithQueryType.IsMatch(callback, _serviceProvider);
        Assert.That(actual, Is.True);
    }
    [Test]
    public void WhenFilterWithQueryTypeIsMatch_UnknownQueryString_ThenShouldReturnFalse()
    {
        var queryString = "Unknown;1;2";
        _serializerMock.Setup(x => x.CanDeserializeTo(queryString, typeof(TestCallbackQuery)))
           .Returns(false);
        var callback = new CallbackQuery
        {
            Data = queryString,
        };
        var actual = _filterAttributeWithQueryType.IsMatch(callback, _serviceProvider);
        Assert.That(actual, Is.False);
    }
    [Test]
    public void WhenFilterEmptyIsMatch_TestQueryString_ThenShouldReturnTrue()
    {
        var queryString = "Test;1;2";
        _serializerMock.Setup(x => x.CanDeserializeTo(queryString, typeof(TestCallbackQuery)))
           .Returns(true);
        var callback = new CallbackQuery
        {
            Data = queryString,
        };
        var actual = _filterAttributeEmpty.IsMatch(callback, _serviceProvider);
        Assert.That(actual, Is.True);
    }
}
