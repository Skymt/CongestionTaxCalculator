namespace CongestionTaxCalculator.Core.Rules
{
    public sealed class TollFreeSaturdaySundayRule : IRule
    {
        public Passage[] Apply(string vehicleType, DateTime date, Passage[] passages)
        {
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                return passages.Select(p => new Passage(p.Time, 0, Math.Max(p.Fee, p.Discount))).ToArray();
            return passages;

        }
    }
}
