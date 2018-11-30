using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VisualStudio2012Style.Controls
{
    public partial class InsightTip : UserControl
    {
        public InsightTip()
        {
            InitializeComponent();
        }

        public string Problem
        {
            get => expandablePanel1.TitleText;
            set => expandablePanel1.TitleText = value;
        }

        public string Solution
        {
            get => labelX1.Text;
            set => labelX1.Text = value;
        }

        private void expandablePanel1_ExpandedChanged(object sender, DevComponents.DotNetBar.ExpandedChangeEventArgs e)
        {
            this.Height = expandablePanel1.Height + 10;
        }
    }
}
