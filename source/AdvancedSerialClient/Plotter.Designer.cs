namespace AdvancedSerial
{
    partial class Plotter
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Plotter));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.TB_XAxisTime = new System.Windows.Forms.ToolStripTextBox();
            this.Splitter = new System.Windows.Forms.SplitContainer();
            this.Chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.dgvplots = new System.Windows.Forms.DataGridView();
            this.Visible = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Signal = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.btn_ShowNames = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Splitter)).BeginInit();
            this.Splitter.Panel1.SuspendLayout();
            this.Splitter.Panel2.SuspendLayout();
            this.Splitter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Chart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvplots)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.TB_XAxisTime,
            this.btn_ShowNames});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(650, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(142, 22);
            this.toolStripLabel1.Text = "Time-Length (*=all time):";
            // 
            // TB_XAxisTime
            // 
            this.TB_XAxisTime.Name = "TB_XAxisTime";
            this.TB_XAxisTime.Size = new System.Drawing.Size(50, 25);
            this.TB_XAxisTime.Text = "*";
            this.TB_XAxisTime.Validating += new System.ComponentModel.CancelEventHandler(this.TB_XAxisTime_Validating);
            this.TB_XAxisTime.Click += new System.EventHandler(this.TB_XAxisTime_Click);
            // 
            // Splitter
            // 
            this.Splitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Splitter.Location = new System.Drawing.Point(0, 25);
            this.Splitter.Name = "Splitter";
            this.Splitter.MouseUp += new System.Windows.Forms.MouseEventHandler(SplitterMouseUp);
            // 
            // Splitter.Panel1
            // 
            this.Splitter.Panel1.Controls.Add(this.Chart);
            // 
            // Splitter.Panel2
            // 
            this.Splitter.Panel2.Controls.Add(this.dgvplots);
            this.Splitter.Size = new System.Drawing.Size(650, 307);
            this.Splitter.SplitterDistance = 452;
            this.Splitter.TabIndex = 1;
            // 
            // Chart
            // 
            chartArea1.AxisX.Interval = 10D;
            chartArea1.AxisX.ScaleView.SmallScrollMinSize = 10D;
            chartArea1.AxisX.ScaleView.SmallScrollSize = 10D;
            chartArea1.AxisX.ScaleView.SmallScrollSizeType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea1.BackColor = System.Drawing.Color.White;
            chartArea1.Name = "ChartArea1";
            this.Chart.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.Chart.Legends.Add(legend1);
            this.Chart.Location = new System.Drawing.Point(0, 7);
            this.Chart.Name = "Chart";
            this.Chart.Size = new System.Drawing.Size(453, 300);
            this.Chart.TabIndex = 0;
            this.Chart.Text = "chart1";
            // 
            // dgvplots
            // 
            this.dgvplots.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvplots.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Visible,
            this.Signal});
            this.dgvplots.Location = new System.Drawing.Point(-1, 4);
            this.dgvplots.Name = "dgvplots";
            this.dgvplots.RowHeadersVisible = false;
            this.dgvplots.Size = new System.Drawing.Size(192, 291);
            this.dgvplots.TabIndex = 0;

            this.dgvplots.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvplots_CellContentClick);
            //this.dgvplots.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvplots_CellContentClick);
            // 
            // Visible
            // 
            this.Visible.FalseValue = "";
            this.Visible.HeaderText = "Visible";
            this.Visible.Name = "Visible";
            this.Visible.TrueValue = "";
            this.Visible.Width = 50;
            // 
            // Signal
            // 
            this.Signal.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.Signal.HeaderText = "Name";
            this.Signal.Name = "Signal";
            // 
            // btn_ShowNames
            // 
            this.btn_ShowNames.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btn_ShowNames.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btn_ShowNames.Image = ((System.Drawing.Image)(resources.GetObject("btn_ShowNames.Image")));
            this.btn_ShowNames.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_ShowNames.Name = "btn_ShowNames";
            this.btn_ShowNames.Size = new System.Drawing.Size(23, 22);
            this.btn_ShowNames.Text = ">";
            this.btn_ShowNames.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // Plotter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(650, 332);
            this.Controls.Add(this.Splitter);
            this.Controls.Add(this.toolStrip1);
            this.Name = "Plotter";
            this.Text = "Plotter";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.Splitter.Panel1.ResumeLayout(false);
            this.Splitter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Splitter)).EndInit();
            this.Splitter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Chart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvplots)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.SplitContainer Splitter;
        private System.Windows.Forms.DataGridView dgvplots;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Visible;
        private System.Windows.Forms.DataGridViewComboBoxColumn Signal;
        private System.Windows.Forms.DataVisualization.Charting.Chart Chart;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox TB_XAxisTime;
        private System.Windows.Forms.ToolStripButton btn_ShowNames;
    }
}