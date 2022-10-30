namespace CongestionTaxCalculator.Core.Rules
{
    public class TollFreeHolidaysRule : IRule
    {
        readonly DateTime[] _holidays;
        public TollFreeHolidaysRule(params DateTime[] holidays) => _holidays = holidays;
        public (TimeSpan passage, int fee)[] Apply(string vehicleType, DateTime date, (TimeSpan passage, int fee)[] passages)
        {
            if (_holidays.Contains(date)) return Array.Empty<(TimeSpan, int)>();
            return passages;
        }
    }
}
