namespace CongestionTaxCalculator.Core.Rules
{
    public sealed class TollFreeMonthOfJulyRule : IRule
    {
        public Passage[] Apply(string vehicleType, DateTime date, Passage[] passages)
        {
            if (date.Month == 7) 
                return passages.Select(p => new Passage(p.Time, 0, Math.Max(p.Fee, p.Discount))).ToArray();
            return passages;
        }
    }
}
