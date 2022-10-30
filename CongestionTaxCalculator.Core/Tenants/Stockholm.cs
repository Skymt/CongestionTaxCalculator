using CongestionTaxCalculator.Core.Rules;

namespace CongestionTaxCalculator.Core.Tenants
{
    internal class Stockholm
    {
        public static readonly Rate[] Rates =
            RatesBuilder.StartWith(offHoursFee: 0)
                .ThenFrom(06, 00, fee: 18)
                .ThenFrom(06, 30, fee: 23)
                .ThenFrom(07, 00, fee: 28)
                .ThenFrom(08, 00, fee: 23)
                .ThenFrom(08, 30, fee: 18)
                .ThenFrom(15, 00, fee: 23)
                .ThenFrom(15, 30, fee: 28)
                .ThenFrom(17, 00, fee: 23)
                .ThenFrom(18, 00, fee: 18)
                .Until(18, 30);

        public static readonly IRule[] Rules = new IRule[]
        {
            new SingleChargeRule(),
            new TollFreeSaturdaySundayRule(),
            new TollFreeVehiclesRule(new[] { "Motorcycle", "Bus", "Emergency", "Diplomat", "Foreign", "Military" }),
            new TollFreeHolidaysRule(),
            new TollFreeDayBeforeHolidaysRule(),
            new MaxChargeRule(maxCharge: 100)
        };
    }
}
