using CongestionTaxCalculator.Core;
using static CongestionTaxCalculator.Core.Calculator;

namespace CongestionTaxCalculatorTest
{
    [TestClass]
    public class RateBuilderTests
    {
        [TestMethod]
        public void Integrity()
        {
            var rates = RatesBuilder.StartWith(offHoursFee: 0)
                .ThenFrom(06, 00, fee: 8)
                .Until(18, 00);

            // Start: 00:00:00 Fee: 0
            // Start: 06:00:00 Fee: 8
            // Start: 18:00:00 Fee: 0

            Assert.AreEqual(rates.Length, 3);
            Assert.AreEqual(rates[0].Start, new TimeSpan(00, 00, 00));
            Assert.AreEqual(rates[0].Fee, 0);
            Assert.AreEqual(rates[1].Start, new TimeSpan(06, 00, 00));
            Assert.AreEqual(rates[1].Fee, 8);
            Assert.AreEqual(rates[2].Start, new TimeSpan(18, 00, 00));
            Assert.AreEqual(rates[2].Fee, 0);
        }

        [TestMethod]
        public void FaultyRateConfiguration()
        {
            var buildFaultyRates = () => RatesBuilder.StartWith(offHoursFee: 0)
                .ThenFrom(08, 00, fee: 13)
                .ThenFrom(07, 00, fee: 21)
                .Until(18, 30);

            Assert.ThrowsException<RatesNotInChronologicalOrderException>(buildFaultyRates);
        }
    }
}
