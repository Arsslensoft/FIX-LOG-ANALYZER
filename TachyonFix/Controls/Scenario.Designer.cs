namespace TachyonFix.Controls
{
    partial class Scenario
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.start = new DevComponents.Editors.IntegerInput();
            this.end = new DevComponents.Editors.IntegerInput();
            this.goView1 = new Northwoods.Go.GoView();
            this.buttonX1 = new DevComponents.DotNetBar.ButtonX();
            this.integerInput1 = new DevComponents.Editors.IntegerInput();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.start)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.end)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.integerInput1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.integerInput1);
            this.panel1.Controls.Add(this.labelX3);
            this.panel1.Controls.Add(this.buttonX1);
            this.panel1.Controls.Add(this.end);
            this.panel1.Controls.Add(this.start);
            this.panel1.Controls.Add(this.labelX2);
            this.panel1.Controls.Add(this.labelX1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1221, 76);
            this.panel1.TabIndex = 0;
            // 
            // labelX1
            // 
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(18, 24);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(75, 23);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "First Event";
            // 
            // labelX2
            // 
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Location = new System.Drawing.Point(380, 27);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(75, 23);
            this.labelX2.TabIndex = 1;
            this.labelX2.Text = "Last Event";
            // 
            // start
            // 
            // 
            // 
            // 
            this.start.BackgroundStyle.Class = "DateTimeInputBackground";
            this.start.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.start.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.start.Location = new System.Drawing.Point(99, 27);
            this.start.Name = "start";
            this.start.ShowUpDown = true;
            this.start.Size = new System.Drawing.Size(259, 20);
            this.start.TabIndex = 2;
            // 
            // end
            // 
            // 
            // 
            // 
            this.end.BackgroundStyle.Class = "DateTimeInputBackground";
            this.end.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.end.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.end.Location = new System.Drawing.Point(461, 27);
            this.end.Name = "end";
            this.end.ShowUpDown = true;
            this.end.Size = new System.Drawing.Size(259, 20);
            this.end.TabIndex = 3;
            // 
            // goView1
            // 
            this.goView1.ArrowMoveLarge = 10F;
            this.goView1.ArrowMoveSmall = 1F;
            this.goView1.BackColor = System.Drawing.Color.White;
            this.goView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.goView1.DragsRealtime = true;
            this.goView1.GridCellSizeHeight = 15F;
            this.goView1.GridOriginY = 75F;
            this.goView1.GridStyle = Northwoods.Go.GoViewGridStyle.HorizontalLine;
            this.goView1.GridUnboundedSpots = 24;
            this.goView1.Location = new System.Drawing.Point(0, 76);
            this.goView1.Name = "goView1";
            this.goView1.Size = new System.Drawing.Size(1221, 493);
            this.goView1.TabIndex = 1;
            this.goView1.Text = "goView1";
            // 
            // buttonX1
            // 
            this.buttonX1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX1.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX1.Location = new System.Drawing.Point(1090, 27);
            this.buttonX1.Name = "buttonX1";
            this.buttonX1.Size = new System.Drawing.Size(75, 23);
            this.buttonX1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonX1.TabIndex = 4;
            this.buttonX1.Text = "Visualize";
            this.buttonX1.Click += new System.EventHandler(this.buttonX1_Click);
            // 
            // integerInput1
            // 
            // 
            // 
            // 
            this.integerInput1.BackgroundStyle.Class = "DateTimeInputBackground";
            this.integerInput1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.integerInput1.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.integerInput1.Location = new System.Drawing.Point(902, 27);
            this.integerInput1.MinValue = 0;
            this.integerInput1.Name = "integerInput1";
            this.integerInput1.ShowUpDown = true;
            this.integerInput1.Size = new System.Drawing.Size(79, 20);
            this.integerInput1.TabIndex = 6;
            this.integerInput1.Value = 300;
            // 
            // labelX3
            // 
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.Location = new System.Drawing.Point(821, 24);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(75, 23);
            this.labelX3.TabIndex = 5;
            this.labelX3.Text = "Spacing";
            // 
            // Scenario
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.goView1);
            this.Controls.Add(this.panel1);
            this.Name = "Scenario";
            this.Size = new System.Drawing.Size(1221, 569);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.start)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.end)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.integerInput1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.Editors.IntegerInput start;
        private DevComponents.Editors.IntegerInput end;
        private Northwoods.Go.GoView goView1;
        private DevComponents.DotNetBar.ButtonX buttonX1;
        private DevComponents.Editors.IntegerInput integerInput1;
        private DevComponents.DotNetBar.LabelX labelX3;
    }
}
