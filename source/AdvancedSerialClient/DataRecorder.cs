using AdvancedSerial;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdvancedSerialClient
{
    public partial class DataRecorder : Form
    {
        private Stopwatch SW = new Stopwatch();
        private Double RdgStartTime = 0;
        private Double RdgEndTime = 0;
        private bool isRunning = false;
        private RealTimeDB DB;
        System.Threading.Timer updateTimer;

        public DataRecorder(RealTimeDB DataBase)
        {
            InitializeComponent();
            SW.Reset();
            DB = DataBase;

            this.MaximumSize = new Size(this.Width, this.Height);

            AutoResetEvent autoEvent = new AutoResetEvent(false);
            TimerCallback tcb = Update;
            updateTimer = new System.Threading.Timer(tcb, autoEvent, 1000, 1000);
        }

        private void Update(Object stateInfo)
        {
            if (isRunning)
            {
                if (InvokeRequired)
                {
                    this.Invoke(new Action<string>(Update), new object[] { SW.Elapsed.ToString(@"hh\:mm\:ss") });
                    return;
                }
                else
                {
                    setClockText(SW.Elapsed.ToString(@"hh\:mm\:ss"));
                }
            }
        }

        private void setClockText(string value)
        {
            lbl_time.Text = value;
        }

        private void BTN_RECORD_Click(object sender, EventArgs e)
        {
            if (isRunning)
            {
                //Stop the Running and Save
                RdgEndTime = Convert.ToDouble(DB.GetTimeMs()) / 1000;
                SW.Stop();
                lbl_status.Text = "Stopped...";
                BTN_RECORD.Text = "Record";
                isRunning = false;
                cmb_sampleHz.Enabled = true;
                SW.Reset();

                //Ask User to Store the Data
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();

                saveFileDialog1.Filter = "csv files (*.csv)|*.csv";
                saveFileDialog1.FilterIndex = 2;
                saveFileDialog1.RestoreDirectory = true;

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    Single sampleHz = Single.Parse(cmb_sampleHz.Text);
                    Single sampleRt = 1 / sampleHz;
                    bool result = DB.storeCSV(saveFileDialog1.FileName, Convert.ToSingle(RdgStartTime), Convert.ToSingle(RdgEndTime), sampleRt);
                    if (!result)
                    {
                        MessageBox.Show("Error Writing Output. Data May be Corrupted");
                    }
                }
            }
            else
            {
                RdgStartTime = Convert.ToDouble(DB.GetTimeMs()) / 1000;
                SW.Start();
                lbl_status.Text = "Running...";
                BTN_RECORD.Text = "Stop and Save";
                cmb_sampleHz.Enabled = false;
                isRunning = true;
            }
        }

        private void cmb_sampleHz_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
