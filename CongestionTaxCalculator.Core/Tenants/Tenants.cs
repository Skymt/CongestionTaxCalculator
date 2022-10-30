using CongestionTaxCalculator.Core.Rules;

namespace CongestionTaxCalculator.Core.Tenants
{
    public static class Tenants
    {
        static Dictionary<string, (Rate[] rates, IRule[] rules)> tenantsCache { get; } = new()
        {
            ["Gothenburg"] = (Gothenburg.Rates, Gothenburg.Rules)
        };
        public static bool TryGetTenant(string tenantName, out (Rate[] rates, IRule[] rules) tenantSettings)
        {
            if (tenantsCache.ContainsKey(tenantName))
            {
                tenantSettings = tenantsCache[tenantName];
                return true;
            }
            tenantSettings = default;
            return false;
        }
    }
}
