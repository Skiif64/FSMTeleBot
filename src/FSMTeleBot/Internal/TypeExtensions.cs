using System.Reflection;

namespace FSMTeleBot.Internal;

internal static class TypeExtensions
{
    public static bool IsConcrete(this Type type) => !type.IsInterface && !type.IsAbstract;

    public static Type GetGenericFromImplementedInterface(this Type type, Type implementedInterface)
    {
        var interfaces = type.GetInterfaces();
        foreach (var @interface in interfaces)
        {
            if (@interface.IsGenericType && @interface.GetGenericTypeDefinition() == implementedInterface.GetGenericTypeDefinition())
                return @interface.GetGenericArguments()[0];
        }
        throw new InvalidOperationException();//TODO: normal exception
    }

    public static Type[] GetAllImplementationOfGenericInterface(this Type baseType, Assembly assembly)
    {
        return (from x in assembly.GetTypes()
                from z in x.GetInterfaces()
                let y = x.BaseType
                where
                (
                 y != null && y.IsGenericType &&
                 baseType.IsAssignableFrom(y.GetGenericTypeDefinition())
                ) ||
                (
                 z.IsGenericType &&
                 baseType.IsAssignableFrom(z.GetGenericTypeDefinition())
                 )
                 && x.IsConcrete()
                select x)
                .ToArray();
    }

    //public static IEnumerable<Type> GetAssignablesToGenericInterface(this Type interfaceType, Assembly assembly)
    //{
    //    if (!interfaceType.IsInterface || !interfaceType.IsGenericType)
    //        throw new InvalidOperationException($"{nameof(interfaceType)} is not interface or not generic");

    //    var genericTypes = interfaceType.GetGenericArguments();
    //    var types = assembly.GetTypes();
    //    foreach(var type in types)
    //    {
    //        if(type.IsConcrete() && )
    //    }

}




