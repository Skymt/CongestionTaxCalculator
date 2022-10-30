namespace CongestionTaxCalculator.Core.Rules
{
    public class TollFreeVehiclesRule : IRule
    {
        readonly string[] _taxExcemptVehicleTypes;
        public TollFreeVehiclesRule(params string[] taxExcemptVehicleTypes) => _taxExcemptVehicleTypes = taxExcemptVehicleTypes;
        public (TimeSpan passage, int fee)[] Apply(string vehicleType, DateTime date, (TimeSpan passage, int fee)[] passages)
        {
            if (_taxExcemptVehicleTypes.Contains(vehicleType)) return Array.Empty<(TimeSpan, int)>();
            return passages;
        }
    }
}
