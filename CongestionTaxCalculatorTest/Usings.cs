global using Microsoft.VisualStudio.TestTools.UnitTesting;
global using static Globals;
using CongestionTaxCalculator.Core.Rules;

static class Globals
{
    public static readonly string TaxedVehicle = "Car";
    public static readonly string TaxExcemptVehicle = "Motorcycle";

    static Globals() =>
        TollFreeHolidaysRule.HolidayChecker = new Holidays2013();

    class Holidays2013 : TollFreeHolidaysRule.IHolidayChecker
    {
        static readonly DateTime[] _holidays = new DateTime[]
            {
                new (2013, 01, 01),
                new (2013, 03, 28), new (2013, 03, 29),
                new (2013, 05, 01), new (2013, 05, 08), new (2013, 05, 09),
                new (2013, 06, 05), new (2013, 06, 06), new (2013, 06, 21),
                new (2013, 11, 01),
                new (2013, 12, 24), new (2013, 12, 25),
                new (2013, 12, 26), new (2013, 12, 31)
            };
        public bool IsHoliday(DateTime date) => _holidays.Contains(date);
    }
}