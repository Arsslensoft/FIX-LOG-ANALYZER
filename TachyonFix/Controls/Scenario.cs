using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using Northwoods.Go;
using QuickFix;
using TachyonFix;
using TachyonFix.Core;
using TachyonFix.Core.Scenarios;
using Message = TachyonFix.Message;

namespace TachyonFix.Controls
{
    public partial class Scenario : UserControl
    {
        public Scenario()
        {
            InitializeComponent();
        }
        Dictionary<string, Lifeline> lifelines = new Dictionary<string, Lifeline>();
        public void BrokerChanged(TachyonFix.Core.Broker b)
        {
       
            var events = MainForm.CurrentSolution.Linker.Events.Where(x => x.Source == b || x.Target == b);
            Events = events.ToList();
            end.MaxValue = Events.Count - 1;
            start.MaxValue = Events.Count - 2;
            end.MinValue = 0;
            start.MinValue = 0;
           
        }
        private List<Event> Events;
        private void buttonX1_Click(object sender, EventArgs ea)
        {
            if (Events != null && Events.Count > 0)
            {
                if (start.Value >= end.Value)
                {
                    MessageBoxEx.Show("Start event id should always be les than end event id", "Events",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    
                    return;

                }
                lifelines.Clear();
                GoDocument doc = goView1.Document;
                doc.AllowLink = false;
                doc.AllowEdit = false;
                doc.AllowResize = false;
                doc.Clear();
           
                for(int i = start.Value; i <= end.Value; i++)
                {
                    var e = Events[i];
                    Lifeline lf = null;
                    Lifeline s = null, t = null;
                    if (e.Unknow && !lifelines.ContainsKey("Unknown"))
                    {
                        lf = new Lifeline("Unknown");
                        doc.Add(lf);
                        lifelines.Add(lf.Text, lf);
                    }
                    if (e.Source != null && !lifelines.ContainsKey(e.Source.Name))
                    {
                        lf = new Lifeline(e.Source.Name);
                        doc.Add(lf);
                        lifelines.Add(lf.Text, lf);
                      
                    }
                    if (e.Target != null && !lifelines.ContainsKey(e.Target.Name))
                    {
                        lf = new Lifeline(e.Target.Name);
                        doc.Add(lf);
                        lifelines.Add(lf.Text, lf);
                    }

                    if (e.Unknow)
                    {
                        if (e.Source == null)
                        {
                            s = lifelines["Unknown"];
                            t = lifelines[e.Target.Name];
                        }
                        else if (e.Target == null)
                        {
                            t = lifelines["Unknown"];
                            s = lifelines[e.Source.Name];
                        }
                        else continue;
                    }
                    else
                    {
                        t = lifelines[e.Target.Name];
                        s = lifelines[e.Source.Name];
                    }
                    var m = new Message(i - start.Value, s, t, e.Message.GetType().Name, 2, e.Message);
                    m.OnMessageClicked += message => Program.MainInstance.DisplayMessageInfo(message);
                    doc.Add(m);

                }
                int margin = integerInput1.Value;
                foreach (var lifeline in lifelines)
                {
                    lifeline.Value.Left = margin;
                    margin *= 2;
                }
                doc.Bounds = doc.ComputeBounds();
                goView1.DocPosition = doc.TopLeft;

                goView1.GridUnboundedSpots = GoObject.BottomLeft | GoObject.BottomRight;
                goView1.Grid.Top = Lifeline.LineStart;
                goView1.GridOriginY = Lifeline.LineStart;
                goView1.GridCellSizeHeight = Lifeline.MessageSpacing;

                // support undo/redo
                doc.UndoManager = new GoUndoManager();

            }
        


        }
    }
}
