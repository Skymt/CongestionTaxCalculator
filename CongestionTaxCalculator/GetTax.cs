using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq;
using CongestionTaxCalculator.Core.Tenants;
using CongestionTaxCalculator.Core;

namespace CongestionTaxCalculator
{
    public static class GetTax
    {
        [FunctionName("GetTax")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "GetTax/{vehicleType}")]
            [FromBody]string passages, [FromRoute]string vehicleType, ILogger log)
        {
            var dates = passages.Split(',').Select(DateTime.Parse);
            if(Tenants.TryGetTenant("Gothenburg", out var settings))
            {
                var calculator = new Calculator(settings.rates);
                var tax = calculator.GetTax(vehicleType, dates.ToArray(), settings.rules);
                return new OkObjectResult(new { tax });
            }

            return new OkObjectResult(new { Ok = false });
        }
    }
}
