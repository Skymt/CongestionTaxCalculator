using CongestionTaxCalculator.Core.Rules;

namespace CongestionTaxCalculator.Core.Tenants
{
    internal class Gothenburg
    {
        public static readonly Rate[] Rates =
            RatesBuilder.StartWith(fee: 0)
                .ThenFrom(06, 00, fee: 8)
                .ThenFrom(06, 30, fee: 13)
                .ThenFrom(07, 00, fee: 18)
                .ThenFrom(08, 00, fee: 13)
                .ThenFrom(08, 30, fee: 8)
                .ThenFrom(15, 00, fee: 13)
                .ThenFrom(15, 30, fee: 18)
                .ThenFrom(17, 00, fee: 13)
                .ThenFrom(18, 00, fee: 8)
                .Until(18, 30);

        public static readonly IRule[] Rules = new IRule[]
        {
            new SingleChargeRule(),
            new TollFreeMonthOfJulyRule(),
            new TollFreeVehiclesRule(new[] { "Motorcycle", "Tractor", "Emergency", "Diplomat", "Foreign", "Military" }),
            new TollFreeSaturdaySundayRule(),
            new TollFreeHolidaysRule(),
            new TollFreeDayBeforeHolidaysRule(),
            new MaxChargeRule(maxCharge: 60)
        };
    }
}
