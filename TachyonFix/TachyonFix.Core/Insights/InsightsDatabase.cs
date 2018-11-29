using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TachyonFix.Core.Insights
{
   public class InsightsDatabase
    {
        public List<MessageInsightEntry> Insights { get; set; } 
    }

    public class MessageInsightEntry
    {
        public string MessageTag { get; set; }
      
        public List<InsightEntry> Cases { get; set; }
    }
    public class InsightEntry
    {
        public int ErrorFieldTag { get; set; }
        public string ErrorFieldValue { get; set; }
        public string Recommendation { get; set; }
    }
}
