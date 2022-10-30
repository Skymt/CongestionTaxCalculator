namespace CongestionTaxCalculator.Core
{
    public record Rate(TimeSpan Start, int Fee);
    public sealed class RatesBuilder
    {
        RatesBuilder() { }
        readonly List<Rate> rates = new();
        int OffHoursFee => rates.First().Fee;
        public static RatesBuilder StartWith(int offHoursFee = 0)
        {
            var builder = new RatesBuilder();
            builder.rates.Add(new(new(00, 00, 00), offHoursFee));
            return builder;
        }

        public RatesBuilder ThenFrom(int hour, int minute, int useFee)
        {
            rates.Add(new(new(hour, minute, 0), useFee));
            return this;
        }

        public Rate[] Until(int hour, int minute)
        {
            rates.Add(new(new(hour, minute, 0), OffHoursFee));
            return Calculator.VerifyRates(rates);
        }
    }
}