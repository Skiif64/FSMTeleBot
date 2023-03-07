using Telegram.Bot;

namespace FSMTeleBot.Tests;

public class BotBuilderConfigurationTests
{
    private BotBuilderConfiguration _configuraton = null!;

	public BotBuilderConfigurationTests()
	{

	}

	[SetUp]
	public void SetupConfiguration()
	{
		_configuraton = new BotBuilderConfiguration();
	}

	[Test]
	public void WhenUseCustomTelegramBot_TelegramBotType_ThenShouldNotFall()
	{
		var type = typeof(TelegramBotClient);

		Assert.DoesNotThrow(() => _configuraton.UseCustomTelegramBotClient(type));
	}

    [Test]
    public void WhenUseCustomTelegramBot_StringType_ThenShouldThrowArgumentException()
    {
        var type = typeof(string);		
        Assert.Throws<ArgumentException>(() => _configuraton.UseCustomTelegramBotClient(type));
    }
}
