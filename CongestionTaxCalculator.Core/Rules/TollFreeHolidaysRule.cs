namespace CongestionTaxCalculator.Core.Rules
{
    public class TollFreeHolidaysRule : IRule
    {
        public interface IHolidayChecker
        {
            public bool IsHoliday(DateTime date);
        }
        public static IHolidayChecker? HolidayChecker { get; set; }
        public virtual Passage[] Apply(string vehicleType, DateTime date, Passage[] passages)
        {
            if (HolidayChecker is null)
                throw new NoHolidayCheckerException();

            if (HolidayChecker.IsHoliday(date))
                return passages.Select(p => new Passage(p.Time, 0, Math.Max(p.Fee, p.Discount))).ToArray();

            return passages;
        }
        public class NoHolidayCheckerException : Exception
        {
            public NoHolidayCheckerException()
                : base("You must assign a holiday checker to TollFreeHolidaysRule.HolidayChecker before using this rule.")
            { }
        }
    }
    public sealed class TollFreeDayBeforeHolidaysRule : TollFreeHolidaysRule
    {
        public override Passage[] Apply(string vehicleType, DateTime date, Passage[] passages)
        {
            if (HolidayChecker is null)
                throw new NoHolidayCheckerException();

            if (HolidayChecker.IsHoliday(date.AddDays(1)))
                return passages.Select(p => new Passage(p.Time, 0, Math.Max(p.Fee, p.Discount))).ToArray();

            return passages;
        }
    }
}
