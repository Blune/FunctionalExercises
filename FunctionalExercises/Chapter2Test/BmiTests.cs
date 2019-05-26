using NUnit.Framework;
using Chapter2;
using System;

namespace Tests
{
    public class BmiTests
    {
        //Pure tests
        [Test]
        public void ParseStringToDouble()
        {
            Assert.AreEqual(82.3, "82,3".ToDouble());
        }

        [Test]
        public void ParseWeightInputToDouble()
        {
            Assert.AreEqual(82.3, Program.GetHeight(() => "82,3")); 
        }

        [Test]
        public void ParseHeightInputToDouble()
        {
            Assert.AreEqual(1.80, Program.GetHeight(() => "1,80"));
        }

        [Test]
        public void CalculateBMIHealthy()
        {
            Assert.AreEqual(24.69, Math.Round(Bmi.Calculate(1.80, 80), 2));
        }

        [Test]
        public void CalculateBMIUnderweight()
        {
            Assert.AreEqual(17.04, Math.Round(Bmi.Calculate(1.80, 55.2), 2));
        }

        [Test]
        public void CalculateBMIOverweight()
        {
            Assert.AreEqual(28.15, Math.Round(Bmi.Calculate(1.80, 91.2), 2));
        }

        //Impure tests

    }
}