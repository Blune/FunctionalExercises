using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chapter1.Test
{
    [TestClass]
    public class UnitTest1
    {
        // 1. Write a function that negates a given predicate: whenvever the given predicate
        // evaluates to `true`, the resulting function evaluates to `false`, and vice versa.
        [TestMethod]
        public void NegateTest()
        {
            Predicate<int> isEven = (x) => x % 2 == 0;
            var isNotEven = Negate(isEven);
            Assert.IsTrue(isNotEven(5));
        }

        private Predicate<T> Negate<T>(Predicate<T> predicate) => x => !predicate(x);

        // 2. Write a method that uses quicksort to sort a `List<int>` 
        // (return a new list, rather than sorting it in place).

        [TestMethod]
        public void QuickSortTest()
        {
            var unsortedList = new List<int>() { 89, 45, 789, 4, 7896, 34, 39 };
            var sorted = QuickSort(unsortedList);
            Assert.IsTrue(sorted.SequenceEqual(unsortedList.OrderBy(x => x)));
        }

        public List<int> QuickSort(IEnumerable<int> unsortedList)
        {
            if (!unsortedList.Any()) return new List<int>();

            var pivot = unsortedList.First();
            var rest = unsortedList.Skip(1);

            var smaller = rest.Where(x => x <= pivot);
            var bigger = rest.Where(x => x > pivot);

            var result = QuickSort(smaller)
               .Concat(new List<int>() { pivot })
               .Concat(QuickSort(bigger))
               .ToList();

            return result;
        }

        // 3. Generalize your implementation to take a `List<T>`, and additionally a 
        // `Comparison<T>` delegate.

        [TestMethod]
        public void GeneralQuickSortTest()
        {
            var unsortedList = new List<int>() { 89, 45, 789, 4, 7896, 34, 39 };
            var sorted = GeneralQuickSort(unsortedList, new Comparison<int>((x, y) => y.CompareTo(x)));
            Assert.IsTrue(sorted.SequenceEqual(unsortedList.OrderBy(x => x)), $"{string.Join(",", sorted)} vs. {string.Join(",", unsortedList.OrderBy(x => x))}");
        }

        public List<T> GeneralQuickSort<T>(IEnumerable<T> unsortedList, Comparison<T> comparison)
        {
            if (!unsortedList.Any()) return new List<T>();

            var pivot = unsortedList.First();
            var rest = unsortedList.Skip(1);

            var smaller = rest.Where(x => comparison(pivot, x) <= 0);
            var bigger = rest.Where(x => comparison(pivot, x) > 0);

            var result = GeneralQuickSort(smaller, comparison)
               .Concat(new List<T>() { pivot })
               .Concat(GeneralQuickSort(bigger, comparison))
               .ToList();

            return result;
        }


        // 4. In this chapter, you've seen a `Using` function that takes an `IDisposable`
        // and a function of type `Func<TDisp, R>`. Write an overload of `Using` that
        // takes a `Func<IDisposable>` as first
        // parameter, instead of the `IDisposable`. (This can be used to fix warnings
        // given by some code analysis tools about instantiating an `IDisposable` and
        // not disposing it.)

        [TestMethod]
        public void UsingTest()
        {
            var result = Using(() => new DummyDisposable(), isDisposable);
            Assert.IsTrue(result);
        }

        public bool isDisposable(DummyDisposable d) => d is IDisposable;
        public class DummyDisposable : IDisposable
        {
            public void Dispose() { }
        }

        public static R Using<TDisp, R>(Func<TDisp> createDisposable, Func<TDisp, R> func) where TDisp : IDisposable
        {
            using (TDisp disposable = createDisposable())
            {
                return func(disposable);
            };
        }
    }
}
