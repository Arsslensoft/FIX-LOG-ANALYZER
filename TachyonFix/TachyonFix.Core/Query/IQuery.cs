using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickFix.DataDictionary;

namespace TachyonFix.Core.Query
{
    public interface IQuery
    {
        Predicate<Entry> Predicate { get; }
        void ApplyFilter(DDField field, string condition, string value);
        IEnumerable<Entry> Query(IEnumerable<Entry> source);
    }
}
