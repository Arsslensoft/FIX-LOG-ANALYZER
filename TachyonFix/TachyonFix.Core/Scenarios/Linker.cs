using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using QuickFix;
using QuickFix.Fields;

namespace TachyonFix.Core.Scenarios
{
   public class Linker
    {
        public List<Event> Events { get; set; }
        public Solution Solution { get; set; }

        public int UnknownCount { get; set; }
        public Linker (Solution sol)
        {
            Solution = sol;
            Events = new List<Event>();
        }

        public event Action<double, string> OnProgressChanged;
        public void DetectEvents()
        {
            if (Events.Count > 0)
                return;
            OnProgressChanged?.Invoke(0, "Detecting events...");
            double count = Solution.Entries.Count;
            double progress = 0;
            int done = 0;
            UnknownCount = 0;
            foreach (var e in Solution.Entries)
            {
                var message = e.Message ?? e.Error.ParsedMessage;
                if(message == null) throw new NullReferenceException("Message is not defined");

                var s_name = Solution.TryGetSenderNameFromMessage(message);
                var s_id = Solution.TryGetSenderIdFromMessage(message);
                var t_name = Solution.TryGetTargetNameFromMessage(message);
                var t_id = Solution.TryGetTargetIdFromMessage(message);

                var source = Solution.TryGetBrokerByNameOrId(s_name, s_id);
                var target = Solution.TryGetBrokerByNameOrId(t_name, t_id);
                if (source == null || target == null)
                {
                    UnknownCount++;
                    Solution.LinkingErrors.Add(new Error(){ Message = source == null ? "Unknown source id and name" : "Unknown target id and name", Content = e.Content, DateTime = e.DateTime, Direction = e.Direction, ErrorType = ErrorType.Syntax, Kind = e.Kind, ParsedMessage = message, Index = e.Index, RawLogLine = e.RawLogLine, Exception = new NullReferenceException("Unknown target or source")});
                }
      
                Events.Add(new Event(){ Entry = e,Source = source,Target = target});
                
                done++;
                progress = (done / count) * 100;
                OnProgressChanged?.Invoke(progress, "Linking events...");
            }

 

        }
    }
}
