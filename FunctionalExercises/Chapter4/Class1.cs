using System;
using System.Collections.Generic;
using System.Linq;
using LaYumba.Functional;

namespace Chapter4
{
    public static class Chapter4Extensions
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

    public class Person
    {
        public string FirstName { get; }
        public string LastName { get; }

        public decimal Earnings { get; set; }
        public Option<int> Age { get; set; }

        public Person() { }

        public Person(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
    }

    public struct WorkPermit
    {
        public string Number { get; set; }
        public DateTime Expiry { get; set; }
    }

    public class Employee
    {
        public string Id { get; set; }
        public Option<WorkPermit> WorkPermit { get; set; }

        public DateTime JoinedOn { get; }
        public Option<DateTime> LeftOn { get; }
    }
}
