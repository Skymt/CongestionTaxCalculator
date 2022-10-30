using CongestionTaxCalculator.Core;
using CongestionTaxCalculator.Core.Rules;
using CongestionTaxCalculator.Core.Tenants;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(CongestionTaxCalculator.Startup))]

namespace CongestionTaxCalculator
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // Connect the core holiday rules with the nuget NordicHolidays.NET
            TollFreeHolidaysRule.HolidayChecker = new HolidayProvider();

            // Context is used to determine what rules and rates the tax calculator should use
            builder.Services.AddHttpContextAccessor();

            // The rules and rates are determined by the header "Tenant" in the http request.
            // If header is missing, the rules for "Gothenburg" are used as defaults
            builder.Services.AddScoped(services =>
            {
                var context = services.GetService<IHttpContextAccessor>();
                if (context.HttpContext.Request.Headers.TryGetValue("Tenant", out var tenant))
                {
                    if (Tenants.TryGetTenant(tenant, out var settings))
                        return new Calculator(settings.rates, settings.rules);
                    return null;
                }

                if (Tenants.TryGetTenant("Gothenburg", out var defaults))
                    return new Calculator(defaults.rates, defaults.rules);
                return null;
            });
        }
    }
    public class HolidayProvider : TollFreeHolidaysRule.IHolidayChecker
    {
        public bool IsHoliday(DateTime date) => Holidays.ReturnDates.isHoliday(date, Holidays.ReturnDates.Country.Sweden, false, true);
    }
}