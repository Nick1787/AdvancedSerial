using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace AdvancedSerial
{
    public partial class MAIN : Form
    {
        //Main Databse
        private RealTimeDB DB;
        private AdvancedSerial_API ASI;

        //Buffers for REading Serial Data
        List<List<byte>> ASIData = new List<List<byte>>();
        List<String> TextData = new List<String>();
        List<byte> CurrentLine = new List<byte>();

        //For Tests Signals
        Stopwatch sw;
        System.Threading.Timer updateTimer;
        Dictionary<String, List<Tuple<Single, Single>>> Buffer = new Dictionary<String, List<Tuple<Single, Single>>>();
        List<Single> cos = new List<Single>();
        List<Single> sin = new List<Single>();
   
        //Keep TRack of the Open PLotting Windows
        List<Plotter> OpenPlotters = new List<Plotter>();

        //Serial Port Communication Stuff
        SerialPort ComPort = new SerialPort();
        enum EnumComPortStatus { NotConnected, Connected, Disconnected, ConnectionFailed }
        EnumComPortStatus ComPortStatus;
        Dictionary<String, String> EOLChars = new Dictionary<String, String>();

        //Form Stuff
        bool Autoscroll = true;

        public MAIN()
        {
            InitializeComponent();
            this.Resize += new EventHandler(FrmResize);
            cmb_baud.Items.Add(300);
            cmb_baud.Items.Add(600);
            cmb_baud.Items.Add(1200);
            cmb_baud.Items.Add(2400);
            cmb_baud.Items.Add(4800);
            cmb_baud.Items.Add(9600);
            cmb_baud.Items.Add(14400);
            cmb_baud.Items.Add(19200);
            cmb_baud.Items.Add(28800);
            cmb_baud.Items.Add(38400);
            cmb_baud.Items.Add(57600);
            cmb_baud.Items.Add(115200);

            ComPort.DataReceived+=new SerialDataReceivedEventHandler(ComPort_DataReceived);
            ComPortStatus = EnumComPortStatus.NotConnected;
            btn_connect.Enabled = false;

            EOLChars.Add("None","");
            EOLChars.Add("Newline","\n");
            EOLChars.Add("Carraige Regurn","\r");
            EOLChars.Add("Both NL and CR","\n\r");

            cmb_eol.DataSource = new BindingSource(EOLChars, null);
            cmb_eol.DisplayMember = "Key";
            cmb_eol.ValueMember = "Value";

            DB = new RealTimeDB();
            ASI = new AdvancedSerial_API(DB);

            //Initialized the Buffer
            Buffer.Add("TEST_sin", new List<Tuple<Single,Single>>());
            Buffer.Add("TEST_cos", new List<Tuple<Single,Single>>());

            //Create a Timer to update the test data
            sw = new Stopwatch();
            TimerCallback tcb = UpdateTestData;
            updateTimer = new System.Threading.Timer(tcb, sw, 0, 100);
            sw.Start();
        }

        //
        private void UpdateTestData(Object Watch)
        {
            Double Elapsed = DB.GetTimeMs();
            Double Angle = (Double)(2 * 3.1415 * 0.1 * Elapsed/1000);

            Single TimeSec = (Single)(Elapsed / 1000);
            Single SineValue = (Single)Math.Sin(Angle);
            Single CosineVAlue = (Single)Math.Cos(Angle);

            int lck = DB.requestLock();
            if (lck >= 0)
            {
                //Write buffer Data
                if (Buffer[Buffer.Keys.First()].Count > 0)
                {
                    foreach (String key in Buffer.Keys)
                    {
                        DB.Write(key, Buffer[key]);
                        Buffer[key].Clear();
                    }
                }

                //Now write new data
                DB.Write("TEST_sin", TimeSec, SineValue);
                DB.Write("TEST_cos", TimeSec, CosineVAlue);
                DB.returnLock(lck);
            }
            else
            {
                //Write data to buffer couldnt get a lock
                Buffer["TEST_sin"].Add(new Tuple<Single, Single>(TimeSec, SineValue));
                Buffer["TEST_cos"].Add(new Tuple<Single, Single>(TimeSec, CosineVAlue));

            }
        }

        private void FrmResize(object sender, EventArgs e)
        {
            this.grp_connect.Width = this.Size.Width - 25;
            this.rtb_serialdata.Width = this.Size.Width - 25;
            this.tb_send.Width = this.Size.Width - btn_send.Size.Width - 30;
            this.tb_send.Height = tb_status.Top - grp_connect.Bottom - 30;
            this.btn_send.Location = new System.Drawing.Point(tb_send.Right + 5, tb_send.Top - 2);

            cb_autoscroll.Location = new System.Drawing.Point(tb_status.Left + 5, tb_status.Top - cb_autoscroll.Height - 5);
            cmb_eol.Location = new System.Drawing.Point(this.Width - 25 - cmb_eol.Width, cb_autoscroll.Top-2);
            lbl_eol.Location = new System.Drawing.Point(cmb_eol.Left - 5 - lbl_eol.Width, cmb_eol.Top + 2);
            this.rtb_serialdata.Height = cb_autoscroll.Top - tb_send.Bottom - 10;
    
        }

        private void cmb_ports_DropDown(object sender, EventArgs e) 
        {
            cmb_ports.Items.Clear();
            string[] ports = SerialPort.GetPortNames();
            cmb_ports.Items.AddRange(ports);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void newPlotterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Plotter Plt = new Plotter(DB);
            OpenPlotters.Add(Plt);
            Plt.Show();
        }

        private void btn_connect_Click(object sender, EventArgs e)
        {
            if (ComPortStatus == EnumComPortStatus.Connected)
            {
                ComPort.Close();
                tb_status.Text = "Disconnected.";
                cmb_baud.Enabled = true;
                cmb_ports.Enabled = true;
                btn_connect.Text = "Connect";
                btn_send.Enabled = false;
                btn_reset.Enabled = false;
                ComPortStatus = EnumComPortStatus.Disconnected;
            }
            else
            {
                ComPort.PortName = cmb_ports.Text;
                ComPort.BaudRate = Convert.ToInt32(cmb_baud.Text);
                ComPort.DataBits = 8;
                ComPort.StopBits = StopBits.One;
                
                tb_status.Text = "Connecting...";
                try
                {
                    ComPort.Open();
                    ComPortStatus = EnumComPortStatus.Connected;
                    tb_status.Text = "Connected";
                    cmb_baud.Enabled = false;
                    cmb_ports.Enabled = false;
                    btn_send.Enabled = true;
                    btn_connect.Text = "Disconnect";
                    btn_reset.Enabled = true;
                    rtb_serialdata.Clear();

                    //Get Symbols
                    ASI.getSymbols(ComPort);
                }
                catch
                {
                    ComPortStatus = EnumComPortStatus.ConnectionFailed;
                    tb_status.Text = "Connection Failed";
                }
            }

            
        }

        public void SetControlText(Control control, string text)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<Control, string>(SetControlText), new object[] { control, text });
            }
            else
            {
                control.Text = text;
            }
        }

        private void ComPort_DataReceived(object sender,SerialDataReceivedEventArgs e)
        {
            while (ComPort.BytesToRead > 0)
            {
                //Add Byte read to idata array
                byte inByte = (byte)ComPort.ReadByte();
                CurrentLine.Add(inByte);

                //See If we reached an End of Line (Carriage Return + New Line), then see if its ASI data or Test Data
                if (CurrentLine.Count() > 2){
                    if ((CurrentLine[CurrentLine.Count-1] == (byte)'\n') && (CurrentLine[CurrentLine.Count - 2] == (byte)'\r'))
                    {
                        bool isASIData = false;

                        //Reached End of Line, is it ASI data?
                        if (CurrentLine.Count() > 5)
                        {
                            if ((CurrentLine[0] == (byte)'#') && (CurrentLine[1] == (byte)'A') && (CurrentLine[2] == (byte)'S') && (CurrentLine[3] == (byte)'I') && (CurrentLine[4] == (byte)':'))
                            {
                                isASIData = true;
                            }
                        }

                        //Process the Data
                        if(isASIData){
                            ASIData.Add(CurrentLine);

                            //If not filtering out ASI messages then add to the text box.
                            if (!(cbk_FilterASI.Checked))
                            {
                                rtb_serialdata.Invoke(new Action(() =>
                                {
                                    //Convert Data to a String
                                    ASCIIEncoding encoding = new ASCIIEncoding();
                                    string StrData = encoding.GetString(CurrentLine.ToArray());

                                    //Add it to the Text Box
                                    rtb_serialdata.AppendText(StrData);
                                    if (cb_autoscroll.Checked)
                                    {
                                        rtb_serialdata.ScrollToCaret();
                                    }
                                }));
                            }

                            //Process the ASI data
                            ASI.ProcessMessage(CurrentLine);

                            //Clear Current Line Data
                            CurrentLine.Clear();
                        }
                        else
                        {

                            //Convert Data to a String
                            ASCIIEncoding encoding = new ASCIIEncoding();
                            string StrData = encoding.GetString(CurrentLine.ToArray());

                            //Add it to the Text Data List
                            TextData.Add(StrData);

                            //Add it to the Text Box
                            rtb_serialdata.Invoke(new Action(() =>
                            {
                                rtb_serialdata.AppendText(StrData);
                                if (cb_autoscroll.Checked)
                                {
                                    rtb_serialdata.ScrollToCaret();
                                }
                            }));

                            //Clear Current Line Data
                            CurrentLine.Clear();
                        }
                    }
                }
            }
        }

        private void cmb_ports_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((cmb_baud.SelectedIndex >= 0) && (cmb_baud.SelectedIndex >= 0))
            {
                btn_connect.Enabled = true;
            }
            else
            {
                btn_connect.Enabled = false;
            }
        }

        private void cmb_baud_SelectedIndexChanged(object sender, EventArgs e)
        {

            if ((cmb_baud.SelectedIndex >= 0) && (cmb_baud.SelectedIndex >= 0))
            {
                btn_connect.Enabled = true;
            }
            else
            {
                btn_connect.Enabled = false;
            }
        }

        private void cb_autoscroll_CheckedChanged(object sender, EventArgs e)
        {
            Autoscroll = cb_autoscroll.Checked;
        }

        private void btn_send_Click(object sender, EventArgs e)
        {
            ComPort.Write(tb_send.Text + cmb_eol.SelectedValue);
            tb_send.Clear();
        }

        private void btn_reset_Click(object sender, EventArgs e)
        {
            ComPort.DtrEnable = true;
            System.Threading.Thread.Sleep(500);
            ComPort.DtrEnable = false;
        }

        private void requestSymbolsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ASI.getSymbols(ComPort);
        }

        private void restartDBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DB.Reset();
            ASI.getSymbols(ComPort);
        }



    }
}
