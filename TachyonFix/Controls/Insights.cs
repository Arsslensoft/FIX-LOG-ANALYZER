using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TachyonFix.Core;

namespace TachyonFix.Controls
{
    public partial class Insights : UserControl
    {
        public Insights()
        {
            InitializeComponent();
        }
        
        public void BrokerChanged(TachyonFix.Core.Broker b)
        {
            panel3.Controls.Clear();
            foreach (var insightsInsight in b.Insights.Insights)
            {
                var ic = new InsightTip();
                ic.Problem = insightsInsight.Problem;
                ic.Solution = insightsInsight.Recommendation;
                ic.Dock = DockStyle.Top;
               panel3.Controls.Add(ic);
            }
        }
    }
}
