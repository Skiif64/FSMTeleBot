using FSMTeleBot.Abstractions;
using FSMTeleBot.Services;
using System.Reflection;
using Telegram.Bot;

namespace FSMTeleBot;

public class BotBuilderConfiguration
{
    public List<Assembly> Assemblies { get; private set; } = new();
    public Type TelegramBotClientImplementationType { get; private set; } = typeof(TelegramBotClient);
    public Type StateStorageImplementationType { get; private set; } = typeof(InMemoryStateStorage);
    public Type MemberServiceImplementationType { get; private set; } = typeof(ChatMemberService);

    internal BotBuilderConfiguration()
    {

    }
        
    public BotBuilderConfiguration AddAssemblyFrom<T>() 
        => AddAssemblyFrom(typeof(T));

    public BotBuilderConfiguration AddAssemblyFrom(Type type) 
        => AddAssembly(type.Assembly);

    public BotBuilderConfiguration UseCustomTelegramBotClient<TClient>() where TClient : ITelegramBotClient
        => UseCustomTelegramBotClient(typeof(TClient));

    public BotBuilderConfiguration UseCustomStateStorage<TStorage>() where TStorage : IChatStateStorage
        => UseCustomStateStorage(typeof(TStorage));

    public BotBuilderConfiguration UseCustomMemberService<TService>() where TService : IChatMemberService
        => UseCustomMemberService(typeof(TService));

    public BotBuilderConfiguration AddAssembly(Assembly assembly)
    {
        Assemblies.Add(assembly);
        return this;
    }

    public BotBuilderConfiguration UseCustomTelegramBotClient(Type clientType)
    {
        if (!clientType.IsAssignableTo(typeof(ITelegramBotClient)))
            throw new ArgumentException($"Type {clientType.GetType().Name} is not implementing ITelegramBotClient interface.");
        TelegramBotClientImplementationType= clientType;
        return this;
    }

    public BotBuilderConfiguration UseCustomStateStorage(Type storageType)
    {
        if (!storageType.IsAssignableTo(typeof(IChatStateStorage)))
            throw new ArgumentException($"Type {storageType.GetType().Name} is not implementing IChatStateStorage interface.");
        StateStorageImplementationType= storageType;
        return this;
    }

   public BotBuilderConfiguration UseCustomMemberService(Type serviceType)
    {
        if (!serviceType.IsAssignableTo(typeof(IChatMemberService)))
            throw new ArgumentException($"Type {serviceType.GetType().Name} is not implementing IChatMemberService interface.");
        MemberServiceImplementationType = serviceType;
        return this;
    }
}
