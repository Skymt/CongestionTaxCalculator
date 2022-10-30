namespace CongestionTaxCalculator.Core.Rules
{
    public class SingleChargeRule : IRule
    {
        public (TimeSpan passage, int fee)[] Apply(string vehicleType, DateTime date, (TimeSpan passage, int fee)[] passages)
        {
            var singleChargeGroups = new List<List<(TimeSpan passage, int fee)>>();
            var lastCheckedTime = TimeSpan.FromHours(-1);
            foreach (var passage in passages)
            {
                if (passage.passage - lastCheckedTime > TimeSpan.FromHours(1))
                {
                    singleChargeGroups.Add(new List<(TimeSpan passage, int fee)> { passage });
                    lastCheckedTime = passage.passage;
                }
                else singleChargeGroups.Last().Add(passage);
            }

            var maxChargeWithinEachGroup = singleChargeGroups.Select(group => group.OrderByDescending(p => p.fee).First());
            return maxChargeWithinEachGroup.ToArray();
        }
    }
}
