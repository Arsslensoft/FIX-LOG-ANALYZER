using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickFix.FIX42;

namespace TachyonFix.Core.Insights
{
   public class InsightCalculator
    {
        public static InsightsDatabase InsightsDatabase { get; set; }
        public Solution Solution { get; set; }
        public Broker Broker { get; set; }
        public List<string> Insights { get; set; }

        public InsightCalculator(Solution s, Broker b)
        {
            Solution = s;
            Broker = b;
        }

        public event Action<double, string> OnProgressChanged;

        public void RecommendInsights()
        {
            if (Insights != null && Insights.Count > 0)
                return;
            Insights = new List<string>();
            // get all rejects
            var allIncomingMessages = Solution.Linker.Events.Where(x => x.Target == Broker).Select(x => x.Message);
            foreach (var m in allIncomingMessages)
            {
                if (InsightsDatabase.Insights.Any(x => x.MessageTag == m.Header.GetString(35)))
                {
                    var insights = InsightsDatabase.Insights.FirstOrDefault(x => x.MessageTag == m.Header.GetString(35));
                    if(insights.Cases.Any(x => m.IsSetField(x.ErrorFieldTag) && m.GetString(x.ErrorFieldTag) == x.ErrorFieldValue))
                        Insights.Add(insights.Cases.FirstOrDefault(x => m.IsSetField(x.ErrorFieldTag) && m.GetString(x.ErrorFieldTag) == x.ErrorFieldValue).Recommendation);
                }
            }
            Insights = Insights.Distinct().ToList();
        }
    }
}
