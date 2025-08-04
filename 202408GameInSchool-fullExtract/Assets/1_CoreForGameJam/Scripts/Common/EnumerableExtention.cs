using System;
using System.Collections.Generic;
using Unity.VisualScripting;

public static class EnumerableExtension
{
    // Extension method to check if an IEnumerable is empty
    public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
    {
        foreach (var item in enumerable)
        {
            action(item);
        }
    }

    public static void ForEachWithIndex<T>(this IEnumerable<T> enumerable, Action<int, T> action)
    {
        var index = 0;
        foreach (var item in enumerable)
        {
            action(index, item);
            index++;
        }
    }

    public static void Repeat(this int count, Action action)
    {
        for (var i = 0; i < count; i++)
        {
            action();
        }
    }

    // Extension method to shuffle the elements of an IEnumerable
    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> enumerable)
    {
        var random = new Random();
        var list = new List<T>(enumerable);
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
        return list;
    }
}