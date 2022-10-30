using CongestionTaxCalculator.Core.Rules;

namespace CongestionTaxCalculator.Core.Tenants
{
    internal class Stockholm
    {
        public static readonly Rate[] Rates =
            RatesBuilder.StartWith(offHoursFee: 0)
                .ThenFrom(06, 00, useFee: 18)
                .ThenFrom(06, 30, useFee: 23)
                .ThenFrom(07, 00, useFee: 28)
                .ThenFrom(08, 00, useFee: 23)
                .ThenFrom(08, 30, useFee: 18)
                .ThenFrom(15, 00, useFee: 23)
                .ThenFrom(15, 30, useFee: 28)
                .ThenFrom(17, 00, useFee: 23)
                .ThenFrom(18, 00, useFee: 18)
                .Until(18, 30);

        public static readonly IRule[] Rules = new IRule[]
        {
            new SingleChargeRule(),
            new TollFreeVehiclesRule(new[] { "Tractor", "Emergency", "Diplomat", "Foreign", "Military" }),
            new TollFreeHolidaysRule(includeDayBefore: false),
            new MaxChargeRule(200)
        };
    }
}
