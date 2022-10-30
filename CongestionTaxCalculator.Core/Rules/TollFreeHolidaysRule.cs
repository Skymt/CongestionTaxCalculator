namespace CongestionTaxCalculator.Core.Rules
{
    public class TollFreeHolidaysRule : IRule
    {
        public interface IHolidayChecker
        {
            public bool IsHoliday(DateTime date);
        }
        public static IHolidayChecker? HolidayChecker { get; set; }
        bool _includeDayBefore;
        public TollFreeHolidaysRule(bool includeDayBefore) => _includeDayBefore = includeDayBefore;
        public (TimeSpan passage, int fee)[] Apply(string vehicleType, DateTime date, (TimeSpan passage, int fee)[] passages)
        {
            if (HolidayChecker is null)
                throw new NoHolidayCheckerException();

            if (HolidayChecker.IsHoliday(date))
                return Array.Empty<(TimeSpan, int)>();

            if (_includeDayBefore && HolidayChecker.IsHoliday(date.AddDays(-1)))
                return Array.Empty<(TimeSpan, int)>();

            return passages;
        }
        public class NoHolidayCheckerException : Exception
        {
            public NoHolidayCheckerException()
                : base("You must assign a holiday checker to TollFreeHolidaysRule.HolidayChecker before using this rule.")
            { }
        }
    }
}
