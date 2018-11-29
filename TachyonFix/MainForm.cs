using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.AdvTree;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Metro;
using Newtonsoft.Json;
using QuickFix.DataDictionary;
using QuickFix.Fields;
using TachyonFix.Core;
using TachyonFix.Core.Insights;
using ColumnHeader = System.Windows.Forms.ColumnHeader;

namespace VisualStudio2012Style
{
    public partial class MainForm : MetroForm
    {
        private QuickFix.DataDictionary.DataDictionary dd;
        Dictionary<string, ComboBoxItem> _operationsBoxItems = new Dictionary<string, ComboBoxItem>();
        Dictionary<int, ComboBoxItem> _fieldsBoxItems = new Dictionary<int, ComboBoxItem>();
        Dictionary<string, ComboBoxItem> _conditionBoxItems = new Dictionary<string, ComboBoxItem>();
        public MainForm()
        {
            InitializeComponent();
            Init();
        }

        void Init()
        {
            LoadOpAndConditions();
            LoadDictionaryAndFields();
            LoadBaseRows();
        }


        void LoadDictionaryAndFields()
        {
            InsightCalculator.InsightsDatabase =
                JsonConvert.DeserializeObject<InsightsDatabase>(
                    File.ReadAllText(Path.Combine(Application.StartupPath, "insights.json")));
            dd = new QuickFix.DataDictionary.DataDictionary();

            Stream stream = new FileStream(Path.Combine(Application.StartupPath, "spec", "FIX42.xml"), FileMode.Open, FileAccess.Read);
            dd.Load(stream);

            foreach (var ddField in dd.FieldsByTag)
            {
                var c = new ComboBoxItem();
                c.Text = ddField.Value.Name + $" ({ddField.Key})";
                c.Tag = ddField.Value;
                _fieldsBoxItems.Add(ddField.Key, c);
                FieldCol.Items.Add(c);
            }
        }
        void LoadOpAndConditions()
        {
            foreach (var ddField in new []{ "", "AND", "OR", "NOT"})
            {
                var c = new ComboBoxItem();
                c.Text = ddField;
                c.Tag = ddField;
                _operationsBoxItems.Add(ddField, c);
                OperationCol.Items.Add(c);
            }

            foreach (var ddField in new [] { "", "=", ">", "<", "!=", ">=", "<=", "CONTAINS", "STARTS_WITH", "ENDS_WITH" })
            {
                var c = new ComboBoxItem();
                c.Text = ddField;
                c.Tag = ddField;
                _conditionBoxItems.Add(ddField, c);
                ConditionCol.Items.Add(c);
            }
        }

        void AddDefaultRow(ComboBoxItem op, ComboBoxItem field, ComboBoxItem cond, bool v)
        {
            dataGridViewX1.Rows.Add(op, field, cond,"", v);
            var item = new ColumnHeader();
            item.Text = field.Text;
            item.Tag = field.Tag as DDField;
            messagesList.Columns.Add(item);
        }
        void LoadBaseRows()
        {
            AddDefaultRow(_operationsBoxItems[""], _fieldsBoxItems[34], _operationsBoxItems[""], true);
            AddDefaultRow(_operationsBoxItems[""], _fieldsBoxItems[35], _operationsBoxItems[""], true);
            AddDefaultRow(_operationsBoxItems[""], _fieldsBoxItems[49], _operationsBoxItems[""], true);
            AddDefaultRow(_operationsBoxItems[""], _fieldsBoxItems[52], _operationsBoxItems[""], true);
            AddDefaultRow(_operationsBoxItems[""], _fieldsBoxItems[56], _operationsBoxItems[""], true);
            AddDefaultRow(_operationsBoxItems[""], _fieldsBoxItems[22], _operationsBoxItems[""], true);
            UpdateColumnsWidth();
        }


        void ClearSolution()
        {
            messagesListView.Items.Clear();
            LogNode.Nodes.Clear();
        }

        void DisplayErrors()
        {
            if (CurrentSolution != null)
            {
                foreach (var currentSolutionError in CurrentSolution.Errors)
                {
                    ListViewItem item = messagesListView.Items.Add(new ListViewItem(currentSolutionError.Message));
                    //     message.FileName = CSol.GetProjectByName(message.Project).TempService.GetFileFromTemp(message.FileName);
                    item.SubItems.Add(currentSolutionError.Exception.GetType().Name);
                    item.SubItems.Add(currentSolutionError.Index.ToString());
                    item.Tag = currentSolutionError;
              
                    switch (currentSolutionError.ErrorType)
                    {
                        case ErrorType.Syntax:
                            item.ForeColor = System.Drawing.Color.Red;
                            item.ImageKey = "error";
                            break;
                        case ErrorType.Invalid:
                            item.ForeColor = System.Drawing.Color.Red;
                            item.ImageKey = "warning";
                            break;
                    }

                }
            }
        }
        void InitializeSolution()
        {
            DisplayErrors();
            SelectedSource = CurrentSolution.Entries;
            LoadBrokers();
            dataFetcher.RunWorkerAsync();
            analysisWorker.RunWorkerAsync();
        }

        void LoadBrokers()
        {
            LogNode.ImageKey = "log";
            foreach (var broker in CurrentSolution.Intermediaries)
            {
                Node n = new Node(broker.Value.Name);
                n.Tag = broker.Value;
                n.Name = broker.Value.Name;
                if (broker.Value.Trader)
                {
                    n.ImageKey = "trader";
                    LogNode.Nodes.Insert(0,n);
                }
                else if (n.Name == "UNKNOWN")
                {
                    n.ImageKey = "unknown";
                    LogNode.Nodes.Add(n);
                }
                else
                {
                    n.ImageKey = "broker";
                    LogNode.Nodes.Add(n);
                }
            }
        }
        public static Solution CurrentSolution { get; set; }
        public static List<Entry> SelectedSource { get; set; }
        public static string SolutionFile { get; set; }
        private void openbtn_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                SolutionFile = openFileDialog1.FileName;
                progressBarItem1.Visible = true;
                importWorker.RunWorkerAsync();
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            ClearSolution();
            CurrentSolution = TachyonFix.Core.Solution.Open(SolutionFile, importWorker, dd);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                statuslb.Text = "Ready";
                progressBarItem1.Value = 0;
                progressBarItem1.Visible = false;
                MessageBoxEx.Show("Log Import completed", "Log Import", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                InitializeSolution();
            }));
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.bar1.Invoke(new Action(() =>
            {
                
                statuslb.Text = "Importing "+ SolutionFile + "...";
                progressBarItem1.Value = e.ProgressPercentage;
           
            }));
          

        }

        private void analysisWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            progressBarItem1.Visible = true;
            CurrentSolution?.Analyze(analysisWorker);
        }

        private void analysisWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.bar1.Invoke(new Action(() =>
            {

                statuslb.Text = e.UserState.ToString();
                progressBarItem1.Value = e.ProgressPercentage;

            }));
        }

        private void analysisWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                statuslb.Text = "Ready";
                progressBarItem1.Value = 0;
                progressBarItem1.Visible = false;
            }));
        }
        #region Messages and Grid
        void UpdateColumnsWidth()
        {
            foreach (var messagesListColumn in messagesList.Columns)
               ( messagesListColumn as ColumnHeader).Width = messagesList.Width / (messagesList.Columns.Count + 1);
        }
        private void dataGridViewX1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0 || e.RowIndex == dataGridViewX1.RowCount - 1)
                return;
            TryConvertRow(e.RowIndex);
            if (messagesList.Columns.Count > e.RowIndex)
            {
                // update column if field is changed
                if (e.ColumnIndex == 1)
                {
                    var item = messagesList.Columns[e.RowIndex] ;
                    item.Text = dataGridViewX1[e.ColumnIndex, e.RowIndex].Value.ToString();
                    item.Tag = (dataGridViewX1[e.ColumnIndex, e.RowIndex].Value as ComboBoxItem)?.Tag as DDField;
                    messagesList.Columns.RemoveAt(e.RowIndex);
                    messagesList.Columns.Insert(e.RowIndex, item);
                    UpdateColumnsWidth();
                }
                // hide
                if (e.ColumnIndex == 4 && (bool) (dataGridViewX1[e.ColumnIndex, e.RowIndex].Value) == false)
                {
                    messagesList.Columns.RemoveAt(e.RowIndex);
                    UpdateColumnsWidth();
                }
                
                // show
                if (e.ColumnIndex == 4 && (bool)(dataGridViewX1[e.ColumnIndex, e.RowIndex].Value) &&
                    dataGridViewX1[1, e.RowIndex].Value is ComboBoxItem)
                {
                    var item = new ColumnHeader();
                    item.Text = dataGridViewX1[1, e.RowIndex].Value.ToString();
                    item.Tag = (dataGridViewX1[1, e.RowIndex].Value as ComboBoxItem)?.Tag as DDField;
           
                    messagesList.Columns.Insert(e.RowIndex, item);
                    UpdateColumnsWidth();
                }
            }
            else
            {

                // add new column if field is changed
                if (e.ColumnIndex == 1)
                {
                    var item = new ColumnHeader();
                    item.Text = dataGridViewX1[e.ColumnIndex, e.RowIndex].Value.ToString();
                    item.Tag = (dataGridViewX1[e.ColumnIndex, e.RowIndex].Value as ComboBoxItem)?.Tag as DDField;
              
                    messagesList.Columns.Add(item);
                    UpdateColumnsWidth();
                }
               
            }

        }
        void TryConvertRow(int rowIndex)
        {
            var dr = dataGridViewX1.Rows[rowIndex];
            if (dr.Cells[0].Value != null && !(dr.Cells[0].Value is ComboBoxItem))
            {
                var op = dr.Cells[0].Value.ToString();
                if (this._operationsBoxItems.ContainsKey(op))
                    dr.Cells[0].Value = this._operationsBoxItems[op];
                else dr.Cells[0].Value = this._operationsBoxItems[""];
            }
            else if (dr.Cells[0].Value == null)
                dr.Cells[0].Value = this._operationsBoxItems[""];

            if (dr.Cells[1].Value != null && !(dr.Cells[1].Value is ComboBoxItem))
            {
                var f = dr.Cells[1].Value.ToString().Remove(0, dr.Cells[1].Value.ToString().IndexOf('(') + 1);
                f = f.Substring(0, f.IndexOf(')'));
                int field = 0;
                if (int.TryParse(f, out field))
                {
                    if (this._fieldsBoxItems.ContainsKey(field))
                        dr.Cells[1].Value = this._fieldsBoxItems[field];
                    else dr.Cells[1].Value = this._fieldsBoxItems.FirstOrDefault().Value;
                }
                else dr.Cells[1].Value = this._fieldsBoxItems.FirstOrDefault().Value;
            }
            else if (dr.Cells[1].Value == null)
                dr.Cells[1].Value = this._fieldsBoxItems.FirstOrDefault().Value;

            if (dr.Cells[2].Value != null && !(dr.Cells[2].Value is ComboBoxItem))
            {
                var c = dr.Cells[2].Value.ToString();
                if (this._conditionBoxItems.ContainsKey(c))
                    dr.Cells[2].Value = this._conditionBoxItems[c];
                else dr.Cells[2].Value = this._conditionBoxItems[""];
            }
            else if (dr.Cells[2].Value == null)
                dr.Cells[2].Value = this._conditionBoxItems[""];

            if (dr.Cells[3].Value == null)
                dr.Cells[3].Value = "";

            if (dr.Cells[4].Value == null || !(dr.Cells[4].Value is bool))
                dr.Cells[4].Value = true;
        }
        private void dataGridViewX1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            TryConvertRow(e.RowIndex);
        }
        private void messagesList_SizeChanged(object sender, EventArgs e)
        {
            UpdateColumnsWidth();
        }
        #endregion

        List<Entry> GetDefaultSource()
        {
            if (advTree1.SelectedNode != null)
                    return (advTree1.SelectedNode.Tag as Broker).Entries;
            else return CurrentSolution.Entries;
        }
        void FetchData()
        {
            dataFetcher.ReportProgress(1, "Preparing query...");
            var qe = new QueryEngine(GetDefaultSource());
            var fields = new List<DDField>();
            var  ops = new List<string>();
            var cond = new List<string>();
            var values = new List<string>();
            foreach (var row in dataGridViewX1.Rows)
            {
                var dr = row as DataGridViewRow;
                if(dr.Cells.Count != 5 || !(dr.Cells[0].Value is ComboBoxItem) || dr.Index == dataGridViewX1.RowCount - 1)
                    continue;
                ops.Add((dr.Cells[0].Value as ComboBoxItem).Text );
                fields.Add((dr.Cells[1].Value as ComboBoxItem).Tag as DDField);
                cond.Add(dr.Cells[2].Value.ToString());
                values.Add(dr.Cells[3].Value.ToString());
            }
            if (fields.Count > 0) { 
                qe.ApplyFields(fields);
            qe.ApplyConditions(cond, values);
            qe.ApplyOperators(ops);
                dataFetcher.ReportProgress(50, "Executing query...");
                var res = qe.Execute();
                var result = new List<Entry>(res);
                SelectedSource = result;
                recordperpage.Maximum = result.Count;
                recordperpage.Minimum = 1;
                recordperpage.Value = 10;
                int p = result.Count / recordperpage.Value;
                if (result.Count % recordperpage.Value != 0)
                    p++;

                page.Minimum = 1;
                page.Maximum = p;
                page.Value = 1;

            }
        }

        void DisplayData()
        {
            if (SelectedSource != null && messagesList.Columns.Count > 0 && SelectedSource.Count > 0)
            {
                messagesList.Invoke(new Action(() =>
                {
                    messagesList.Items.Clear();
                }));

                QueryEngine qe = new QueryEngine(SelectedSource);
                var fields = new List<DDField>();
                foreach (var row in dataGridViewX1.Rows)
                {
                    var dr = row as DataGridViewRow;
                    if (dr.Cells.Count != 5 || !(dr.Cells[0].Value is ComboBoxItem))
                        continue;
                    fields.Add((dr.Cells[1].Value as ComboBoxItem).Tag as DDField);
                }
                double progress = 0;
                double count = recordperpage.Value;
                int c = 0;
                int start = Math.Min(SelectedSource.Count - 1, (page.Value-1) * recordperpage.Value),  stop = Math.Min(SelectedSource.Count - 1, page.Value * recordperpage.Value);
                foreach (var entry in SelectedSource.GetRange(start, stop - start))
                {
                    c++;
                    progress = ((double)c / count) * 100;
                    messagesList.Invoke(new Action(() =>
                    {
                        ListViewItem item = messagesList.Items.Add(new ListViewItem(qe.GetValueFromEntry(fields[0].Tag, entry)));
                        for (int i = 1; i < fields.Count; i++)
                            item.SubItems.Add(qe.GetValueFromEntry(fields[i].Tag, entry));
                        item.Tag = entry;
                        var m = entry.Message ?? entry.Error.ParsedMessage;
                        if (m != null)
                        {
                            if (m.IsAdmin())
                                item.ImageKey = "admin";
                            else item.ImageKey = "app";

                            if (entry.Message == null)
                                item.ForeColor = Color.Red;
                        }
                        dataDisplay.ReportProgress((int)progress, "Displaying data...");
                    }));

                }
            }
           
        }
        private void dataFetcher_DoWork(object sender, DoWorkEventArgs e)
        {
            this.Invoke(new Action(() =>
            {
       
                progressBarItem1.Visible = true;
                messagesList.Items.Clear();
                dataGridViewX1.Enabled = false;
                buttonItem6.Enabled = false;
           
            }));
           
            FetchData();
        }

        private void dataFetcher_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.bar1.Invoke(new Action(() =>
            {

                statuslb.Text = e.UserState.ToString();
                progressBarItem1.Value = e.ProgressPercentage;

            }));
        }

        private void dataFetcher_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                statuslb.Text = "Ready";
                progressBarItem1.Value = 0;
                progressBarItem1.Visible = false;
                dataGridViewX1.Enabled = true;
                buttonItem6.Enabled = true;
         
            }));
            if (!dataDisplay.IsBusy)
                dataDisplay.RunWorkerAsync();
        }

        private void buttonItem6_Click(object sender, EventArgs e)
        {
            dataFetcher.RunWorkerAsync();
        }

        private void dataDisplay_DoWork(object sender, DoWorkEventArgs e)
        {
            this.Invoke(new Action(() =>
            {

                progressBarItem1.Visible = true;
                messagesList.Items.Clear();
                dataGridViewX1.Enabled = false;
                buttonItem6.Enabled = false;
              
            }));
            DisplayData();
        }

        private void dataDisplay_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                statuslb.Text = "Ready";
                progressBarItem1.Value = 0;
                progressBarItem1.Visible = false;
                dataGridViewX1.Enabled = true;
                buttonItem6.Enabled = true;
           
            }));
      
        }

        private void dataDisplay_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.bar1.Invoke(new Action(() =>
            {

                statuslb.Text = e.UserState.ToString();
                progressBarItem1.Value = e.ProgressPercentage;

            }));
        }

        private void recordperpage_ValueChanged(object sender, EventArgs e)
        {
            
            if(!dataFetcher.IsBusy && !dataDisplay.IsBusy)
                dataDisplay.RunWorkerAsync();
            this.Invoke(new Action(() =>
            {
                page.Text = "Page " + page.Value;
                recordperpage.Text = "Records per page " + recordperpage.Value;

            }));

        
        }
        void UpdateColumnsWidthForDetails()
        {
            foreach (var messagesListColumn in messageDetailsList.Columns)
                (messagesListColumn as ColumnHeader).Width = messageDetailsList.Width / (messageDetailsList.Columns.Count + 1);
        }
        private void messageDetailsList_SizeChanged(object sender, EventArgs e)
        {
            UpdateColumnsWidthForDetails();
        }

        private void messagesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = messagesList.SelectedItems.Count > 0 ? messagesList.SelectedItems[0] : null;
            if (item != null)
            {
                var entry = item.Tag as Entry;
                var message = entry?.Message ?? entry?.Error.ParsedMessage;
                if (message != null)
                {
                    messageDetailsList.Items.Clear();
               
                    foreach (var h in message.Header)
                    {
                        var ddf = _fieldsBoxItems.ContainsKey(h.Key) ? (_fieldsBoxItems[h.Key].Tag as DDField) : null;
                        var ditem = messageDetailsList.Items.Add(h.Key.ToString());
                        if (ddf != null)
                            ditem.SubItems.Add(ddf.Name).ForeColor = Color.FromArgb(255, 22, 160, 133);
                        else ditem.SubItems.Add(h.Key.ToString()).ForeColor = Color.FromArgb(255, 22, 160, 133);
                        ditem.SubItems.Add(h.Value.ToString()).ForeColor = Color.FromArgb(255, 22, 160, 133);
                        if (ddf != null)
                            ditem.SubItems.Add(ddf.FixFldType).ForeColor = Color.FromArgb(255, 22, 160, 133);
                        else ditem.SubItems.Add("DEFAULT").ForeColor = Color.FromArgb(255, 22, 160, 133);

                        ditem.ForeColor = Color.FromArgb(255, 22, 160, 133);
                    }
                    foreach (var h in message)
                    {
                        var ddf = _fieldsBoxItems.ContainsKey(h.Key) ? (_fieldsBoxItems[h.Key].Tag as DDField) : null;
                        var ditem = messageDetailsList.Items.Add(h.Key.ToString());
                        if(ddf != null)
                            ditem.SubItems.Add(ddf.Name).ForeColor = Color.FromArgb(255, 192, 57, 43);
                        else ditem.SubItems.Add(h.Key.ToString()).ForeColor = Color.FromArgb(255, 192, 57, 43);
                        ditem.SubItems.Add(h.Value.ToString()).ForeColor = Color.FromArgb(255, 192, 57, 43);
                        if (ddf != null)
                            ditem.SubItems.Add(ddf.FixFldType).ForeColor = Color.FromArgb(255, 192, 57, 43);
                        else ditem.SubItems.Add("DEFAULT").ForeColor = Color.FromArgb(255, 192, 57, 43);

                        ditem.ForeColor = Color.FromArgb(255,192, 57, 43);
                  
                    }
                    foreach (var h in message.Trailer)
                    {
                        var ddf = _fieldsBoxItems.ContainsKey(h.Key) ? (_fieldsBoxItems[h.Key].Tag as DDField) : null;
                        var ditem = messageDetailsList.Items.Add(h.Key.ToString());
                        if (ddf != null)
                            ditem.SubItems.Add(ddf.Name).ForeColor = Color.FromArgb(255, 25, 42, 86);
                        else ditem.SubItems.Add(h.Key.ToString()).ForeColor = Color.FromArgb(255, 25, 42, 86);
                        ditem.SubItems.Add(h.Value.ToString()).ForeColor = Color.FromArgb(255, 25, 42, 86);
                        if (ddf != null)
                            ditem.SubItems.Add(ddf.FixFldType).ForeColor = Color.FromArgb(255, 25, 42, 86);
                        else ditem.SubItems.Add("DEFAULT").ForeColor = Color.FromArgb(255, 25, 42, 86);
                      
                        ditem.ForeColor = Color.FromArgb(255, 25, 42, 86);
                    }
                    rawMessage.Text = entry.Content;

                }
            }
        }
    }
}