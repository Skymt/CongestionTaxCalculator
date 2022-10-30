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
            new TollFreeHolidaysRule(includeDayBefore: true),
            new MaxChargeRule(60)
        };
    }
}
