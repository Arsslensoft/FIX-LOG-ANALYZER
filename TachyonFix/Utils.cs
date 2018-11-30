using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar.Controls;

namespace VisualStudio2012Style
{
   public class Utils
    {
        public static DataTable ListViewToDataTable(ListViewEx list, BackgroundWorker bw)
        {
            var dt = new DataTable("Data");
            list.Invoke(new Action(() =>
            {
                foreach (var listColumn in list.Columns)
                    dt.Columns.Add((listColumn as ColumnHeader).Text);
                int c = 0;

                foreach (var listItem in list.Items)
                {
                    bw.ReportProgress((int) ((c / (double) list.Items.Count) * 100), "Converting...");
                    var item = listItem as ListViewItem;
                    var values = new object[dt.Columns.Count];
                    for (int i = 0; i < item.SubItems.Count; i++)
                        values[i] = item.SubItems[i].Text;


                    dt.Rows.Add(values);
                }
            }));
            return dt;
        }
    }
}
