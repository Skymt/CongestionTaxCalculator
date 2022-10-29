namespace WithDefaultSettings
{
    [TestClass]
    public class ExceptionTests
    {
        [TestMethod]
        public void FailWhenFedSeveralDates()
        {
            Assert.ThrowsException<CongestionTaxCalculator2.DateOverflowException>(() => Calculator.GetTax(TaxedVehicle, TollPassages));
        }
    }
    [TestClass]
    public class NoTaxTests
    {
        [TestMethod]
        public void TestExcemptVehicle()
        {
            foreach (var tollPassageDate in TollPassageDates)
            {
                var passages = TollPassagesForDate(tollPassageDate).ToArray();
                Assert.AreEqual(Calculator.GetTax(TaxExcemptVehicle, passages), 0);
            }
        }

        [TestMethod]
        public void TestOutOfHours()
        {
            var passages = TollPassagesForDate(DateTime.Parse("2013-01-14")).ToArray();
            Assert.AreEqual(Calculator.GetTax(TaxedVehicle, passages), 0);
        }

        [TestMethod]
        public void TestHoliday()
        {
            var passages = TollPassagesForDate(DateTime.Parse("2013-12-24")).ToArray();
            Assert.AreEqual(Calculator.GetTax(TaxedVehicle, passages), 0);
        }
    }
    [TestClass]
    public class TollTests
    {
        [TestMethod]
        public void TestSingleChargeOverflow()
        {
            var passages = TollPassagesForDate(DateTime.Parse("2013-02-06")).ToArray();
            Assert.AreEqual(Calculator.GetTax(TaxedVehicle, passages), 18);
        }

        [TestMethod]
        public void TestToll()
        {
            var passages = TollPassagesForDate(DateTime.Parse("2013-02-07")).ToArray();
            Assert.AreEqual(Calculator.GetTax(TaxedVehicle, passages), 21);
        }

        [TestMethod]
        public void TestTollMaxCharge()
        {
            var passages = TollPassagesForDate(DateTime.Parse("2013-02-08")).ToArray();
            Assert.AreEqual(Calculator.GetTax(TaxedVehicle, passages), 60);
        }
    }
}
