using System.IO;
using AnujBank;
using Moq;
using NUnit.Framework;

namespace TestAnujBank
{
    [TestFixture]
    public class TestInterestRates
    {
        [Test]
        public void ShouldLoadInterestRates()
        {
            StringReader reader = new StringReader("PositiveInterestRate=2.0\nNegativeInterestRate=3.0");
            var configurationManager = new InterestRates(reader);
            Assert.AreEqual(0.0d, configurationManager.PositiveInterestRate());
            Assert.AreEqual(0.0d, configurationManager.NegativeInterestRate());
            configurationManager.Configure();
            Assert.AreEqual(2.0d, configurationManager.PositiveInterestRate());
            Assert.AreEqual(3.0d, configurationManager.NegativeInterestRate());
        }
    }
}