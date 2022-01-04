using System.Collections.Concurrent;
using System.Reflection;

namespace Domain.Extensions;

public static class TypeExtensions
{
    private static readonly ConcurrentDictionary<string, string[]> AllKeys = new();

    public static string[] GetAllKeys(this Type type)
    {
        // ReSharper disable once HeapView.CanAvoidClosure
        return AllKeys.GetOrAdd(type.FullName, k => type
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Select(f => (string) f.GetValue(null))
            .ToArray());
    }
}