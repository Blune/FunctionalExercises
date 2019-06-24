using System;
using System.Collections.Generic;
using System.Linq;

namespace Chapter4
{
    public static class Class1
    {
        public static ISet<T2> Map<T, T2>(this ISet<T> set, Func<T, T2> map)
        {
            return set.Select(map).ToHashSet();
        }

        public static IDictionary<T2Key, T2Value> Map<TKey, TValue, T2Key, T2Value>(this IDictionary<TKey, TValue> dictionary, Func<KeyValuePair<TKey, TValue>, KeyValuePair<T2Key, T2Value>> map)
        {
            return dictionary.Select(map).ToDictionary(x => x.Key, x => x.Value);
        }
    }
}
