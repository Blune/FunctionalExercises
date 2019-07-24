using System;
using LaYumba.Functional;
using NUnit.Framework;

namespace Tests
{
    public static class Chapter7Extensions
    {
        public static Func<T1, R> ApplyR<T1, T2, R>(this Func<T1, T2, R> f, T2 t)
        {
            return x => f(x, t);
        }

        public static Func<T1, T2, R> ApplyR<T1, T2, T3, R>(this Func<T1, T2, T3, R> f, T3 t3)
        {
            return (x1, x2) => f(x1, x2, t3);
        }
    }

    public class Tests
    {
        // 1. Partial application with a binary arithmethic function:
        // Write a function `Remainder`, that calculates the remainder of 
        // integer division(and works for negative input values!). 

        public Func<int, int, int> Remainder = (number, divisor) => number % divisor;

        // Notice how the expected order of parameters is not the
        // one that is most likely to be required by partial application
        // (you are more likely to partially apply the divisor).


        // Write an `ApplyR` function, that gives the rightmost parameter to
        // a given binary function (try to write it without looking at the implementation for `Apply`).
        // Write the signature of `ApplyR` in arrow notation, both in curried and non-curried form

        /*
         * Normal:  ((T1,T2) -> T3) ,T2) -> T1 -> T3) 
         * Curried: ((T1 -> T2 -> T3) -> T2) -> T1 -> T3)
         */

        // Use `ApplyR` to create a function that returns the
        // remainder of dividing any number by 5. 

        [Test]
        public void TestApplyR()
        {
            var remainderOfDivisionBy5 = Remainder.ApplyR(5);
            Assert.AreEqual(4, remainderOfDivisionBy5(24));
        }

        // Write an overload of `ApplyR` that gives the rightmost argument to a ternary function
        [Test]
        public void TestApplyRWithTernaryFunction()
        {
            Func<int, int, int, int> AddAllAndMultiply = (a, b, c) => (a + b) * c;
            var multiplyWith5 = AddAllAndMultiply.ApplyR(5);
            Assert.AreEqual(25, multiplyWith5(2, 3));
        }

        // 2. Let's move on to ternary functions. Define a class `PhoneNumber` with 3
        // fields: number type(home, mobile, ...), country code('it', 'uk', ...), and number.
        // `CountryCode` should be a custom type with implicit conversion to and from string.

        enum NumberType { Home, Mobile, Office }

        class PhoneNumber
        {
            public PhoneNumber(CountryCode countryCode, NumberType numberType, string number)
            {
                NumberType = numberType;
                CountryCode = countryCode;
                Number = number;
            }

            public NumberType NumberType { get; }
            public CountryCode CountryCode { get; }
            public string Number { get; }
        }

        class CountryCode
        {
            private string Value { get; }
            public CountryCode(string s) { Value = s; }
            public static implicit operator string(CountryCode c) => c.Value;
            public static implicit operator CountryCode(string s) => new CountryCode(s);
            public override string ToString() => Value;
        }

        // Now define a ternary function that creates a new number, given values for these fields.
        // What's the signature of your factory function? 
        /*
        * CountryCode -> NumberType -> string -> PhoneNumber
        */

        Func<CountryCode, NumberType, string, PhoneNumber> CreatePhoneNumber
            = (countryCode, numberType, number) => new PhoneNumber(countryCode, numberType, number);


        // Use partial application to create a binary function that creates a UK number, 
        // and then again to create a unary function that creates a UK mobile
        [Test]
        public void MakeTernaryCreatePhoneNumberFunctionToBinary()
        {
            Func<NumberType, string, PhoneNumber> CreateUkNumber = CreatePhoneNumber.Apply(new CountryCode("UK-Number"));
            Assert.IsNotNull(CreateUkNumber(NumberType.Home, "123456789"));
        }


        // 3. Functions everywhere. You may still have a feeling that objects are ultimately 
        // more powerful than functions. Surely, a logger object should expose methods 
        // for related operations such as Debug, Info, Error? 
        // To see that this is not necessarily so, challenge yourself to write 
        // a very simple logging mechanism without defining any classes or structs. 
        // You should still be able to inject a Log value into a consumer class/function, 
        // exposing operations like Debug, Info, and Error, like so:

        //static void ConsumeLog(Log log) 
        //   => log.Info("look! no objects!");

        static void ConsumeLog(LoggerExtension.Log log) => log.Info("look! no objects!");
    }

    public static class LoggerExtension
    {
        public enum Level { Debug, Info, Error }

        public delegate void Log(Level level, string message);
        public static Log Logger = (Level level, string message) => Console.WriteLine($"{level}: {message}");
        public static void Debug(this Log log, string message) => log(Level.Debug, message);
        public static void Info(this Log log, string message) => log(Level.Info, message);
        public static void Error(this Log log, string message) => log(Level.Error, message);
    }
}