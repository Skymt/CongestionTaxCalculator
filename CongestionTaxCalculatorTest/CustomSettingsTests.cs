namespace CustomSettingsTests
{
    [TestClass]
    public class SingleChargeRule
    {

        [TestMethod]
        public void SettingsApplySingleChargeRule()
        {
            var withRule = new CongestionTaxCalculator2
            {
                Settings = new() { ApplySingleChargeRule = true, MaxDailyFee = 10000 },

            };
            var withoutRule = new CongestionTaxCalculator2
            {
                Settings = new() { ApplySingleChargeRule = false, MaxDailyFee = 10000 },

            };

            var passages = TollPassagesForDate(DateTime.Parse("2013-02-08")).ToArray();
            var taxSingleCharge = withRule.GetTax(TaxedVehicle, passages);
            var taxNoSingleCharge = withoutRule.GetTax(TaxedVehicle, passages);

            Assert.AreNotEqual(taxSingleCharge, taxNoSingleCharge);
            Assert.AreEqual(taxSingleCharge, 70);
            Assert.AreEqual(taxNoSingleCharge, 117);
        }
    }
}
