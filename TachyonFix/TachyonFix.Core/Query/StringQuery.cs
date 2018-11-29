using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickFix;
using QuickFix.DataDictionary;

namespace TachyonFix.Core.Query
{
   public class StringQuery : BaseQuery
    {
        public override Predicate<Entry> Predicate { get; protected set; }

        public override void ApplyFilter(DDField field, string condition, string value)
        {
            if(condition == "=")
                Predicate = e => GetValueFromEntry(field.Tag, e) == value;
            else if (condition == "STARTS_WITH")
                Predicate = e => GetValueFromEntry(field.Tag, e).StartsWith(value);
            else if (condition == "CONTAINS")
                Predicate = e => GetValueFromEntry(field.Tag, e).Contains(value);
            else if (condition == "ENDS_WITH")
                Predicate = e => GetValueFromEntry(field.Tag, e).EndsWith(value);
            else 
                throw new NotSupportedException($"{condition} operator is not supported for {this.GetType().Name}");
        }

        public override IEnumerable<Entry> Query(IEnumerable<Entry> source)
        {
            return source.Where(x => Predicate(x));
        }

      
    }
}
