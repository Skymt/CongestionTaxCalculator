namespace CongestionTaxCalculator.Core.Rules
{
    public class MaxChargeRule : IRule
    {
        int _maxCharge;
        public MaxChargeRule(int maxCharge) => _maxCharge = maxCharge;
        public (TimeSpan passage, int fee)[] Apply(string vehicleType, DateTime date, (TimeSpan passage, int fee)[] passages)
        {
            if (passages.Sum(p => p.fee) < _maxCharge)
                return passages;

            List<(TimeSpan passage, int fee)> adjusted = new List<(TimeSpan passage, int fee)>();
            var total = 0;
            foreach (var passage in passages)
            {
                if (total + passage.fee >= _maxCharge)
                {
                    var newFee = _maxCharge - total;
                    adjusted.Add((passage.passage, newFee));
                }
                else
                {
                    total += passage.fee;
                    adjusted.Add(passage);
                }
            }
            return adjusted.ToArray();
        }
    }
}
