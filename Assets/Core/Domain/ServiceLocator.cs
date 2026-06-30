using System;
using System.Collections.Concurrent;
using Core.Domain;

public static class ServiceLocator
{
    private static ConcurrentDictionary<Type, object> _services = new();
    public static bool Register<T>(T service) => _services.TryAdd(typeof(T), service);
    public static void UnRegister<T>() => _services.TryRemove(typeof(T), out _);
    public static bool IsRegistered<T>() => _services.ContainsKey(typeof(T));
    public static T Get<T>()
    {
        if (!_services.TryGetValue(typeof(T), out var service))
        {
            Get<ILogService>().LogError($"[SERVICE LOCATOR] {typeof(T).Name} service not found.");
            throw new ArgumentException();
        }
        return (T)service;
    }
}
