namespace CongestionTaxCalculator.Core.Rules
{
    public interface IRule
    {
        Passage[] Apply(string vehicleType, DateTime date, Passage[] passages);
    }
}
