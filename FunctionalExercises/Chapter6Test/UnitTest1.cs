using LaYumba.Functional;
using NUnit.Framework;
using System;

namespace Tests
{
    public static class Chapter6Extensions
    {
        // 1. Write a `ToOption` extension method to convert an `Either` into an
        // `Option`. Then write a `ToEither` method to convert an `Option` into an
        // `Either`, with a suitable parameter that can be invoked to obtain the
        // appropriate `Left` value, if the `Option` is `None`. (Tip: start by writing
        // the function signatures in arrow notation)

        public static Option<T2> ToOption<T1, T2>(this Either<T1, T2> either)
        {
            return either.Match(left => F.None, right => F.Some<T2>(right));
        }

        public static Either<L, R> ToEither<L, R>(this Option<R> option, Func<L> leftOperation)
        {
            return option.Match<Either<L, R>>(() => leftOperation(), right => right);
        }


    }

    public class Tests
    {
        // 2. Take a workflow where 2 or more functions that return an `Option`
        // are chained using `Bind`.

        // Then change the first one of the functions to return an `Either`.

        // This should cause compilation to fail. Since `Either` can be
        // converted into an `Option` as we have done in the previous exercise,
        // write extension overloads for `Bind`, so that
        // functions returning `Either` and `Option` can be chained with `Bind`,
        // yielding an `Option`.

        [Test]
        public void Test2()
        {
            var dogYears = ToHumanYears(5)
                .Bind(ToCatYears)
                .Bind(ToDogYearsFromCatYears)
                .Match(() => 0, years => years);

            Assert.AreEqual(36, dogYears);
        }

        private Option<int> ToHumanYears(int years) => years >= 0 && years < 120 ? F.Some(years) : F.None;

        private Option<int> ToCatYears(int i)
            => i <= 0 ? F.None : F.Some(i * 6);

        private Option<int> ToDogYearsFromCatYears(int i)
        => i <= 0 ? F.None : F.Some((int)(i * 1.2));

        // Then change the first one of the functions to return an `Either`.

        private Either<string, int> ToHumanYearsWithError(int years) => ToHumanYears(years).ToEither(() => "Non realistic amount of years");

        // This should cause compilation to fail. Since `Either` can be
        // converted into an `Option` as we have done in the previous exercise,
        // write extension overloads for `Bind`, so that
        // functions returning `Either` and `Option` can be chained with `Bind`,
        // yielding an `Option`.

        [Test]
        public void ToHumanYearsWithError_GoodCase()
        {
            var test = ToHumanYearsWithError(5)
                .Match(
                    x => "Error: " + x, 
                    x => $"In Human years that is {x} years");

            Assert.AreEqual("In Human years that is 5 years", test);
        }

        [Test]
        public void ToHumanYearsWithError_BadCase()
        {
            var test = ToHumanYearsWithError(333)
                .Match(
                    x => "Error: " + x, 
                    x => $"In Human years that is {x} years");

            Assert.AreEqual("Error: Non realistic amount of years", test);
        }

        [Test]
        public void Test3_GoodCase()
        {
            var dogYears = ToHumanYearsWithError(5)
                .MyBind(ToCatYears)
                .Bind(ToDogYearsFromCatYears)
                .Match(() => 0, years => years);

            Assert.AreEqual(36, dogYears);
        }

      
        [Test]
        public void Test3_BadCase()
        {
            var dogYears = ToHumanYearsWithError(122)
                .MyBind(ToCatYears)
                .Bind(ToDogYearsFromCatYears)
                .Match(() => 0, years => years);

            Assert.AreEqual(0, dogYears);
        }

        // 3. Write a function `Safely` of type ((() → R), (Exception → L)) → Either<L, R> that will
        // run the given function in a `try/catch`, returning an appropriately
        // populated `Either`.

        private int IsBetween0And5(int number)
        {
            if (number < 0 || number > 5) throw new Exception($"The number {number} is not between 0 and 5!");
            return number;
        }

        private Either<Exception, int> ExecuteSafely(Func<int, int> f, int i)
        {
            try
            {
                return F.Right(f(i));
            }
            catch (Exception e)
            {
                return F.Left(e);
            }
        }

        [Test]
        public void Test4_GoodCase()
        {
            string test = ExecuteSafely(IsBetween0And5, 3)
                .Match(
                    x => $"Error: {x.Message}" , 
                    x => $"Correct Number: {x}");

            Assert.AreEqual("Correct Number: 3", test);
        }

        [Test]
        public void Test4_BadCase()
        {
            string test = ExecuteSafely(IsBetween0And5, 13)
                .Match(
                    x => x.Message , 
                    x => "Correct Number: {x}");

            Assert.AreEqual("The number 13 is not between 0 and 5!", test);
        }

        // 4. Write a function `Try` of type (() → T) → Exceptional<T> that will
        // run the given function in a `try/catch`, returning an appropriately
        // populated `Exceptional`.

        private Exceptional<T> TrySafely<T>(Func<T> todo)
        {
            try
            {
                return todo();
            }
            catch (Exception e)
            {
                return e;
            }
        }

        [Test]
        public void Test5_BadCase()
        {
            var array = new[] {1, 2, 3, 4};
            var test = TrySafely(() => array[9])
                .Match(
                    x => "Could not get value", 
                    x => $"The value in the array is {x}");

            Assert.AreEqual("Could not get value", test);
        }

        [Test]
        public void Test5_GoodCase()
        {
            var array = new[] {1, 2, 3, 4};
            var test = TrySafely(() => array[1])
                .Match(
                    x => "Could not get value", 
                    x => $"The value in the array is {x}");

            Assert.AreEqual("The value in the array is 2", test);
        }
    }

    internal static class Chapter6Extension
    {
        public static Option<T1> MyBind<T, T1>(this Either<T, T1> either, Func<T1, Option<T1>> f)
        {
            return either.Match(x => F.None, f);
        }
    }
}