using NUnit.Framework;
using Chapter2;
using System;

namespace Tests
{
    public class BmiTests
    {
        //Pure tests
        [TestCase("1,80", ExpectedResult =1.80 )]
        [TestCase("52,3", ExpectedResult = 52.3)]
        public double ParseStringToDouble(string number)
            => number.ToDouble();
        
        [TestCase(1.60, 77, ExpectedResult = 30.08)]
        [TestCase(1.80, 77, ExpectedResult = 23.77)]
        public double CalculateBMI(double height, double weight)
            => Bmi.Calculate(height, weight);

        [TestCase(1.60, 77, ExpectedResult = "overweight")]
        [TestCase(1.80, 77, ExpectedResult = "healthy weight")]
        [TestCase(1.80, 55, ExpectedResult = "underweight")]
        public string CalculateBMICategory(double height, double weight)
            => Bmi.GetBmiCategory(height, weight);

        //Impure tests
        [Test]
        public void CalculateBmiFromConsole()
        {
            string result = "";
            Action<string> writeResult = x => { result = x; };
            Func<string, double> readInput = x => x.Contains("weight") ? 81 : 1.80;
            Program.CalculateBmi(readInput, writeResult);
            Assert.True(!string.IsNullOrEmpty(result)); //It is just interesting if the string is filled. The calculation results are tested above.
        }

    }
}