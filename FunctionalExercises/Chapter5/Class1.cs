using Chapter4;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chapter5
{
    public class Class1
    {
        // 1. Without looking at any code or documentation (or intllisense), write the function signatures of
        // `OrderByDescending`, `Take` and `Average`, which we used to implement `AverageEarningsOfRichestQuartile`:
        static decimal AverageEarningsOfRichestQuartile(List<Person> population)
           => population
              .OrderByDescending(p => p.Earnings)
              .Take(population.Count / 4)
              .Select(p => p.Earnings)
              .Average();

        // OrderByDescending: (IEnumerable<T>, Func(T -> decimal)) -> IEnumerable<T>
        // Take: (IEnumerable<T>, int) -> IEnumerable<T>
        // Average: IEnumerable<decimal> -> decimal

        // 2 Check your answer with the MSDN documentation: https://docs.microsoft.com/
        // en-us/dotnet/api/system.linq.enumerable. How is Average different?

        // Average stops the chaining by not returning an IEnumerable<T>.

        // 3 Implement a general purpose Compose function that takes two unary functions
        // and returns the composition of the two.

        public Func<T1, T3> Compose<T1, T2, T3>(Func<T1, T2> f1, Func<T2, T3> f2)
        {
            return x => f2(f1(x));
        }
    }
}