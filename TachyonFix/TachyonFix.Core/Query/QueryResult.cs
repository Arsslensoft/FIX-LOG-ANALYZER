using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TachyonFix.Core.Query
{
  
   public class QueryResult
    {
        public IQuery Query { get; set; }
        public string Operator { get; set; }

        public QueryResult Next { get; set; }
        public IEnumerable<Entry> Execute(IEnumerable<Entry> source)
        {
            var src = source;
            var original = new List<Entry>(src);
            if (Query != null)
                src = Query.Query(src);
            if (Next != null)
            {
                if (Operator == "AND")
                    return Next.Execute(src);
                else if (Operator == "NOT")
                    return src.Except(Next.Execute(original));
                else if (Operator == "OR")
                    return Next.Execute(original).Union(src).Distinct();
                else if (Next.Query is EmptyQuery && Query is EmptyQuery)
                        return Next.Execute(original).Union(src).Distinct();
                else return Next.Execute(src);
            }
            else return src;

        }
    }
}
