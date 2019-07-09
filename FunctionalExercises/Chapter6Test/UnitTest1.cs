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

        public static Option<T2> ToOption<T1,T2>(this Either<T1,T2> either)
        {
            return either.Match(left => F.None, right => F.Some<T2>(right));
        }

        public static Either<L,R> ToEither<L,R>(this Option<R> option, Func<L> leftOperation)
        {
            return option.Match<Either<L,R>>(() =>leftOperation(), right => right);
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
            Option<int> humanYears = 5;
            var dogYears = humanYears
                .Bind(InCatYears)
                .Bind(InDogYearsFromCatYears)
                .Match(() => 0, years => years);

            Assert.AreEqual(36, dogYears);
        }

        private Option<int> InCatYears(int i) 
            => i <= 0 ? F.None : F.Some(i * 6);
       
        private Option<int> InDogYearsFromCatYears(int i)
        =>  i <= 0 ? F.None : F.Some((int)(i * 1.2));    
    }
}