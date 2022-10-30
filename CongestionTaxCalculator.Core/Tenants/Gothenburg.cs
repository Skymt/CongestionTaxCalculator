using CongestionTaxCalculator.Core.Rules;

namespace CongestionTaxCalculator.Core.Tenants
{
    internal class Gothenburg
    {
        public static readonly Rate[] Rates = 
            RatesBuilder.StartWith(offHoursFee: 0)
                .ThenFrom(06, 00, useFee: 8)
                .ThenFrom(06, 30, useFee: 13)
                .ThenFrom(07, 00, useFee: 18)
                .ThenFrom(08, 00, useFee: 13)
                .ThenFrom(08, 30, useFee: 8)
                .ThenFrom(15, 00, useFee: 13)
                .ThenFrom(15, 30, useFee: 18)
                .ThenFrom(17, 00, useFee: 13)
                .ThenFrom(18, 00, useFee: 8)
                .Until(18, 30);

        public static readonly IRule[] Rules = new IRule[]
        {
            new SingleChargeRule(),
            new TollFreeMonthOfJulyRule(),
            new TollFreeVehiclesRule(new[] { "Motorcycle", "Tractor", "Emergency", "Diplomat", "Foreign", "Military" }),
            new TollFreeHolidaysRule(new DateTime[]
            {
                new (2013, 01, 01),
                new (2013, 03, 28), new (2013, 03, 29),
                new (2013, 05, 01), new (2013, 05, 08), new (2013, 05, 09),
                new (2013, 06, 05), new (2013, 06, 06), new (2013, 06, 21),
                new (2013, 11, 01),
                new (2013, 12, 24), new (2013, 12, 25),
                new (2013, 12, 26), new (2013, 12, 31)
            }),
            new MaxChargeRule(60)
        };
    }
}
