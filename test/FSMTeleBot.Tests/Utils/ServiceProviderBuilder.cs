using Moq;

namespace FSMTeleBot.Tests.Utils;
internal class ServiceProviderBuilder
{
    public Mock<IServiceProvider> Mock { get; } = new();
    public IServiceProvider ServiceProvider => Mock.Object;

    public ServiceProviderBuilder()
    {
        
    }

    public ServiceProviderBuilder Add<T>(T service)
    {
        var type = typeof(T);
        Mock.Setup(x => x.GetService(type))
            .Returns(service);
        return this;
    }

    public ServiceProviderBuilder Add<T>(T service, Type type)
    {        
        Mock.Setup(x => x.GetService(type))
            .Returns(service);
        return this;
    }

    public void Reset()
        => Mock.Reset();

}
