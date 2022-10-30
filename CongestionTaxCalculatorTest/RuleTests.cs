using CongestionTaxCalculator.Core;
using CongestionTaxCalculator.Core.Rules;

namespace CongestionTaxCalculatorTest
{
    [TestClass]
    public class RuleTests
    {
        [TestMethod]
        public void SingleChargeRule()
        {
            var rates = RatesBuilder.StartWith()
                .ThenFrom(06, 00, useFee: 8)
                .ThenFrom(07, 00, useFee: 13)
                .Until(18, 00);

            var calc = new Calculator(rates);
            var pass = new[]
            {
                DateTime.Parse("2013-02-06 06:48:17"),
                DateTime.Parse("2013-02-06 07:13:21"),
                DateTime.Parse("2013-02-06 18:35:06")
            };

            Assert.AreEqual(21, calc.GetTax(TaxedVehicle, pass));

            var rule = new SingleChargeRule();
            Assert.AreEqual(13, calc.GetTax(TaxedVehicle, pass, rule));
        }

        [TestMethod]
        public void TollFreeHolidayRule()
        {
            var rates = RatesBuilder.StartWith()
                .ThenFrom(06, 00, useFee: 8)
                .ThenFrom(07, 00, useFee: 13)
                .Until(18, 00);

            var calc = new Calculator(rates);
            var pass = new[]
            {
                DateTime.Parse("2013-12-24 06:48:17"),
                DateTime.Parse("2013-12-24 07:13:21"),
                DateTime.Parse("2013-12-24 18:35:06")
            };

            Assert.AreEqual(21, calc.GetTax(TaxedVehicle, pass));

            var rule = new TollFreeHolidaysRule(DateTime.Parse("2013-12-24"));
            Assert.AreEqual(0, calc.GetTax(TaxedVehicle, pass, rule));
        }

        [TestMethod]
        public void TollFreeMonthOfJulyRule()
        {
            var rates = RatesBuilder.StartWith()
                .ThenFrom(06, 00, useFee: 8)
                .ThenFrom(07, 00, useFee: 13)
                .Until(18, 00);

            var calc = new Calculator(rates);
            var pass = new[]
            {
                DateTime.Parse("2013-07-12 06:48:17"),
                DateTime.Parse("2013-07-12 07:13:21"),
                DateTime.Parse("2013-07-12 18:35:06")
            };

            Assert.AreEqual(21, calc.GetTax(TaxedVehicle, pass));

            var rule = new TollFreeMonthOfJulyRule();
            Assert.AreEqual(0, calc.GetTax(TaxedVehicle, pass, rule));
        }

        [TestMethod]
        public void TollFreeVehiclesRule()
        {
            var rates = RatesBuilder.StartWith()
                .ThenFrom(06, 00, useFee: 8)
                .ThenFrom(07, 00, useFee: 13)
                .Until(18, 00);

            var calc = new Calculator(rates);
            var pass = new[]
            {
                DateTime.Parse("2013-07-12 06:48:17"),
                DateTime.Parse("2013-07-12 07:13:21"),
                DateTime.Parse("2013-07-12 18:35:06")
            };

            var rule = new TollFreeVehiclesRule(TaxExcemptVehicle);

            Assert.AreEqual(21, calc.GetTax(TaxedVehicle, pass));
            Assert.AreEqual(21, calc.GetTax(TaxExcemptVehicle, pass));

            Assert.AreEqual(21, calc.GetTax(TaxedVehicle, pass, rule));
            Assert.AreEqual(0, calc.GetTax(TaxExcemptVehicle, pass, rule));
        }
    }
}
