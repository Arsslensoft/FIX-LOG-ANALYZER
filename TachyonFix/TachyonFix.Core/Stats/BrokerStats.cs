using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TachyonFix.Core.Stats
{
    public class BrokerStats
    {
        public AdminAppDistribution AdminAppDistribution { get; set; }
        public ErrorDistribution ErrorDistribution { get; set; }
        public ARDistribution AcceptRejectDistribution { get; set; }
        public List<KeyValuePair<string, int>> MessageTypeDistribution { get; set; }
        public double MessagesPercentage { get; set; }
        public TimeSpan TotalActiveTime { get; set; }
        
        public Broker Broker { get; set; }
    }
}
