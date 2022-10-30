namespace CongestionTaxCalculator.Core.Rules
{
    public sealed class MaxChargeRule : IRule
    {
        int _maxCharge;
        public MaxChargeRule(int maxCharge) => _maxCharge = maxCharge;
        public Passage[] Apply(string vehicleType, DateTime date, Passage[] passages)
        {
            List<Passage> adjustedPassages = new(passages.Length);
            var total = 0;
            foreach(var passage in passages)
            {
                if(total + passage.Fee < _maxCharge)
                {
                    adjustedPassages.Add(passage);
                    total += passage.Fee;
                }
                else
                {
                    var newFee = _maxCharge - total;
                    adjustedPassages.Add(new(passage.Time, newFee, passage.Fee - newFee));
                    total += newFee;
                }
            }
            return adjustedPassages.ToArray();
        }
    }
}
