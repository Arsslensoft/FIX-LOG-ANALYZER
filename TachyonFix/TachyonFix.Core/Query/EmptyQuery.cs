using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickFix.DataDictionary;

namespace TachyonFix.Core.Query
{
   public class EmptyQuery : BaseQuery
    {
        public override Predicate<Entry> Predicate { get; protected set; }
        public override void ApplyFilter(DDField field, string condition, string value)
        {
            return;
        }

        public override IEnumerable<Entry> Query(IEnumerable<Entry> source)
        {
            return source;
        }
    }
}
