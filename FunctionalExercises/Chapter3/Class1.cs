using System;
using System.Collections.Generic;
using LaYumba.Functional;

namespace Chapter3
{
    public static class MyEnum
    {
        public static Option<T> ParseOptional<T>(string text) where T : struct
        {
            return System.Enum.TryParse(text, out T val) ? F.Some(val) : F.None;
        }
    }

    public static class MyListExtensions
    {
        public static Option<int> Lookup(this List<int> list, Predicate<int> predicate)
        {
            foreach (var i in list)
            {
                if (predicate(i)) return F.Some(i);
            }
            return F.None;
        }
    }
}
