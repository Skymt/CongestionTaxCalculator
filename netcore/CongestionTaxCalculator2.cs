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
                throw new Exception("This function only calculates tax for a single day.");
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

        public class TaxSettings
        {
            public string[] TollFreeVehicles { get; set; }
            public DayOfWeek[] TollFreeDaysOfWeek { get; set; }
            public int[] TollFreeMonths { get; set; }
            public DateTime[] Holidays { get; set; }
            public RateDefinition[] Rates { get; set; }
            public bool ApplySingleChargeRule { get; set; }
            public int MaxDailyFee { get; set; }
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
