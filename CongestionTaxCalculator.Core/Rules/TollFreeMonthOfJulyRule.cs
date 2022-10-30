namespace CongestionTaxCalculator.Core.Rules
{
    public class TollFreeMonthOfJulyRule : IRule
    {
        public (TimeSpan passage, int fee)[] Apply(string vehicleType, DateTime date, (TimeSpan passage, int fee)[] passages)
        {
            if (date.Month == 7) return Array.Empty<(TimeSpan, int)>();
            return passages;
        }
    }
}
