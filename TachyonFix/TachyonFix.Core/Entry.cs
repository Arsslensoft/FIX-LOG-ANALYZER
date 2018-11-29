using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickFix;

namespace TachyonFix.Core
{
    public enum Direction
    {
        IN,
        OUT
    }
    public enum EntryType
    {
        Data,
        Fix,
        Unknown
    }
    public abstract class BaseEntry
    {
        public virtual EntryType Kind { get;  set; } = EntryType.Unknown;
        public DateTimeOffset DateTime { get; set; }
        public int Index { get; set; }
        public string Content { get; set; }

        public Direction Direction { get; set; }
        public string RawLogLine { get; set; }
    }
    public class Entry : BaseEntry
    {
        public Message BaseMessage { get; set; }

        public Message Message { get; set; }    

        public Error Error { get; set; }
    }
}
