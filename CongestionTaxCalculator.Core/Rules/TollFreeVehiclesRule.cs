namespace CongestionTaxCalculator.Core.Rules
{
    public sealed class TollFreeVehiclesRule : IRule
    {
        readonly string[] _taxExcemptVehicleTypes;
        public TollFreeVehiclesRule(params string[] taxExcemptVehicleTypes) => _taxExcemptVehicleTypes = taxExcemptVehicleTypes;
        public Passage[] Apply(string vehicleType, DateTime date, Passage[] passages)
        {
            if (_taxExcemptVehicleTypes.Contains(vehicleType)) 
                return passages.Select(p => new Passage(p.Time, 0, Math.Max(p.Fee, p.Discount))).ToArray();
            return passages;

        }
    }
}
