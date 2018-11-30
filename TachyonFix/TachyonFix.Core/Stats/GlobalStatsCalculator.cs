using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickFix;

namespace TachyonFix.Core.Stats
{
  public  class GlobalStatsCalculator
    {
        public Solution Solution { get; set; }
        public GlobalStats Stats { get; set; }
        public GlobalStatsCalculator(Solution s )
        {
            Solution = s;
        }
        public event Action<double, string> OnProgressChanged;

        public void CalculateStats()
        {
            if (Stats != null) return;
            Stats = new GlobalStats();
            OnProgressChanged?.Invoke(25, "Calculating global stats...");

            Stats.BrokerMessagesDistribution.AddRange(Solution.Intermediaries.Select(x => new KeyValuePair<string, int>(x.Value.Name, x.Value.Entries.Count)));
  
      
            Stats.KnownVsUnknownMessages.Add(new KeyValuePair<string, int>("Fix", Solution.MessagesCount));
            Stats.KnownVsUnknownMessages.Add(new KeyValuePair<string, int>("Other", Solution.OtherEntries.Count));

            OnProgressChanged?.Invoke(50, "Calculating message distribution...");
            // MTD
            Stats.MessageTypeDistribution = (from p in Solution.Entries where p.Message != null
                group p by p.Message.GetType().Name into g
                select new KeyValuePair<string, int>(g.Key, g.Count())).ToList();
        }
    }
}
