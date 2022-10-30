namespace CongestionTaxCalculator.Core.Rules
{
    public sealed class SingleChargeRule : IRule
    {
        public Passage[] Apply(string vehicleType, DateTime date, Passage[] passages)
        {
            var singleChargeGroups = new List<List<Passage>>();
            var lastCheckedTime = TimeSpan.FromHours(-1);
            foreach (var passage in passages)
            {
                if (passage.Time - lastCheckedTime > TimeSpan.FromHours(1))
                {
                    singleChargeGroups.Add(new List<Passage> { passage });
                    lastCheckedTime = passage.Time;
                }
                else singleChargeGroups.Last().Add(passage);
            }
            List<Passage> adjustedPassages = new(passages.Length);
            foreach (var singleChargeGroup in singleChargeGroups)
            {
                var orderedGroup = singleChargeGroup.OrderByDescending(p => p.Fee);
                adjustedPassages.Add(orderedGroup.First());
                foreach(var passage in orderedGroup.Skip(1))
                    adjustedPassages.Add(new(passage.Time, 0, passage.Fee));
            }
            return adjustedPassages.OrderBy(p => p.Time).ToArray();
        }    
    }
}
