using CongestionTaxCalculator.Core;

namespace CongestionTaxCalculatorTest
{
    [TestClass]
    public class CalculatorTests
    {
        [TestMethod]
        public void Integrity()
        {
            var rates = RatesBuilder.StartWith(offHoursFee: 0)
                .ThenFrom(06, 00, fee: 8)
                .Until(18, 00);

            var calc = new Calculator(rates);
            var pass = new[]
            {
                DateTime.Parse("2013-02-06 06:48:17"),
                DateTime.Parse("2013-02-06 07:13:21"),
                DateTime.Parse("2013-02-06 18:35:06")
            };

            // Two passes in the morning, for 8 SEK each - one pass in evening, out of hours
            Assert.AreEqual(16, calc.GetTax(TaxedVehicle, pass));

            var details = calc.GetTaxDetails(TaxedVehicle, pass);
            // All passes are accounted for
            Assert.AreEqual(pass.Length, details.Length);
            
            // No rules have supplied discounts
            var discount = details.Sum(p => p.Discount);
            Assert.AreEqual(0, discount);
            
        }

        [TestMethod]
        public void FaultyRateConfiguration()
        {
            var rates = new Rate[]
            {
                new(new TimeSpan(01, 00, 00), 0),
                new(new TimeSpan(06, 00, 00), 8),
                new(new TimeSpan(18, 00, 00), 0)
            };

            Assert.ThrowsException<Calculator.FirstRateStartTimeNotFromMidnightException>(() => new Calculator(rates));
        }

        [TestMethod]
        public void FaultyDateRange()
        {
            var pass = new[]
            {
                DateTime.Parse("2013-02-06 06:48:17"),
                DateTime.Parse("2013-02-07 18:35:06")
            };
            Assert.ThrowsException<Calculator.PassesMayNotSpanSeveralDaysException>(() => Calculator.VerifyPasses(pass));
        }
    }
}
