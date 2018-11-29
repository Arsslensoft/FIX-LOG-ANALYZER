using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickFix;
using QuickFix.DataDictionary;

namespace TachyonFix.Core.Query
{
    public abstract class BaseQuery : IQuery
    {
        public abstract Predicate<Entry> Predicate { get; protected set; }
        public abstract void ApplyFilter(DDField field, string condition, string value);
        public abstract IEnumerable<Entry> Query(IEnumerable<Entry> source);

        protected string GetValueFromEntry(int tag, Entry m)
        {
            if (m.Message != null) return GetValueFromMessage(tag, m.Message);
            else if (m.Error != null && m.Error.ParsedMessage != null) return GetValueFromMessage(tag, m.Error.ParsedMessage);
            return null;
        }
        protected string GetValueFromMessage(int tag, Message m)
        {
            if (m.IsSetField(tag))
                return m.GetField(tag);
            else if (m.Header.IsSetField(tag))
                return m.Header.GetField(tag);
            else if (m.Trailer.IsSetField(tag))
                return m.Trailer.GetField(tag);

            return null;
        }

    }
}
