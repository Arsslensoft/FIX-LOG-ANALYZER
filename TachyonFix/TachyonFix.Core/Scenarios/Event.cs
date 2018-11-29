using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickFix;

namespace TachyonFix.Core.Scenarios
{
   public class Event
    {
        public Broker Source { get; set; }
        public Broker Target { get; set; }

        public bool Unknow => Source == null || Target == null;

        public Entry Entry { get; set; }
        public Message Message => Entry?.Message ?? Entry?.Error?.ParsedMessage;
    }
}
