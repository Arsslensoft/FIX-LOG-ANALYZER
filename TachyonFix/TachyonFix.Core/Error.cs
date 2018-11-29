using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickFix;

namespace TachyonFix.Core
{
    public enum ErrorType
    {
        Syntax,
        Invalid,
        Semantic
    }
   public class Error : BaseEntry
    {
        public ErrorType ErrorType { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }  
        public Message ParsedMessage { get; set; }
       

    }
}
