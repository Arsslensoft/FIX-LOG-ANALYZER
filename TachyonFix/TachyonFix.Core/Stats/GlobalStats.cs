using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TachyonFix.Core.Stats
{
   public class GlobalStats
    {
        public List<KeyValuePair<string, int>> BrokerMessagesDistribution { get; set; } = new List<KeyValuePair<string, int>>();

        public List<KeyValuePair<string, int>> KnownVsUnknownMessages { get; set; } = new List<KeyValuePair<string, int>>();
        public List<KeyValuePair<string, int>> MessageTypeDistribution { get; set; } = new List<KeyValuePair<string, int>>();
    }
}
