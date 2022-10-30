using CongestionTaxCalculator.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using System;
using System.Linq;

namespace CongestionTaxCalculator
{
    public class GetTax
    {
        readonly Calculator _calculator;
        public GetTax(Calculator calculator) => _calculator = calculator;

        [FunctionName("GetTax")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "GetTax/{vehicleType}")]
            [FromBody]string passages, [FromRoute] string vehicleType)
        {
            var dates = passages.Split(',').Select(DateTime.Parse);
            var tax = _calculator.GetTax(vehicleType, dates.ToArray());
            return new OkObjectResult(new { tax });
        }
    }
}
