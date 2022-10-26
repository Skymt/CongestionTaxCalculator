using congestion.calculator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace CongestionTaxCalculator
{
    public static class Function1
    {
        [FunctionName("GetTax")]
        public static IActionResult GetTax1(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req)
        {
            var calculator = new congestion.calculator.CongestionTaxCalculator();
            var vehicle = VehicleFactory.GetVehicle(req.Query["vehicle"]);
            var dates = req.Query["dates"].Select(v => DateTime.Parse(v)).ToArray();

            return new OkObjectResult(new { tax = calculator.GetTax(vehicle, dates) });
        }

        static readonly JsonSerializerOptions jsonSerializerOptions = new(JsonSerializerDefaults.Web);
        [FunctionName("GetTax2")]
        public static async Task<IActionResult> GetTax2(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req)
        {
            var settings = defaultTaxSettings;
            if (req.Body.CanRead)
            {
                try
                {
                    settings = await JsonSerializer.DeserializeAsync<CongestionTaxCalculator2.TaxSettings>(req.Body, jsonSerializerOptions);
                }
                catch
                {
                    settings = defaultTaxSettings;
                }
            }

            var calculator = new CongestionTaxCalculator2() { Settings = settings };
            var vehicle = req.Query["vehicle"];
            var dates = req.Query["dates"].Select(DateTime.Parse).ToArray();
            var tax = calculator.GetTax(vehicle, dates);
            return new OkObjectResult(new { tax, settings });
        }

        static readonly CongestionTaxCalculator2.TaxSettings defaultTaxSettings = new()
        {
            ApplySingleChargeRule = true,
            MaxDailyFee = 60,
            TollFreeDaysOfWeek = new[] { DayOfWeek.Saturday, DayOfWeek.Sunday },
            TollFreeMonths = new[] { 7 },
            Holidays = new DateTime[]
            {
                new DateTime(2013, 1, 1),
                new DateTime(2013, 3, 28), new DateTime(2013, 3, 29),
                new DateTime(2013, 5, 1), new DateTime(2013, 5, 8), new DateTime(2013, 5, 9),
                new DateTime(2013, 6, 5), new DateTime(2013, 6, 6), new DateTime(2013, 6, 21),
                new DateTime(2013, 11, 1),
                new DateTime(2013, 12, 24), new DateTime(2013, 12, 25),
                new DateTime(2013, 12, 26), new DateTime(2013, 12, 31)
            },
            TollFreeVehicles = new[]
            {
                "Motorcycle",
                "Tractor",
                "Emergency",
                "Diplomat",
                "Foreign",
                "Military"
            },
            Rates = new CongestionTaxCalculator2.RateDefinition[]
            {
                new(new TimeSpan(00, 00, 0), 0),
                new(new TimeSpan(06, 00, 0), 8),
                new(new TimeSpan(06, 30, 0), 13),
                new(new TimeSpan(07, 00, 0), 18),
                new(new TimeSpan(08, 00, 0), 13),
                new(new TimeSpan(08, 30, 0), 8),
                new(new TimeSpan(15, 00, 0), 13),
                new(new TimeSpan(15, 30, 0), 18),
                new(new TimeSpan(17, 00, 0), 13),
                new(new TimeSpan(18, 30, 0), 0)
            }
        };
    }
}
