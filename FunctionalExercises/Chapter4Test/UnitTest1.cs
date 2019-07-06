using System;
using System.Collections.Generic;
using Chapter4;
using LaYumba.Functional;
using NUnit.Framework;
using System.Linq;

namespace Tests
{
    public class Tests
    {
        // 1 Implement Map for ISet<T> and IDictionary<K, T>. (Tip: start by writing down
        // the signature in arrow notation.)
        [Test]
        public void MapSetTest()
        {
            /*
             * ISet<T> -> (T -> T2) -> ISet<T2>
             */

            var testSet = new HashSet<int> { 1, 3, 5, 7 };
            var mappedSet = testSet.Map(x => x + 1);
            Assert.AreEqual(new HashSet<int>() { 2, 4, 6, 8 }, mappedSet);
        }

        [Test]
        public void MapDictionaryTest()
        {
            /*
             * IDictionary<TKey, TValue> -> (KeyValuePair<TKey,TValue> -> KeyValuePair<T2Key,T2Value>) -> IDictionary<T2Key, T2Value> 
             */

            var testDictionary = new Dictionary<int, int> { { 2, 2 }, { 4, 4 }, { 6, 6 } };
            var mappedDictionary = testDictionary.Map(x => new KeyValuePair<int, int>(x.Key, x.Value * 2));
            Assert.AreEqual(new Dictionary<int, int> { { 2, 4 }, { 4, 8 }, { 6, 12 } }, mappedDictionary);
        }

        // 2 Implement Map for Option and IEnumerable in terms of Bind and Return.
        [Test]
        public void MapOnOptionWithValueTest()
        {
            Option<int> option = 5;
            var result = option.Map(x => x * 2).Match(() => 0, x => x);
            Assert.AreEqual(10, result);
        }

        [Test]
        public void MapOnOptionWithoutValueTest()
        {
            Option<int> option = F.None;
            var result = option.Map(x => x * 2).Match(() => 0, x => x);
            Assert.AreEqual(0, result);
        }

        [Test]
        public void BindOnOptionWithValueTest()
        {
            Option<int> option = 6;
            var monad = option.Bind(x => x % 2 == 0 ? F.Some(x) : F.None);
            var result = monad.Match(() => 0, x => x);
            Assert.AreEqual(6, result);
        }

        [Test]
        public void BindOnOptionWithoutValueTest()
        {
            Option<int> option = F.None;
            var monad = option.Bind(x => x % 2 == 0 ? F.Some(x) : F.None);
            var result = monad.Match(() => 0, x => x);
            Assert.AreEqual(0, result);
        }

        // 3 Use Bind and an Option-returning Lookup function (such as the one we defined
        // in chapter 3) to implement GetWorkPermit, shown below. 

        static Option<WorkPermit> GetWorkPermit(Dictionary<string, Employee> people, string employeeId)
        {
            return people.Lookup(x => x == employeeId)
                .Bind(x => x.WorkPermit);
        }

        [Test]
        public void TestGetWorkPermit()
        {
            var validWorkPermit = new WorkPermit()
            {
                Expiry = DateTime.Now.AddDays(5)
            };

            var dict = new Dictionary<string, Employee>
            {
                ["Manuel"] = new Employee
                {
                    Id = "Manuel",
                    WorkPermit = F.Some(validWorkPermit)
                }
            };

            var workPermit = GetWorkPermit(dict, "Manuel");
            Assert.AreEqual(F.Some(validWorkPermit), workPermit);
        }

        [Test]
        public void TestGetNotExistingWorkPermit()
        {
            var validWorkPermit = new WorkPermit()
            {
                Expiry = DateTime.Now.AddDays(5)
            };

            var dict = new Dictionary<string, Employee>
            {
                ["Manuel"] = new Employee
                {
                    Id = "Manuel",
                    WorkPermit = F.Some(validWorkPermit)
                }
            };

            var workPermit = GetWorkPermit(dict, "Yegor");
            Assert.AreEqual(F.None, workPermit);
        }
        // Then enrich the implementation so that `GetWorkPermit`
        // returns `None` if the work permit has expired.

        static Option<WorkPermit> GetWorkPermit(Dictionary<string, Employee> people, string employeeId, Predicate<WorkPermit> isValidWorkPermit)
        {
            return people.Lookup(x => x == employeeId)
                .Bind(x => x.WorkPermit)
                .Bind(wp => isValidWorkPermit(wp) ? F.Some(wp) : F.None);
        }
        [Test]
        public void TestGetWorkPermitWithValidity()
        {
            var validWorkPermit = new WorkPermit()
            {
                Expiry = DateTime.Today.AddDays(7)
            };

            var dict = new Dictionary<string, Employee>
            {
                ["Manuel"] = new Employee
                {
                    Id = "Manuel",
                    WorkPermit = F.Some(validWorkPermit)
                }
            };

            var workPermit = GetWorkPermit(dict, "Manuel", x => x.Expiry > DateTime.Today);
            Assert.AreEqual(F.Some(validWorkPermit), workPermit);
        }

        // 4 Use Bind to implement AverageYearsWorkedAtTheCompany, shown below (only
        // employees who have left should be included).

        static double AverageYearsWorkedAtTheCompany(List<Employee> employees)
        {
            return employees
                .Bind(x => x.LeftOn.Map(leaveDate => (leaveDate - x.JoinedOn).Days / 365))
                .Average();
        }
    }

    public static class OptionExtension
    {
        internal static Option<T1> Bind<T, T1>(this Option<T> option, Func<T, Option<T1>> f)
        {
            return option.Match(() => F.None, x => f(x));
        }

        internal static Option<T1> Map<T, T1>(this Option<T> option, Func<T, T1> map)
        {
            return option.Match(() => F.None, x => F.Some(map(x)));
        }

        public static Option<Employee> Lookup(this Dictionary<string, Employee> dict, Predicate<string> predicate)
        {
            foreach (var i in dict)
            {
                if (predicate(i.Key)) return F.Some(i.Value);
            }
            return F.None;
        }

    }
}