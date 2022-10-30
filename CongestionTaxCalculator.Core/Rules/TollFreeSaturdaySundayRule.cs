namespace CongestionTaxCalculator.Core.Rules
{
    public class TollFreeSaturdaySundayRule : IRule
    {
        public (TimeSpan passage, int fee)[] Apply(string vehicleType, DateTime date, (TimeSpan passage, int fee)[] passages)
        {
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                return Array.Empty<(TimeSpan, int)>();
            return passages;
        }
    }
}
