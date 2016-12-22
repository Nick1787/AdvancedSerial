namespace AdvancedSerial
{
    partial class MAIN
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
            this.btn_connect = new System.Windows.Forms.Button();
            this.cmb_ports = new System.Windows.Forms.ComboBox();
            this.cmb_baud = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_status = new System.Windows.Forms.TextBox();
            this.grp_connect = new System.Windows.Forms.GroupBox();
            this.btn_reset = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newPlotterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restartDBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.requestSymbolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.oxyPlotterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tb_send = new System.Windows.Forms.TextBox();
            this.btn_send = new System.Windows.Forms.Button();
            this.rtb_serialdata = new System.Windows.Forms.RichTextBox();
            this.cmb_eol = new System.Windows.Forms.ComboBox();
            this.lbl_eol = new System.Windows.Forms.Label();
            this.cb_autoscroll = new System.Windows.Forms.CheckBox();
            this.cbk_FilterASI = new System.Windows.Forms.CheckBox();
            this.dataRecorderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.grp_connect.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_connect
            // 
            this.btn_connect.Location = new System.Drawing.Point(398, 25);
            this.btn_connect.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btn_connect.Name = "btn_connect";
            this.btn_connect.Size = new System.Drawing.Size(112, 35);
            this.btn_connect.TabIndex = 0;
            this.btn_connect.Text = "Connect";
            this.btn_connect.UseVisualStyleBackColor = true;
            this.btn_connect.Click += new System.EventHandler(this.btn_connect_Click);
            // 
            // cmb_ports
            // 
            this.cmb_ports.FormattingEnabled = true;
            this.cmb_ports.Location = new System.Drawing.Point(76, 26);
            this.cmb_ports.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmb_ports.Name = "cmb_ports";
            this.cmb_ports.Size = new System.Drawing.Size(115, 28);
            this.cmb_ports.TabIndex = 1;
            this.cmb_ports.DropDown += new System.EventHandler(this.cmb_ports_DropDown);
            this.cmb_ports.SelectedIndexChanged += new System.EventHandler(this.cmb_ports_SelectedIndexChanged);
            // 
            // cmb_baud
            // 
            this.cmb_baud.FormattingEnabled = true;
            this.cmb_baud.Location = new System.Drawing.Point(267, 26);
            this.cmb_baud.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmb_baud.Name = "cmb_baud";
            this.cmb_baud.Size = new System.Drawing.Size(115, 28);
            this.cmb_baud.TabIndex = 2;
            this.cmb_baud.SelectedIndexChanged += new System.EventHandler(this.cmb_baud_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 31);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "COM:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(202, 34);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "BAUD:";
            // 
            // tb_status
            // 
            this.tb_status.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_status.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.tb_status.Location = new System.Drawing.Point(2, 440);
            this.tb_status.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tb_status.Name = "tb_status";
            this.tb_status.ReadOnly = true;
            this.tb_status.Size = new System.Drawing.Size(774, 26);
            this.tb_status.TabIndex = 5;
            this.tb_status.Text = "Not Connected";
            // 
            // grp_connect
            // 
            this.grp_connect.Controls.Add(this.btn_reset);
            this.grp_connect.Controls.Add(this.cmb_ports);
            this.grp_connect.Controls.Add(this.cmb_baud);
            this.grp_connect.Controls.Add(this.label1);
            this.grp_connect.Controls.Add(this.btn_connect);
            this.grp_connect.Controls.Add(this.label2);
            this.grp_connect.Location = new System.Drawing.Point(2, 42);
            this.grp_connect.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.grp_connect.Name = "grp_connect";
            this.grp_connect.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.grp_connect.Size = new System.Drawing.Size(758, 74);
            this.grp_connect.TabIndex = 8;
            this.grp_connect.TabStop = false;
            this.grp_connect.Text = "Connect";
            // 
            // btn_reset
            // 
            this.btn_reset.Enabled = false;
            this.btn_reset.Location = new System.Drawing.Point(606, 23);
            this.btn_reset.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btn_reset.Name = "btn_reset";
            this.btn_reset.Size = new System.Drawing.Size(142, 35);
            this.btn_reset.TabIndex = 5;
            this.btn_reset.Text = "Reset Arduino";
            this.btn_reset.UseVisualStyleBackColor = true;
            this.btn_reset.Click += new System.EventHandler(this.btn_reset_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(9, 3, 0, 3);
            this.menuStrip1.Size = new System.Drawing.Size(777, 35);
            this.menuStrip1.TabIndex = 9;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(50, 29);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(124, 30);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newPlotterToolStripMenuItem,
            this.restartDBToolStripMenuItem,
            this.requestSymbolsToolStripMenuItem,
            this.dataRecorderToolStripMenuItem,
            this.oxyPlotterToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(65, 29);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // newPlotterToolStripMenuItem
            // 
            this.newPlotterToolStripMenuItem.Name = "newPlotterToolStripMenuItem";
            this.newPlotterToolStripMenuItem.Size = new System.Drawing.Size(233, 30);
            this.newPlotterToolStripMenuItem.Text = "New Plotter";
            this.newPlotterToolStripMenuItem.Visible = false;
            this.newPlotterToolStripMenuItem.Click += new System.EventHandler(this.newPlotterToolStripMenuItem_Click);
            // 
            // restartDBToolStripMenuItem
            // 
            this.restartDBToolStripMenuItem.Name = "restartDBToolStripMenuItem";
            this.restartDBToolStripMenuItem.Size = new System.Drawing.Size(233, 30);
            this.restartDBToolStripMenuItem.Text = "RestartDB";
            this.restartDBToolStripMenuItem.Click += new System.EventHandler(this.restartDBToolStripMenuItem_Click);
            // 
            // requestSymbolsToolStripMenuItem
            // 
            this.requestSymbolsToolStripMenuItem.Name = "requestSymbolsToolStripMenuItem";
            this.requestSymbolsToolStripMenuItem.Size = new System.Drawing.Size(233, 30);
            this.requestSymbolsToolStripMenuItem.Text = "Request Symbols";
            this.requestSymbolsToolStripMenuItem.Click += new System.EventHandler(this.requestSymbolsToolStripMenuItem_Click);
            // 
            // oxyPlotterToolStripMenuItem
            // 
            this.oxyPlotterToolStripMenuItem.Name = "oxyPlotterToolStripMenuItem";
            this.oxyPlotterToolStripMenuItem.Size = new System.Drawing.Size(233, 30);
            this.oxyPlotterToolStripMenuItem.Text = "DataPlotter";
            this.oxyPlotterToolStripMenuItem.Click += new System.EventHandler(this.oxyPlotterToolStripMenuItem_Click);
            // 
            // tb_send
            // 
            this.tb_send.Location = new System.Drawing.Point(4, 126);
            this.tb_send.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tb_send.Name = "tb_send";
            this.tb_send.Size = new System.Drawing.Size(646, 26);
            this.tb_send.TabIndex = 10;
            // 
            // btn_send
            // 
            this.btn_send.Enabled = false;
            this.btn_send.Location = new System.Drawing.Point(657, 123);
            this.btn_send.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btn_send.Name = "btn_send";
            this.btn_send.Size = new System.Drawing.Size(112, 35);
            this.btn_send.TabIndex = 11;
            this.btn_send.Text = "Send";
            this.btn_send.UseVisualStyleBackColor = true;
            this.btn_send.Click += new System.EventHandler(this.btn_send_Click);
            // 
            // rtb_serialdata
            // 
            this.rtb_serialdata.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtb_serialdata.Location = new System.Drawing.Point(4, 166);
            this.rtb_serialdata.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rtb_serialdata.Name = "rtb_serialdata";
            this.rtb_serialdata.Size = new System.Drawing.Size(763, 221);
            this.rtb_serialdata.TabIndex = 12;
            this.rtb_serialdata.Text = "";
            // 
            // cmb_eol
            // 
            this.cmb_eol.FormattingEnabled = true;
            this.cmb_eol.Location = new System.Drawing.Point(608, 398);
            this.cmb_eol.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmb_eol.Name = "cmb_eol";
            this.cmb_eol.Size = new System.Drawing.Size(160, 28);
            this.cmb_eol.TabIndex = 5;
            // 
            // lbl_eol
            // 
            this.lbl_eol.AutoSize = true;
            this.lbl_eol.Location = new System.Drawing.Point(416, 406);
            this.lbl_eol.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_eol.Name = "lbl_eol";
            this.lbl_eol.Size = new System.Drawing.Size(184, 20);
            this.lbl_eol.TabIndex = 6;
            this.lbl_eol.Text = "End of Line Characcters:";
            // 
            // cb_autoscroll
            // 
            this.cb_autoscroll.AutoSize = true;
            this.cb_autoscroll.Checked = true;
            this.cb_autoscroll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_autoscroll.Location = new System.Drawing.Point(8, 403);
            this.cb_autoscroll.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cb_autoscroll.Name = "cb_autoscroll";
            this.cb_autoscroll.Size = new System.Drawing.Size(105, 24);
            this.cb_autoscroll.TabIndex = 13;
            this.cb_autoscroll.Text = "Autoscroll";
            this.cb_autoscroll.UseVisualStyleBackColor = true;
            this.cb_autoscroll.CheckedChanged += new System.EventHandler(this.cb_autoscroll_CheckedChanged);
            // 
            // cbk_FilterASI
            // 
            this.cbk_FilterASI.AutoSize = true;
            this.cbk_FilterASI.Checked = true;
            this.cbk_FilterASI.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbk_FilterASI.Location = new System.Drawing.Point(124, 403);
            this.cbk_FilterASI.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbk_FilterASI.Name = "cbk_FilterASI";
            this.cbk_FilterASI.Size = new System.Drawing.Size(262, 24);
            this.cbk_FilterASI.TabIndex = 14;
            this.cbk_FilterASI.Text = "Filter AdvancedSerial Messages";
            this.cbk_FilterASI.UseVisualStyleBackColor = true;
            // 
            // dataRecorderToolStripMenuItem
            // 
            this.dataRecorderToolStripMenuItem.Name = "dataRecorderToolStripMenuItem";
            this.dataRecorderToolStripMenuItem.Size = new System.Drawing.Size(233, 30);
            this.dataRecorderToolStripMenuItem.Text = "DataRecorder";
            this.dataRecorderToolStripMenuItem.Click += new System.EventHandler(this.dataRecorderToolStripMenuItem_Click);
            // 
            // MAIN
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(777, 472);
            this.Controls.Add(this.cbk_FilterASI);
            this.Controls.Add(this.cb_autoscroll);
            this.Controls.Add(this.lbl_eol);
            this.Controls.Add(this.rtb_serialdata);
            this.Controls.Add(this.cmb_eol);
            this.Controls.Add(this.btn_send);
            this.Controls.Add(this.tb_send);
            this.Controls.Add(this.grp_connect);
            this.Controls.Add(this.tb_status);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "MAIN";
            this.Text = "AdvancedSerialClient";
            this.grp_connect.ResumeLayout(false);
            this.grp_connect.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_connect;
        private System.Windows.Forms.ComboBox cmb_ports;
        private System.Windows.Forms.ComboBox cmb_baud;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_status;
        private System.Windows.Forms.GroupBox grp_connect;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newPlotterToolStripMenuItem;
        private System.Windows.Forms.TextBox tb_send;
        private System.Windows.Forms.Button btn_send;
        private System.Windows.Forms.RichTextBox rtb_serialdata;
        private System.Windows.Forms.ComboBox cmb_eol;
        private System.Windows.Forms.Label lbl_eol;
        private System.Windows.Forms.CheckBox cb_autoscroll;
        private System.Windows.Forms.Button btn_reset;
        private System.Windows.Forms.CheckBox cbk_FilterASI;
        private System.Windows.Forms.ToolStripMenuItem requestSymbolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem restartDBToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem oxyPlotterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dataRecorderToolStripMenuItem;
    }
}

