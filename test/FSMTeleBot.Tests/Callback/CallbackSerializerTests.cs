using FSMTeleBot.Callbacks;

namespace FSMTeleBot.Tests.Callback;
internal class CallbackSerializerTests
{
    private readonly CallbackSerializer _serializer;

    public CallbackSerializerTests()
    {
        _serializer = new();
    }

    [Test]
    public void WhenSerialize_TestQuery_ThenShouldReturnExpectedSerializedString()
    {
        var query = new TestCallbackQuery
        {
            Value1 = "1",
            Value2 = "2",
        };
        var expected = "Test;1;2";

        var actual = _serializer.Serialize(query);
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void WhenDeserialize_TestQueryString_ThenShouldReturnExpectedCallbackInfo()
    {
        var queryString = "Test;1;2";
        var expected = new CallbackInfo("Test", new[] { "1", "2" });

        var actual = _serializer.Deserialize(queryString);

        Assert.That(actual.Header, Is.EqualTo(expected.Header));
        CollectionAssert.AreEqual(expected.Data, actual.Data);
    }
    [Test]
    public void WhenDeserializeAsTestCallbackQuery_TestQueryString_ThenShouldReturnTestCallbackQuery()
    {
        var queryString = "Test;1;2";
        var expected = new TestCallbackQuery
        {
            Value1 = "1",
            Value2 = "2",
        };

        var actual = _serializer.DeserializeAs<TestCallbackQuery>(queryString);
        Assert.Multiple(() =>
        {
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Header, Is.EqualTo(expected.Header));
            Assert.That(actual.Value1, Is.EqualTo(expected.Value1));
            Assert.That(actual.Value2, Is.EqualTo(expected.Value2));
        });

    }

    [Test]
    public void WhenCanDesirializeToTestCallbackQuery_TestQueryString_ThenShouldReturnTrue()
    {
        var queryString = "Test;1;2";
        var type = typeof(TestCallbackQuery);
        var actual = _serializer.CanDeserializeTo(queryString, type);
        Assert.That(actual, Is.True);
    }

    [Test]
    public void WhenCanDesirializeToTestCallbackQuery_UnknownQueryString_ThenShouldReturnFalse()
    {
        var queryString = "Unknown;1;2";
        var type = typeof(TestCallbackQuery);
        var actual = _serializer.CanDeserializeTo(queryString, type);
        Assert.That(actual, Is.False);
    }
}
