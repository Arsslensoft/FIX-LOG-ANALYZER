using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickFix;
using TachyonFix.Core.Insights;
using TachyonFix.Core.Stats;

namespace TachyonFix.Core
{
   public class Broker
    {
        public string Name { get; set; }
        public List<string> IdList { get; set; } = new List<string>();
        public IEnumerable<Error> Errors => Entries.Where(e => e.Error != null).Select(x => x.Error);
        public bool Unknown { get; set; }
        public bool Trader { get; set; }
        public List<Entry> Entries { get; set; } = new List<Entry>();
        public BrokerStatisticsCalculator Stats { get; set; }
        public InsightCalculator Insights { get; set; }
        public DateTimeOffset? Start => Entries?.FirstOrDefault()?.DateTime;
        public DateTimeOffset? End => Entries?.LastOrDefault()?.DateTime;
        public TimeSpan? RecordedTime => End - Start;
        public IEnumerable<Message> AllMessages => Entries.Select(x => x.Message ?? x.Error.ParsedMessage);

    }
}
