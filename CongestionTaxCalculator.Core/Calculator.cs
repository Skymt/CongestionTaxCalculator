using CongestionTaxCalculator.Core.Rules;

namespace CongestionTaxCalculator.Core
{
    public class Calculator
    {
        readonly Rate[] _rates;
        public Calculator(Rate[] rates) => _rates = VerifyRates(rates);

        public int GetTax(string vehicleType, DateTime[] passages, params IRule[] rules)
        {
            (var date, var times) = VerifyPassages(passages);
            var tolls = GetTolls(times);

            foreach (var rule in rules)
            {
                tolls = rule.Apply(vehicleType, date, tolls);
                if (!tolls.Any()) return 0;
            }

            return tolls.Sum(p => p.fee);
        }

        public static (DateTime date, TimeSpan[] passages) VerifyPassages(DateTime[] passages)
        {
            var group = passages.GroupBy(p => p.Date);
            if (group.Count() != 1) throw new PassagesMayNotSpanSeveralDaysException();

            var date = group.First().Key;
            var passageTimes = group.First().Select(p => p.TimeOfDay).OrderBy(p => p.Ticks);

            return (date, passageTimes.ToArray());
        }
        public (TimeSpan passage, int fee)[] GetTolls(TimeSpan[] passages)
        {
            (TimeSpan passage, int fee) rateAtTime(TimeSpan time) => (time, _rates.Where(r => r.Start < time).Last().Fee);
            var passageFees = passages.Select(rateAtTime);

            return passageFees.ToArray();
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
        public class FirstRateStartTimeNotFromMidnightException : Exception
        {
            public FirstRateStartTimeNotFromMidnightException() : base("First rate must start from midnight (00:00)") { }
        }
        public class RatesNotInChronologicalOrderException : Exception
        {
            public RatesNotInChronologicalOrderException() : base("Order of rates is invalid") { }
        }
        public class PassagesMayNotSpanSeveralDaysException : Exception
        {
            public PassagesMayNotSpanSeveralDaysException() : base("This calculator only handles passages during a single day") { }
        }
    }
}
