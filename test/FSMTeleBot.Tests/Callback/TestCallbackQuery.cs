using FSMTeleBot.Callbacks;

namespace FSMTeleBot.Tests.Callback;
internal class TestCallbackQuery : ICallbackQuery
{
    public string Header { get; set; } = "Test"; //TODO: remove setter

    public string Value1 { get; set; } = string.Empty;
    public string Value2 { get; set; } = string.Empty;
}
