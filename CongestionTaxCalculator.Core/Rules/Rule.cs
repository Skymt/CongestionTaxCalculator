namespace CongestionTaxCalculator.Core.Rules
{
    public interface IRule
    {
        public (TimeSpan passage, int fee)[] Apply(string vehicleType, DateTime date, (TimeSpan passage, int fee)[] passages);
    }
}
