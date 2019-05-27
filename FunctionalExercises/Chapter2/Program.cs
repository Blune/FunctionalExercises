using System;

namespace Chapter2
{
    public class Program
    {
        // 1. Write a console app that calculates a user's Body-Mass Index:
        //   - prompt the user for her height in metres and weight in kg
        //   - calculate the BMI as weight/height^2
        //   - output a message: underweight(bmi<18.5), overweight(bmi>=25) or healthy weight
        // 2. Structure your code so that structure it so that pure and impure parts are separate
        // 3. Unit test the pure parts
        // 4. Unit test the impure parts using the HOF-based approach

        static void Main()
        {
            CalculateBmi(ConsoleRead, ConsoleWrite);
            Console.ReadLine();
        }

        //Impure function because it relies on impure functions. 
        public static void CalculateBmi(Func<string, double> read, Action<string> write)
        {
            var height = read("Please write your height in meters");
            var weight = read("Please write your weight in kg");

            var bmi = Bmi.GetBmiCategory(height, weight);
            write($"Your BMI category is {bmi}");
        }

        //Impure because it will return different values depending on user input
        public static double ConsoleRead(string message)
        {
            Console.WriteLine(message);
            return Console.ReadLine().ToDouble();
        }

        //Impure because it will return different values depending on user input
        public static void ConsoleWrite(string message)
        {
            Console.WriteLine(message);
        }
    }

    public static class StringExtension
    {
        //Pure function. Always returns the same value for same input. No side effects
        public static double ToDouble(this string s) => double.Parse(s);
    }

    public static class Bmi
    {
        public static string GetBmiCategory(double height, double weight) => Calculate(height, weight).Categorize();

        //Pure function. Always returns the same value for same input. No side effects
        public static double Calculate(double height, double weight)
            => Math.Round(weight / Math.Pow(height, 2), 2);

        //Pure function. Always returns the same value for same input. No side effects
        public static string Categorize(this double bmi)
        {
            if (bmi < 18.5) return "underweight";
            if (bmi < 25) return "healthy weight";
            return "overweight";
        }
    }
}
