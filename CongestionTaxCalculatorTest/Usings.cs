global using Microsoft.VisualStudio.TestTools.UnitTesting;
global using congestion.calculator;
global using static GlobalTestData;

static class GlobalTestData
{
    public static readonly DateTime[] TollPassages = new[]
    {
        DateTime.Parse("2013-01-14 21:00:00"), // Out of hours
        DateTime.Parse("2013-02-06 06:48:17"), // Toll: 13      T
        DateTime.Parse("2013-02-06 07:13:21"), // Toll: 18      | Single charge winner
        DateTime.Parse("2013-02-06 18:35:06"), // Out of hours
        DateTime.Parse("2013-02-07 06:23:27"), // Toll: 8
        DateTime.Parse("2013-02-07 15:27:00"), // Toll: 13
        DateTime.Parse("2013-02-08 06:20:27"), // Toll: 8       T
        DateTime.Parse("2013-02-08 06:27:00"), // Toll: 8       |
        DateTime.Parse("2013-02-08 14:35:00"), // Toll: 8       T
        DateTime.Parse("2013-02-08 15:29:00"), // Toll: 13      |
        DateTime.Parse("2013-02-08 15:47:00"), // Toll: 18      T
        DateTime.Parse("2013-02-08 16:01:00"), // Toll: 18      |
        DateTime.Parse("2013-02-08 16:48:00"), // Toll: 18      T
        DateTime.Parse("2013-02-08 17:49:00"), // Toll: 13      T
        DateTime.Parse("2013-02-08 18:29:00"), // Toll: 8       T
        DateTime.Parse("2013-02-08 18:35:00"), // Out of hours  |
        DateTime.Parse("2013-03-26 14:25:00"), // Toll: 8
        DateTime.Parse("2013-03-28 14:07:27"), // Toll: 8
        DateTime.Parse("2013-12-24 06:23:27"), // Holiday
        DateTime.Parse("2013-12-24 15:27:00"), // Holiday
    };
    public static readonly IEnumerable<DateTime> TollPassageDates = TollPassages.Select(d => d.Date).Distinct();
    public static IEnumerable<DateTime> TollPassagesForDate(DateTime date) => TollPassages.Where(d => d.Date == date);
    public static readonly CongestionTaxCalculator2 Calculator = new() { Settings = new() };
    public static readonly string TaxedVehicle = "Car";
    public static readonly string TaxExcemptVehicle = "Motorcycle";
}