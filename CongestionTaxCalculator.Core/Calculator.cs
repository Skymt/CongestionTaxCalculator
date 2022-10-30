using CongestionTaxCalculator.Core.Rules;

namespace CongestionTaxCalculator.Core
{
    
    public sealed class Calculator
    {
        readonly Rate[] _rates;
        readonly IRule[]? _rules;
        public Calculator(Rate[] rates) => _rates = VerifyRates(rates);
        public Calculator(Rate[] rates, IRule[]? rules) : this(rates) => _rules = rules;

        public Passage[] GetTaxDetails(string vehicleType, DateTime[] passes) => GetTaxDetails(vehicleType, passes, _rules ?? Array.Empty<IRule>());
        public Passage[] GetTaxDetails(string vehicleType, DateTime[] passes, IRule[] rules)
        {
            (var date, var times) = VerifyPasses(passes);
            var tolls = GetFees(times);

            foreach (var rule in rules)
            {
                tolls = rule.Apply(vehicleType, date, tolls);
            }
            return tolls;
        }

        public int GetTax(string vehicleType, DateTime[] passes) => GetTax(vehicleType, passes, _rules ?? Array.Empty<IRule>());
        public int GetTax(string vehicleType, DateTime[] passes, params IRule[] rules) => GetTaxDetails(vehicleType, passes, rules).Sum(p => p.Fee);
        
        public Passage[] GetFees(TimeSpan[] passes)
        {
            int feeAtTime(TimeSpan time) => _rates.Where(r => r.Start < time).Last().Fee;
            return passes.Select(p => new Passage(p, feeAtTime(p))).ToArray();
        }

        public static (DateTime date, TimeSpan[] times) VerifyPasses(DateTime[] passes)
        {
            var group = passes.GroupBy(p => p.Date);
            if (group.Count() != 1) throw new PassesMayNotSpanSeveralDaysException();

            var date = group.First().Key;
            var times = group.First().Select(p => p.TimeOfDay).OrderBy(p => p.Ticks).ToArray();

            return (date, times);
        }
        
        public static Rate[] VerifyRates(IEnumerable<Rate> rates)
        {
            var previousRate = rates.First();

            if (previousRate.Start != new TimeSpan())
                throw new FirstRateStartTimeNotFromMidnightException();

            foreach (var currentRate in rates.Skip(1))
            {
                if (previousRate.Start > currentRate.Start)
                    throw new RatesNotInChronologicalOrderException();
                previousRate = currentRate;
            }
            return rates.ToArray();
        }
        public sealed class FirstRateStartTimeNotFromMidnightException : Exception
        {
            public FirstRateStartTimeNotFromMidnightException() : base("First rate must start from midnight (00:00)") { }
        }
        public sealed class RatesNotInChronologicalOrderException : Exception
        {
            public RatesNotInChronologicalOrderException() : base("Order of rates is invalid") { }
        }
        public sealed class PassesMayNotSpanSeveralDaysException : Exception
        {
            public PassesMayNotSpanSeveralDaysException() : base("This calculator only handles passages during a single day") { }
        }
    }
}
