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
            CongestionTaxCalculator2.TaxSettings settings = new();
            if (HttpMethods.IsPost(req.Method))
            {
                try
                {
                    settings = await JsonSerializer.DeserializeAsync<CongestionTaxCalculator2.TaxSettings>(req.Body, jsonSerializerOptions);
                }
                catch 
                {
                    settings = new();
                }
            }

            var calculator = new CongestionTaxCalculator2() { Settings = settings };
            var vehicle = req.Query["vehicle"];
            var dates = req.Query["dates"].Select(DateTime.Parse).ToArray();
            var tax = calculator.GetTax(vehicle, dates);
            return new OkObjectResult(new { tax, settings });
        }
    }
}
