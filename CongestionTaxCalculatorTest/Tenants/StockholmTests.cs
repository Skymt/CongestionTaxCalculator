using CongestionTaxCalculator.Core;
using CongestionTaxCalculator.Core.Rules;
using Core = CongestionTaxCalculator.Core.Tenants.Tenants;

namespace CongestionTaxCalculatorTest.Tenants
{
    [TestClass]
    public class StockholmTests
    {
        static (Rate[] rates, IRule[] rules) Settings
        {
            get
            {
                if (Core.TryGetTenant("Stockholm", out var settings))
                    return settings;
                throw new Exception("Missing configuration");
            }
        }

        [TestMethod]
        public void EnsureTenant()
        {
            var settingsExist = Core.TryGetTenant("Stockholm", out var settings);
            Assert.AreEqual(true, settingsExist);

            var calculator = new Calculator(settings.rates);
            Assert.IsNotNull(calculator);
        }


        [TestMethod]
        public void Rates()
        {
            var calculator = new Calculator(Settings.rates);
            var passages = new TimeSpan[]
            {
                new(06,00,30), new(06,30,30), new(07,00,30), new(08,00,30), new(08,30,30),
                new(15,00,30), new(15,30,30), new(17,00,30), new(18,00,30), new(18,30,30)
            };
            var expectedFees = new[] { 18, 23, 28, 23, 18, 23, 28, 23, 18, 0 };

            var expectedResults = passages.Zip(expectedFees).ToDictionary(t => t.First, t => t.Second);
            var actualResults = calculator.GetTolls(passages);

            foreach (var result in actualResults)
                Assert.AreEqual(expectedResults[result.passage], result.fee);
        }

        [TestMethod]
        public void TaxExcemptVehicle()
        {
            (var rates, var rules) = Settings;
            var calculator = new Calculator(rates);
            var passages = new DateTime[]
            {
                new(2013, 01, 24, 06, 00, 30)
            };
            var tollFee = calculator.GetTax(TaxedVehicle, passages, rules);
            Assert.AreEqual(18, tollFee);

            tollFee = calculator.GetTax(Globals.TaxExcemptVehicle, passages, rules);
            Assert.AreEqual(0, tollFee);
        }

        [TestMethod]
        public void TaxExcemptHoliday()
        {
            (var rates, var rules) = Settings;
            var calculator = new Calculator(rates);
            var passages = new DateTime[]
            {
                new(2013, 12, 24, 06, 00, 30)
            };

            var tollFee = calculator.GetTax(TaxedVehicle, passages, rules);
            Assert.AreEqual(0, tollFee);
        }

        [TestMethod]
        public void TaxExcemptDayBeforeHoliday()
        {
            (var rates, var rules) = Settings;
            var calculator = new Calculator(rates);
            var passages = new DateTime[]
            {
                new(2013, 12, 23, 06, 00, 30)
            };

            var tollFee = calculator.GetTax(TaxedVehicle, passages, rules);
            Assert.AreEqual(0, tollFee);
        }

        [TestMethod]
        public void TaxExcemptJuly()
        {
            (var rates, var rules) = Settings;
            var calculator = new Calculator(rates);
            var passages = new DateTime[]
            {
                new(2013, 07, 24, 06, 00, 30)
            };

            var tollFee = calculator.GetTax(TaxedVehicle, passages, rules);
            Assert.AreEqual(0, tollFee);
        }

        [TestMethod]
        public void SingleCharge()
        {
            (var rates, var rules) = Settings;
            var calculator = new Calculator(rates);
            var passages = new DateTime[]
            {
                new(2013, 01, 24, 06, 15, 30),
                new(2013, 01, 24, 07, 00, 30),
                new(2013, 01, 24, 18, 10, 30)
            };

            var tollFee = calculator.GetTax(TaxedVehicle, passages, rules);
            Assert.AreEqual(46, tollFee);

            var tollFeeNoRules = calculator.GetTax(TaxedVehicle, passages);
            Assert.AreNotEqual(tollFee, tollFeeNoRules);
        }

        [TestMethod]
        public void MaxFee()
        {
            (var rates, var rules) = Settings;
            var calculator = new Calculator(rates);
            var passages = new DateTime[]
            {
                new(2013, 01, 24, 06, 15, 30),
                new(2013, 01, 24, 06, 31, 30),
                new(2013, 01, 24, 06, 48, 30),
                new(2013, 01, 24, 07, 25, 30),
                new(2013, 01, 24, 08, 15, 30),
                new(2013, 01, 24, 15, 15, 30),
                new(2013, 01, 24, 15, 32, 30),
                new(2013, 01, 24, 16, 05, 30),
                new(2013, 01, 24, 16, 36, 30)
            };
            var tollFee = calculator.GetTax(TaxedVehicle, passages, rules);
            Assert.AreEqual(100, tollFee);

            var noRulesTollFee = calculator.GetTax(TaxedVehicle, passages);
            Assert.AreNotEqual(tollFee, noRulesTollFee);
        }
    }
}
