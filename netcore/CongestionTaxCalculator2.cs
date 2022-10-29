using System;
using System.Collections.Generic;
using System.Linq;

namespace congestion.calculator
{
    public class CongestionTaxCalculator2
    {
        public TaxSettings Settings;

        /// <summary>
        /// Calculate the total toll fee for one day
        /// </summary>
        /// <param name="vehicle">The type of vehicle</param>
        /// <param name="dates">The times the vehicle passed the border</param>
        /// <returns>The toll total</returns>
        public int GetTax(string vehicle, DateTime[] dates)
        {
            var totalTax = 0;

            //Check if vehicle is tax excempt
            if (Settings.TollFreeVehicles.Contains(vehicle))
                return totalTax;

            //Verify that this is for a single day
            var dateGroup = dates.GroupBy(d => d.Date);
            if (dateGroup.Count() != 1)
                throw new DateOverflowException();
            var times = dateGroup.First();

            //Check if day is tax excempt
            if (IsTollFreeDay(times.Key))
                return totalTax;

            if (Settings.ApplySingleChargeRule)
            {
                //Group all times that occur within one hour of each-other
                var allTimes = times.Select(d => d.TimeOfDay).OrderBy(d => d);
                var timeGroups = new List<List<TimeSpan>>();
                var previousTime = TimeSpan.FromDays(-1);
                foreach (var time in allTimes)
                {
                    if (time - previousTime > TimeSpan.FromHours(1))
                    {
                        timeGroups.Add(new List<TimeSpan>());
                        timeGroups.Last().Add(time);
                        previousTime = time;
                    }
                    else
                    {
                        timeGroups.Last().Add(time);
                    }
                }

                //Add up the the highest rate that applies to each group
                foreach (var timeGroup in timeGroups)
                {
                    var rates = timeGroup.Select(RateAtTime);
                    totalTax += rates.Max();
                }
            }
            else
            {
                totalTax = times.Select(d => d.TimeOfDay).Sum(RateAtTime);
            }

            //Don't overcharge
            return Math.Min(totalTax, Settings.MaxDailyFee);
        }

        bool IsTollFreeDay(DateTime date) => Settings.TollFreeDaysOfWeek.Contains(date.DayOfWeek)
            || Settings.TollFreeMonths.Contains(date.Month)
            || Settings.Holidays.Contains(date)
            || Settings.Holidays.Contains(date.AddDays(-1));

        int RateAtTime(TimeSpan time) => Settings.Rates.TakeWhile(r => r.Start < time).Last().Fee;

        public class DateOverflowException : Exception
        {
            public DateOverflowException() : base("This function only calculates tax for a single day.") { }
        }

        public class TaxSettings
        {
            public string[] TollFreeVehicles { get; set; } = new[]
            {
                "Motorcycle",
                "Tractor",
                "Emergency",
                "Diplomat",
                "Foreign",
                "Military"
            };
            public DayOfWeek[] TollFreeDaysOfWeek { get; set; } = new[] { DayOfWeek.Saturday, DayOfWeek.Sunday };
            public int[] TollFreeMonths { get; set; } = new[] { 7 };
            public DateTime[] Holidays { get; set; } = new[]
            {
                new DateTime(2013, 1, 1),
                new DateTime(2013, 3, 28), new DateTime(2013, 3, 29),
                new DateTime(2013, 5, 1), new DateTime(2013, 5, 8), new DateTime(2013, 5, 9),
                new DateTime(2013, 6, 5), new DateTime(2013, 6, 6), new DateTime(2013, 6, 21),
                new DateTime(2013, 11, 1),
                new DateTime(2013, 12, 24), new DateTime(2013, 12, 25),
                new DateTime(2013, 12, 26), new DateTime(2013, 12, 31)
            };
            public RateDefinition[] Rates { get; set; } = new RateDefinition[]
            {
                new RateDefinition(new TimeSpan(00, 00, 0), 0),
                new RateDefinition(new TimeSpan(06, 00, 0), 8),
                new RateDefinition(new TimeSpan(06, 30, 0), 13),
                new RateDefinition(new TimeSpan(07, 00, 0), 18),
                new RateDefinition(new TimeSpan(08, 00, 0), 13),
                new RateDefinition(new TimeSpan(08, 30, 0), 8),
                new RateDefinition(new TimeSpan(15, 00, 0), 13),
                new RateDefinition(new TimeSpan(15, 30, 0), 18),
                new RateDefinition(new TimeSpan(17, 00, 0), 13),
                new RateDefinition(new TimeSpan(18, 30, 0), 0)
            };
            public bool ApplySingleChargeRule { get; set; } = true;
            public int MaxDailyFee { get; set; } = 60;
        }
        public struct RateDefinition { 
            public TimeSpan Start { get; set; } 
            public int Fee { get; set; }
            public RateDefinition(TimeSpan start, int fee)
            {
                Start = start;
                Fee = fee;
            }
        }
    }
}
