using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TachyonFix.Core.Reporting
{
   public abstract class Exporter
    {
        public event Action<double, string> OnProgressChanged;

        protected void ReportProgress(double p, string m)
        {
            OnProgressChanged?.Invoke(p,m);
        }
        public abstract string ExporterName { get; set; }
        public abstract string FileExtensions { get; set; }
        public abstract void Export(string f, DataTable dt);
      
    }
}
