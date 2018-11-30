using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar.Charts;
using TachyonFix.Core;

namespace TachyonFix.Controls
{

    public partial class GlobalStats : UserControl
    {

        public GlobalStats()
        {
            InitializeComponent();    
        }

        public void BrokerChanged(TachyonFix.Core.Solution s)
        {
            DisplayTopStats(s);
        
        }



        void DisplayTopStats(Solution s)
        {
           panel1.Controls.Clear();
            panel1.Controls.Add(DisplayPieChart("Message Count", "Message distribution by party", s.GlobalStatsCalculator.Stats.BrokerMessagesDistribution));
            panel1.Controls.Add(DisplayDonutChart("Known/Unknown", "Fix vs Other", s.GlobalStatsCalculator.Stats.KnownVsUnknownMessages));
            panel1.Controls.Add(DisplayPieChart("Message Type", "Message type distribution", s.GlobalStatsCalculator.Stats.MessageTypeDistribution));

            foreach (var c in panel1.Controls)
            {
                var control = c as Control;
                control.Width = panel1.Width / 3;
            }

        }
        ChartControl DisplayPieChart(string series, string name, List<KeyValuePair<string, int>> values)
        {
            var chartControl1 = new DevComponents.DotNetBar.Charts.ChartControl();
            DevComponents.DotNetBar.Charts.PieChart pieChart1 = new DevComponents.DotNetBar.Charts.PieChart();
            DevComponents.DotNetBar.Charts.PieSeries pieSeries1 = new DevComponents.DotNetBar.Charts.PieSeries();

            DevComponents.DotNetBar.Charts.Style.Background background1 = new DevComponents.DotNetBar.Charts.Style.Background();
            DevComponents.DotNetBar.Charts.Style.BorderColor borderColor1 = new DevComponents.DotNetBar.Charts.Style.BorderColor();
            DevComponents.DotNetBar.Charts.Style.Thickness thickness1 = new DevComponents.DotNetBar.Charts.Style.Thickness();
            DevComponents.DotNetBar.Charts.Style.Padding padding1 = new DevComponents.DotNetBar.Charts.Style.Padding();
            DevComponents.DotNetBar.Charts.Style.Background background2 = new DevComponents.DotNetBar.Charts.Style.Background();
            DevComponents.DotNetBar.Charts.Style.BorderColor borderColor2 = new DevComponents.DotNetBar.Charts.Style.BorderColor();
            DevComponents.DotNetBar.Charts.Style.Thickness thickness2 = new DevComponents.DotNetBar.Charts.Style.Thickness();
            DevComponents.DotNetBar.Charts.Style.Padding padding2 = new DevComponents.DotNetBar.Charts.Style.Padding();
            DevComponents.DotNetBar.Charts.Style.Background background3 = new DevComponents.DotNetBar.Charts.Style.Background();
            DevComponents.DotNetBar.Charts.Style.BorderColor borderColor3 = new DevComponents.DotNetBar.Charts.Style.BorderColor();
            DevComponents.DotNetBar.Charts.Style.Thickness thickness3 = new DevComponents.DotNetBar.Charts.Style.Thickness();
            DevComponents.DotNetBar.Charts.Style.Padding padding3 = new DevComponents.DotNetBar.Charts.Style.Padding();
            DevComponents.DotNetBar.Charts.Style.Padding padding4 = new DevComponents.DotNetBar.Charts.Style.Padding();
            DevComponents.DotNetBar.Charts.ChartTitle chartTitle1 = new DevComponents.DotNetBar.Charts.ChartTitle();
            DevComponents.DotNetBar.Charts.Style.Padding padding5 = new DevComponents.DotNetBar.Charts.Style.Padding();
            DevComponents.DotNetBar.Charts.Style.Background background4 = new DevComponents.DotNetBar.Charts.Style.Background();
            DevComponents.DotNetBar.Charts.Style.Background background5 = new DevComponents.DotNetBar.Charts.Style.Background();
            DevComponents.DotNetBar.Charts.Style.Background background6 = new DevComponents.DotNetBar.Charts.Style.Background();
            DevComponents.DotNetBar.Charts.Style.Background background7 = new DevComponents.DotNetBar.Charts.Style.Background();
            DevComponents.DotNetBar.Charts.Style.Background background8 = new DevComponents.DotNetBar.Charts.Style.Background();
            DevComponents.DotNetBar.Charts.Style.Background background9 = new DevComponents.DotNetBar.Charts.Style.Background();
            DevComponents.DotNetBar.Charts.Style.Background background10 = new DevComponents.DotNetBar.Charts.Style.Background();
            DevComponents.DotNetBar.Charts.Style.Background background11 = new DevComponents.DotNetBar.Charts.Style.Background();
            // 
            // chartControl1
            // 
            chartControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(239)))), ((int)(((byte)(242)))));
            pieSeries1.Name = series;
            foreach (var keyValuePair in values)
            {
                DevComponents.DotNetBar.Charts.PieSeriesPoint p = new DevComponents.DotNetBar.Charts.PieSeriesPoint();
                p.ValueX = keyValuePair.Key;
                p.ValueY = new object[] {
                    ((object)(keyValuePair.Value))};

                pieSeries1.SeriesPoints.Add(p);
            }
        
  
            pieSeries1.SeriesType = DevComponents.DotNetBar.Charts.SeriesType.Pie;
            pieChart1.ChartSeries.Add(pieSeries1);
            background1.Color1 = System.Drawing.Color.White;
            pieChart1.ChartVisualStyle.Background = background1;
            borderColor1.Bottom = System.Drawing.Color.Black;
            borderColor1.Left = System.Drawing.Color.Black;
            borderColor1.Right = System.Drawing.Color.Black;
            borderColor1.Top = System.Drawing.Color.Black;
            pieChart1.ChartVisualStyle.BorderColor = borderColor1;
            thickness1.Bottom = 1;
            thickness1.Left = 1;
            thickness1.Right = 1;
            thickness1.Top = 1;
            pieChart1.ChartVisualStyle.BorderThickness = thickness1;
            padding1.Bottom = 6;
            padding1.Left = 6;
            padding1.Right = 6;
            padding1.Top = 6;
            pieChart1.ChartVisualStyle.Padding = padding1;
            background2.Color1 = System.Drawing.Color.White;
            pieChart1.ContainerVisualStyles.Default.Background = background2;
            borderColor2.Bottom = System.Drawing.Color.DimGray;
            borderColor2.Left = System.Drawing.Color.DimGray;
            borderColor2.Right = System.Drawing.Color.DimGray;
            borderColor2.Top = System.Drawing.Color.DimGray;
            pieChart1.ContainerVisualStyles.Default.BorderColor = borderColor2;
            thickness2.Bottom = 1;
            thickness2.Left = 1;
            thickness2.Right = 1;
            thickness2.Top = 1;
            pieChart1.ContainerVisualStyles.Default.BorderThickness = thickness2;
            pieChart1.ContainerVisualStyles.Default.DropShadow.Enabled = DevComponents.DotNetBar.Charts.Style.Tbool.True;
            padding2.Bottom = 6;
            padding2.Left = 6;
            padding2.Right = 6;
            padding2.Top = 6;
            pieChart1.ContainerVisualStyles.Default.Padding = padding2;
            pieChart1.IsExploded = true;
            pieChart1.Legend.Alignment = DevComponents.DotNetBar.Charts.Style.Alignment.BottomCenter;
            pieChart1.Legend.AlignVerticalItems = true;
            background3.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            pieChart1.Legend.ChartLegendVisualStyles.Default.Background = background3;
            borderColor3.Bottom = System.Drawing.Color.Black;
            borderColor3.Left = System.Drawing.Color.Black;
            borderColor3.Right = System.Drawing.Color.Black;
            borderColor3.Top = System.Drawing.Color.Black;
            pieChart1.Legend.ChartLegendVisualStyles.Default.BorderColor = borderColor3;
            thickness3.Bottom = 1;
            thickness3.Left = 1;
            thickness3.Right = 1;
            thickness3.Top = 1;
            pieChart1.Legend.ChartLegendVisualStyles.Default.BorderThickness = thickness3;
            padding3.Bottom = 4;
            padding3.Left = 4;
            padding3.Right = 4;
            padding3.Top = 4;
            pieChart1.Legend.ChartLegendVisualStyles.Default.Margin = padding3;
            padding4.Bottom = 4;
            padding4.Left = 4;
            padding4.Right = 4;
            padding4.Top = 4;
            pieChart1.Legend.ChartLegendVisualStyles.Default.Padding = padding4;
            pieChart1.Legend.Direction = DevComponents.DotNetBar.Charts.Direction.LeftToRight;
            pieChart1.Legend.MaxVerticalPct = 90D;
            pieChart1.Legend.Placement = DevComponents.DotNetBar.Charts.Placement.Outside;
            pieChart1.Legend.Visible = true;
            pieChart1.Name = "PieChart1";
            chartTitle1.ChartTitleVisualStyle.Alignment = DevComponents.DotNetBar.Charts.Style.Alignment.MiddleCenter;
            chartTitle1.ChartTitleVisualStyle.Font = new System.Drawing.Font("Georgia", 16F);
            padding5.Bottom = 8;
            padding5.Left = 8;
            padding5.Right = 8;
            padding5.Top = 8;
            chartTitle1.ChartTitleVisualStyle.Padding = padding5;
            chartTitle1.ChartTitleVisualStyle.TextColor = System.Drawing.Color.Navy;
            chartTitle1.Name = "Title1";
            chartTitle1.Text = name;
            chartTitle1.XyAlignment = DevComponents.DotNetBar.Charts.XyAlignment.Top;
            pieChart1.Titles.Add(chartTitle1);
            chartControl1.ChartPanel.ChartContainers.Add(pieChart1);
            chartControl1.ChartPanel.Legend.Visible = false;
            chartControl1.ChartPanel.Name = "PrimaryPanel";
            background4.Color1 = System.Drawing.Color.AliceBlue;
            chartControl1.DefaultVisualStyles.HScrollBarVisualStyles.MouseOver.ArrowBackground = background4;
            background5.Color1 = System.Drawing.Color.AliceBlue;
            chartControl1.DefaultVisualStyles.HScrollBarVisualStyles.MouseOver.ThumbBackground = background5;
            background6.Color1 = System.Drawing.Color.White;
            chartControl1.DefaultVisualStyles.HScrollBarVisualStyles.SelectedMouseOver.ArrowBackground = background6;
            background7.Color1 = System.Drawing.Color.White;
            chartControl1.DefaultVisualStyles.HScrollBarVisualStyles.SelectedMouseOver.ThumbBackground = background7;
            background8.Color1 = System.Drawing.Color.AliceBlue;
            chartControl1.DefaultVisualStyles.VScrollBarVisualStyles.MouseOver.ArrowBackground = background8;
            background9.Color1 = System.Drawing.Color.AliceBlue;
            chartControl1.DefaultVisualStyles.VScrollBarVisualStyles.MouseOver.ThumbBackground = background9;
            background10.Color1 = System.Drawing.Color.White;
            chartControl1.DefaultVisualStyles.VScrollBarVisualStyles.SelectedMouseOver.ArrowBackground = background10;
            background11.Color1 = System.Drawing.Color.White;
            chartControl1.DefaultVisualStyles.VScrollBarVisualStyles.SelectedMouseOver.ThumbBackground = background11;
            chartControl1.Dock = System.Windows.Forms.DockStyle.Left;
            chartControl1.ForeColor = System.Drawing.Color.Black;
            chartControl1.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
            chartControl1.Location = new System.Drawing.Point(0, 0);
            chartControl1.Name = "chartControl1";
            chartControl1.Size = new System.Drawing.Size(305, 201);
            chartControl1.TabIndex = 0;
            chartControl1.Text = "chartControl1";

            return chartControl1;
        }
        ChartControl DisplayDonutChart(string series, string name, List<KeyValuePair<string, int>> values)
        {
            var chartControl1 = new DevComponents.DotNetBar.Charts.ChartControl();
            DevComponents.DotNetBar.Charts.PieChart pieChart1 = new DevComponents.DotNetBar.Charts.PieChart();
            DevComponents.DotNetBar.Charts.PieSeries pieSeries1 = new DevComponents.DotNetBar.Charts.PieSeries();
            DevComponents.DotNetBar.Charts.PieSeriesPoint pieSeriesPoint1 = new DevComponents.DotNetBar.Charts.PieSeriesPoint();
            DevComponents.DotNetBar.Charts.Style.Background background1 = new DevComponents.DotNetBar.Charts.Style.Background();
            DevComponents.DotNetBar.Charts.Style.BorderColor borderColor1 = new DevComponents.DotNetBar.Charts.Style.BorderColor();
            DevComponents.DotNetBar.Charts.Style.Thickness thickness1 = new DevComponents.DotNetBar.Charts.Style.Thickness();
            DevComponents.DotNetBar.Charts.Style.Padding padding1 = new DevComponents.DotNetBar.Charts.Style.Padding();
            DevComponents.DotNetBar.Charts.Style.Background background2 = new DevComponents.DotNetBar.Charts.Style.Background();
            DevComponents.DotNetBar.Charts.Style.BorderColor borderColor2 = new DevComponents.DotNetBar.Charts.Style.BorderColor();
            DevComponents.DotNetBar.Charts.Style.Thickness thickness2 = new DevComponents.DotNetBar.Charts.Style.Thickness();
            DevComponents.DotNetBar.Charts.Style.Padding padding2 = new DevComponents.DotNetBar.Charts.Style.Padding();
            DevComponents.DotNetBar.Charts.Style.Background background3 = new DevComponents.DotNetBar.Charts.Style.Background();
            DevComponents.DotNetBar.Charts.Style.BorderColor borderColor3 = new DevComponents.DotNetBar.Charts.Style.BorderColor();
            DevComponents.DotNetBar.Charts.Style.Thickness thickness3 = new DevComponents.DotNetBar.Charts.Style.Thickness();
            DevComponents.DotNetBar.Charts.Style.Padding padding3 = new DevComponents.DotNetBar.Charts.Style.Padding();
            DevComponents.DotNetBar.Charts.Style.Padding padding4 = new DevComponents.DotNetBar.Charts.Style.Padding();
            DevComponents.DotNetBar.Charts.ChartTitle chartTitle1 = new DevComponents.DotNetBar.Charts.ChartTitle();
            DevComponents.DotNetBar.Charts.Style.Padding padding5 = new DevComponents.DotNetBar.Charts.Style.Padding();
            DevComponents.DotNetBar.Charts.Style.Background background4 = new DevComponents.DotNetBar.Charts.Style.Background();
            DevComponents.DotNetBar.Charts.Style.Background background5 = new DevComponents.DotNetBar.Charts.Style.Background();
            DevComponents.DotNetBar.Charts.Style.Background background6 = new DevComponents.DotNetBar.Charts.Style.Background();
            DevComponents.DotNetBar.Charts.Style.Background background7 = new DevComponents.DotNetBar.Charts.Style.Background();
            DevComponents.DotNetBar.Charts.Style.Background background8 = new DevComponents.DotNetBar.Charts.Style.Background();
            DevComponents.DotNetBar.Charts.Style.Background background9 = new DevComponents.DotNetBar.Charts.Style.Background();
            DevComponents.DotNetBar.Charts.Style.Background background10 = new DevComponents.DotNetBar.Charts.Style.Background();
            DevComponents.DotNetBar.Charts.Style.Background background11 = new DevComponents.DotNetBar.Charts.Style.Background();
            // 
            // chartControl1
            // 
            chartControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(239)))), ((int)(((byte)(242)))));
            pieSeries1.Name = series;

            foreach (var keyValuePair in values)
            {
                DevComponents.DotNetBar.Charts.PieSeriesPoint p = new DevComponents.DotNetBar.Charts.PieSeriesPoint();
                p.ValueX = keyValuePair.Key;
                p.ValueY = new object[] {
                    ((object)(keyValuePair.Value))};

                pieSeries1.SeriesPoints.Add(p);
            }


            pieSeries1.SeriesType = DevComponents.DotNetBar.Charts.SeriesType.Pie;
            pieChart1.ChartSeries.Add(pieSeries1);
            background1.Color1 = System.Drawing.Color.White;
            pieChart1.ChartVisualStyle.Background = background1;
            borderColor1.Bottom = System.Drawing.Color.Black;
            borderColor1.Left = System.Drawing.Color.Black;
            borderColor1.Right = System.Drawing.Color.Black;
            borderColor1.Top = System.Drawing.Color.Black;
            pieChart1.ChartVisualStyle.BorderColor = borderColor1;
            thickness1.Bottom = 1;
            thickness1.Left = 1;
            thickness1.Right = 1;
            thickness1.Top = 1;
            pieChart1.ChartVisualStyle.BorderThickness = thickness1;
            padding1.Bottom = 6;
            padding1.Left = 6;
            padding1.Right = 6;
            padding1.Top = 6;
            pieChart1.ChartVisualStyle.Padding = padding1;
            background2.Color1 = System.Drawing.Color.White;
            pieChart1.ContainerVisualStyles.Default.Background = background2;
            borderColor2.Bottom = System.Drawing.Color.DimGray;
            borderColor2.Left = System.Drawing.Color.DimGray;
            borderColor2.Right = System.Drawing.Color.DimGray;
            borderColor2.Top = System.Drawing.Color.DimGray;
            pieChart1.ContainerVisualStyles.Default.BorderColor = borderColor2;
            thickness2.Bottom = 1;
            thickness2.Left = 1;
            thickness2.Right = 1;
            thickness2.Top = 1;
            pieChart1.ContainerVisualStyles.Default.BorderThickness = thickness2;
            pieChart1.ContainerVisualStyles.Default.DropShadow.Enabled = DevComponents.DotNetBar.Charts.Style.Tbool.True;
            padding2.Bottom = 6;
            padding2.Left = 6;
            padding2.Right = 6;
            padding2.Top = 6;
            pieChart1.ContainerVisualStyles.Default.Padding = padding2;
            pieChart1.InnerRadius = 0.2D;
            pieChart1.IsExploded = true;
            pieChart1.Legend.Alignment = DevComponents.DotNetBar.Charts.Style.Alignment.BottomCenter;
            pieChart1.Legend.AlignVerticalItems = true;
            background3.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            pieChart1.Legend.ChartLegendVisualStyles.Default.Background = background3;
            borderColor3.Bottom = System.Drawing.Color.Black;
            borderColor3.Left = System.Drawing.Color.Black;
            borderColor3.Right = System.Drawing.Color.Black;
            borderColor3.Top = System.Drawing.Color.Black;
            pieChart1.Legend.ChartLegendVisualStyles.Default.BorderColor = borderColor3;
            thickness3.Bottom = 1;
            thickness3.Left = 1;
            thickness3.Right = 1;
            thickness3.Top = 1;
            pieChart1.Legend.ChartLegendVisualStyles.Default.BorderThickness = thickness3;
            padding3.Bottom = 4;
            padding3.Left = 4;
            padding3.Right = 4;
            padding3.Top = 4;
            pieChart1.Legend.ChartLegendVisualStyles.Default.Margin = padding3;
            padding4.Bottom = 4;
            padding4.Left = 4;
            padding4.Right = 4;
            padding4.Top = 4;
            pieChart1.Legend.ChartLegendVisualStyles.Default.Padding = padding4;
            pieChart1.Legend.Direction = DevComponents.DotNetBar.Charts.Direction.LeftToRight;
            pieChart1.Legend.MaxVerticalPct = 90D;
            pieChart1.Legend.Placement = DevComponents.DotNetBar.Charts.Placement.Outside;
            pieChart1.Legend.Visible = true;
            pieChart1.Name = "PieChart1";
            chartTitle1.ChartTitleVisualStyle.Alignment = DevComponents.DotNetBar.Charts.Style.Alignment.MiddleCenter;
            chartTitle1.ChartTitleVisualStyle.Font = new System.Drawing.Font("Georgia", 16F);
            padding5.Bottom = 8;
            padding5.Left = 8;
            padding5.Right = 8;
            padding5.Top = 8;
            chartTitle1.ChartTitleVisualStyle.Padding = padding5;
            chartTitle1.ChartTitleVisualStyle.TextColor = System.Drawing.Color.Navy;
            chartTitle1.Name = "Title1";
            chartTitle1.Text =name;
            chartTitle1.XyAlignment = DevComponents.DotNetBar.Charts.XyAlignment.Top;
            pieChart1.Titles.Add(chartTitle1);
            chartControl1.ChartPanel.ChartContainers.Add(pieChart1);
            chartControl1.ChartPanel.Legend.Visible = false;
            chartControl1.ChartPanel.Name = "PrimaryPanel";
            background4.Color1 = System.Drawing.Color.AliceBlue;
            chartControl1.DefaultVisualStyles.HScrollBarVisualStyles.MouseOver.ArrowBackground = background4;
            background5.Color1 = System.Drawing.Color.AliceBlue;
            chartControl1.DefaultVisualStyles.HScrollBarVisualStyles.MouseOver.ThumbBackground = background5;
            background6.Color1 = System.Drawing.Color.White;
            chartControl1.DefaultVisualStyles.HScrollBarVisualStyles.SelectedMouseOver.ArrowBackground = background6;
            background7.Color1 = System.Drawing.Color.White;
            chartControl1.DefaultVisualStyles.HScrollBarVisualStyles.SelectedMouseOver.ThumbBackground = background7;
            background8.Color1 = System.Drawing.Color.AliceBlue;
            chartControl1.DefaultVisualStyles.VScrollBarVisualStyles.MouseOver.ArrowBackground = background8;
            background9.Color1 = System.Drawing.Color.AliceBlue;
            chartControl1.DefaultVisualStyles.VScrollBarVisualStyles.MouseOver.ThumbBackground = background9;
            background10.Color1 = System.Drawing.Color.White;
            chartControl1.DefaultVisualStyles.VScrollBarVisualStyles.SelectedMouseOver.ArrowBackground = background10;
            background11.Color1 = System.Drawing.Color.White;
            chartControl1.DefaultVisualStyles.VScrollBarVisualStyles.SelectedMouseOver.ThumbBackground = background11;
            chartControl1.ForeColor = System.Drawing.Color.Black;
            chartControl1.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
            chartControl1.Location = new System.Drawing.Point(559, 91);
            chartControl1.Name = "chartControl1";
            chartControl1.Size = new System.Drawing.Size(250, 250);
            chartControl1.TabIndex = 0;
            chartControl1.Dock = DockStyle.Left;
            chartControl1.Text = "chartControl1";

            return chartControl1;
        }

        ChartControl DisplayVarientExtentChart(string series, string name, List<Tuple<string, int, int>> values)
        {
            var chartControl1 = new DevComponents.DotNetBar.Charts.ChartControl();
            DevComponents.DotNetBar.Charts.PieChart pieChart1 = new DevComponents.DotNetBar.Charts.PieChart();
            DevComponents.DotNetBar.Charts.PieSeries pieSeries1 = new DevComponents.DotNetBar.Charts.PieSeries();
   
            DevComponents.DotNetBar.Charts.Style.Background background1 = new DevComponents.DotNetBar.Charts.Style.Background();
            DevComponents.DotNetBar.Charts.Style.BorderColor borderColor1 = new DevComponents.DotNetBar.Charts.Style.BorderColor();
            DevComponents.DotNetBar.Charts.Style.Thickness thickness1 = new DevComponents.DotNetBar.Charts.Style.Thickness();
            DevComponents.DotNetBar.Charts.Style.Padding padding1 = new DevComponents.DotNetBar.Charts.Style.Padding();
            DevComponents.DotNetBar.Charts.Style.Background background2 = new DevComponents.DotNetBar.Charts.Style.Background();
            DevComponents.DotNetBar.Charts.Style.BorderColor borderColor2 = new DevComponents.DotNetBar.Charts.Style.BorderColor();
            DevComponents.DotNetBar.Charts.Style.Thickness thickness2 = new DevComponents.DotNetBar.Charts.Style.Thickness();
            DevComponents.DotNetBar.Charts.Style.Padding padding2 = new DevComponents.DotNetBar.Charts.Style.Padding();
            DevComponents.DotNetBar.Charts.Style.Background background3 = new DevComponents.DotNetBar.Charts.Style.Background();
            DevComponents.DotNetBar.Charts.Style.BorderColor borderColor3 = new DevComponents.DotNetBar.Charts.Style.BorderColor();
            DevComponents.DotNetBar.Charts.Style.Thickness thickness3 = new DevComponents.DotNetBar.Charts.Style.Thickness();
            DevComponents.DotNetBar.Charts.Style.Padding padding3 = new DevComponents.DotNetBar.Charts.Style.Padding();
            DevComponents.DotNetBar.Charts.Style.Padding padding4 = new DevComponents.DotNetBar.Charts.Style.Padding();
            DevComponents.DotNetBar.Charts.ChartTitle chartTitle1 = new DevComponents.DotNetBar.Charts.ChartTitle();
            DevComponents.DotNetBar.Charts.Style.Padding padding5 = new DevComponents.DotNetBar.Charts.Style.Padding();
            DevComponents.DotNetBar.Charts.Style.Background background4 = new DevComponents.DotNetBar.Charts.Style.Background();
            DevComponents.DotNetBar.Charts.Style.Background background5 = new DevComponents.DotNetBar.Charts.Style.Background();
            DevComponents.DotNetBar.Charts.Style.Background background6 = new DevComponents.DotNetBar.Charts.Style.Background();
            DevComponents.DotNetBar.Charts.Style.Background background7 = new DevComponents.DotNetBar.Charts.Style.Background();
            DevComponents.DotNetBar.Charts.Style.Background background8 = new DevComponents.DotNetBar.Charts.Style.Background();
            DevComponents.DotNetBar.Charts.Style.Background background9 = new DevComponents.DotNetBar.Charts.Style.Background();
            DevComponents.DotNetBar.Charts.Style.Background background10 = new DevComponents.DotNetBar.Charts.Style.Background();
            DevComponents.DotNetBar.Charts.Style.Background background11 = new DevComponents.DotNetBar.Charts.Style.Background();
            // 
            // chartControl1
            // 
            chartControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(239)))), ((int)(((byte)(242)))));
            pieSeries1.Name = series;

            foreach (var value in values)
            {
                DevComponents.DotNetBar.Charts.PieSeriesPoint pieSeriesPoint1 = new DevComponents.DotNetBar.Charts.PieSeriesPoint();
                pieSeriesPoint1.ValueX =value.Item1;
                pieSeriesPoint1.ValueY = new object[] {
                    ((object)(value.Item2)),
                    ((object)(value.Item3))};
                pieSeries1.SeriesPoints.Add(pieSeriesPoint1);
            }
        
         
          

            pieSeries1.SeriesType = DevComponents.DotNetBar.Charts.SeriesType.Pie;
            pieChart1.ChartSeries.Add(pieSeries1);
            background1.Color1 = System.Drawing.Color.White;
            pieChart1.ChartVisualStyle.Background = background1;
            borderColor1.Bottom = System.Drawing.Color.Black;
            borderColor1.Left = System.Drawing.Color.Black;
            borderColor1.Right = System.Drawing.Color.Black;
            borderColor1.Top = System.Drawing.Color.Black;
            pieChart1.ChartVisualStyle.BorderColor = borderColor1;
            thickness1.Bottom = 1;
            thickness1.Left = 1;
            thickness1.Right = 1;
            thickness1.Top = 1;
            pieChart1.ChartVisualStyle.BorderThickness = thickness1;
            padding1.Bottom = 6;
            padding1.Left = 6;
            padding1.Right = 6;
            padding1.Top = 6;
            pieChart1.ChartVisualStyle.Padding = padding1;
            background2.Color1 = System.Drawing.Color.White;
            pieChart1.ContainerVisualStyles.Default.Background = background2;
            borderColor2.Bottom = System.Drawing.Color.DimGray;
            borderColor2.Left = System.Drawing.Color.DimGray;
            borderColor2.Right = System.Drawing.Color.DimGray;
            borderColor2.Top = System.Drawing.Color.DimGray;
            pieChart1.ContainerVisualStyles.Default.BorderColor = borderColor2;
            thickness2.Bottom = 1;
            thickness2.Left = 1;
            thickness2.Right = 1;
            thickness2.Top = 1;
            pieChart1.ContainerVisualStyles.Default.BorderThickness = thickness2;
            pieChart1.ContainerVisualStyles.Default.DropShadow.Enabled = DevComponents.DotNetBar.Charts.Style.Tbool.True;
            padding2.Bottom = 6;
            padding2.Left = 6;
            padding2.Right = 6;
            padding2.Top = 6;
            pieChart1.ContainerVisualStyles.Default.Padding = padding2;
            pieChart1.IsExploded = true;
            pieChart1.Legend.Alignment = DevComponents.DotNetBar.Charts.Style.Alignment.BottomCenter;
            pieChart1.Legend.AlignVerticalItems = true;
            background3.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            pieChart1.Legend.ChartLegendVisualStyles.Default.Background = background3;
            borderColor3.Bottom = System.Drawing.Color.Black;
            borderColor3.Left = System.Drawing.Color.Black;
            borderColor3.Right = System.Drawing.Color.Black;
            borderColor3.Top = System.Drawing.Color.Black;
            pieChart1.Legend.ChartLegendVisualStyles.Default.BorderColor = borderColor3;
            thickness3.Bottom = 1;
            thickness3.Left = 1;
            thickness3.Right = 1;
            thickness3.Top = 1;
            pieChart1.Legend.ChartLegendVisualStyles.Default.BorderThickness = thickness3;
            padding3.Bottom = 4;
            padding3.Left = 4;
            padding3.Right = 4;
            padding3.Top = 4;
            pieChart1.Legend.ChartLegendVisualStyles.Default.Margin = padding3;
            padding4.Bottom = 4;
            padding4.Left = 4;
            padding4.Right = 4;
            padding4.Top = 4;
            pieChart1.Legend.ChartLegendVisualStyles.Default.Padding = padding4;
            pieChart1.Legend.Direction = DevComponents.DotNetBar.Charts.Direction.LeftToRight;
            pieChart1.Legend.MaxVerticalPct = 90D;
            pieChart1.Legend.Placement = DevComponents.DotNetBar.Charts.Placement.Outside;
            pieChart1.Legend.Visible = true;
            pieChart1.Name = "PieChart1";
            chartTitle1.ChartTitleVisualStyle.Alignment = DevComponents.DotNetBar.Charts.Style.Alignment.MiddleCenter;
            chartTitle1.ChartTitleVisualStyle.Font = new System.Drawing.Font("Georgia", 16F);
            padding5.Bottom = 8;
            padding5.Left = 8;
            padding5.Right = 8;
            padding5.Top = 8;
            chartTitle1.ChartTitleVisualStyle.Padding = padding5;
            chartTitle1.ChartTitleVisualStyle.TextColor = System.Drawing.Color.Navy;
            chartTitle1.Name = "Title1";
            chartTitle1.Text = name;
            chartTitle1.XyAlignment = DevComponents.DotNetBar.Charts.XyAlignment.Top;
            pieChart1.Titles.Add(chartTitle1);
            chartControl1.ChartPanel.ChartContainers.Add(pieChart1);
            chartControl1.ChartPanel.Legend.Visible = false;
            chartControl1.ChartPanel.Name = "PrimaryPanel";
            background4.Color1 = System.Drawing.Color.AliceBlue;
            chartControl1.DefaultVisualStyles.HScrollBarVisualStyles.MouseOver.ArrowBackground = background4;
            background5.Color1 = System.Drawing.Color.AliceBlue;
            chartControl1.DefaultVisualStyles.HScrollBarVisualStyles.MouseOver.ThumbBackground = background5;
            background6.Color1 = System.Drawing.Color.White;
            chartControl1.DefaultVisualStyles.HScrollBarVisualStyles.SelectedMouseOver.ArrowBackground = background6;
            background7.Color1 = System.Drawing.Color.White;
            chartControl1.DefaultVisualStyles.HScrollBarVisualStyles.SelectedMouseOver.ThumbBackground = background7;
            background8.Color1 = System.Drawing.Color.AliceBlue;
            chartControl1.DefaultVisualStyles.VScrollBarVisualStyles.MouseOver.ArrowBackground = background8;
            background9.Color1 = System.Drawing.Color.AliceBlue;
            chartControl1.DefaultVisualStyles.VScrollBarVisualStyles.MouseOver.ThumbBackground = background9;
            background10.Color1 = System.Drawing.Color.White;
            chartControl1.DefaultVisualStyles.VScrollBarVisualStyles.SelectedMouseOver.ArrowBackground = background10;
            background11.Color1 = System.Drawing.Color.White;
            chartControl1.DefaultVisualStyles.VScrollBarVisualStyles.SelectedMouseOver.ThumbBackground = background11;
            chartControl1.Dock = System.Windows.Forms.DockStyle.Left;
            chartControl1.ForeColor = System.Drawing.Color.Black;
            chartControl1.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
            chartControl1.Location = new System.Drawing.Point(0, 0);
            chartControl1.Name = "chartControl1";
            chartControl1.Size = new System.Drawing.Size(305, 342);
            chartControl1.TabIndex = 0;
            chartControl1.Text = "chartControl1";

            return chartControl1;
        }
    

        private void panel1_SizeChanged(object sender, EventArgs e)
        {
            foreach (var c in panel1.Controls)
            {
                var control = c as Control;
                control.Width = panel1.Width / 3;
            }
            
        }
    }

}
