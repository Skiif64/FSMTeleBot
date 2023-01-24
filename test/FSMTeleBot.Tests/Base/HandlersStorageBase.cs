namespace FSMTeleBot.Tests.Base;

public class HandlersStorageBase
{
    protected readonly IHandlersStorage UnderTest;

    public HandlersStorageBase()
    {
        UnderTest = new HandlersStorage();
        UnderTest.Register(typeof(HandlersStorageBase).Assembly);
    }
}
