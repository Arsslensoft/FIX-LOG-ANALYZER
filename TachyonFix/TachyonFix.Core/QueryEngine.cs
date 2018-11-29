using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickFix;
using QuickFix.DataDictionary;
using QuickFix.Fields;
using TachyonFix.Core.Query;

namespace TachyonFix.Core
{
   public class QueryEngine
   {
       private List<DDField> _fields;
       private List<IQuery> _queries;
       private QueryResult _result;
        public IEnumerable<Entry> SourceEntries { get; set; }
        public QueryEngine(IEnumerable<Entry> sourceEntries)
        {
            SourceEntries = sourceEntries;
        }

       public string GetValueFromEntry(int tag, Entry m)
       {
           if (m.Message != null) return GetValueFromMessage(tag, m.Message);
           else if (m.Error != null && m.Error.ParsedMessage != null) return GetValueFromMessage(tag, m.Error.ParsedMessage);
           return "";
       }
       protected string GetValueFromMessage(int tag, Message m)
       {
           if (m.IsSetField(tag))
               return m.GetField(tag);
           else if (m.Header.IsSetField(tag))
               return m.Header.GetField(tag);
           else if (m.Trailer.IsSetField(tag))
               return m.Trailer.GetField(tag);

           return "";
       }

        public void ApplyFields(IEnumerable<DDField> fields)
       {
           _fields = new List<DDField>(fields);
       }
       public void ApplyConditions(IList<string> conditions, IList<string> values)
       {
       
               _queries = new List<IQuery>();

               for (int i = 0; i < conditions.Count; i++)
               {
                   if (conditions[i] == "")
                   {
                       _queries.Add(new EmptyQuery());
                       continue;
                   }

                   var field = _fields[i];
                   var value = values[i];
                   // create query based on type
                   var q = new StringQuery();
                   q.ApplyFilter(field, conditions[i], value);
                   _queries.Add(q);
               }
           
       }

       public void ApplyOperators(IList<string> operators)
       {
           QueryResult last = null;
           for (int i = 0; i < operators.Count; i++)
           {
               if (last == null)
                {
                    last = new QueryResult()
                {
                       Operator = operators[i],
                       Query = _queries[i]
                   };

                    _result = last;
                }
               else
               {
                   last.Next = new QueryResult()
                   {
                       Operator = operators[i],
                       Query = _queries[i]
                   };
                   last = last.Next;
               }
           }
              
       }

       public IEnumerable<Entry> Execute()
       {
           return _result.Execute(SourceEntries);

       }

    }
}
