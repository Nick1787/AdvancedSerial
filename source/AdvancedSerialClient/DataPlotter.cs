using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using Microsoft.Research.DynamicDataDisplay;
using System.Text.RegularExpressions;

using OxyPlot;
using OxyPlot.Series;
using OxyPlot.WindowsForms;
using AdvancedSerial;

namespace AdvancedSerialClient
{
    public partial class DataPlotter : Form
    {

        private PlotView Plot;
        PlotModel DataPlot;
        Single XAxisLength = 0;

        //Update Plotting Stuff
        System.Threading.Timer updateTimer;
        public bool UpdateLock = false;
        private delegate void updateChartDelegate(LineSeries Ser, List<Tuple<Single, Single>> Data);
        private delegate void addSeriesDelegate(LineSeries Ser);
        private delegate void removeSeriesDelegate(LineSeries Ser);

        //DataBase
        private RealTimeDB DB;
        private List<String> SignalNames;

        //Form Stuff
        bool ShowNames = true;
        int lastSplitterDistance = 200;
        private Double XAxisScale = 10;

        public DataPlotter(RealTimeDB Database)
        {
            InitializeComponent();
            XAxisLength = 10;

            //Add new Plot
            Plot = new PlotView();

            //Add the plot form
            Plot.Location = new Point(0, 20);
            Plot.Width = this.Width;
            Plot.Height = this.Height;
            DataPlot = new PlotModel()
            {
                Title = "Advanced Serial Plotter",
                //PlotType = PlotType.Cartesian,
                Background = OxyColors.White,
                LegendOrientation = LegendOrientation.Horizontal,
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.RightTop,
                LegendBackground = OxyColor.FromAColor(200, OxyColors.White),
                LegendBorder = OxyColors.Black,
                IsLegendVisible = true,
            }; 

            Plot.Model = DataPlot;
            this.Controls.Add(this.Plot);
            Plot.Parent = Splitter.Panel1;
            Plot.Update();

            //Attatch to the database
            DB = Database;
            updateSignalNames();
            
            //Event Handlers
            this.Resize += new EventHandler(FrmResize);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(FrmClosing);

            // Create a Timer to update the Plot Data.
            AutoResetEvent autoEvent = new AutoResetEvent(false);
            TimerCallback tcb = Update;
            updateTimer = new System.Threading.Timer(tcb, autoEvent, 100, 200);
            
            TB_XAxisTime.Text = XAxisScale.ToString();
            FrmResize(this, new System.EventArgs());
        }

        /**FORM STUFF*************************************************************/
        private void FrmResize(object sender, System.EventArgs e)
        {


            Splitter.Size = new Size(this.Size.Width, this.Size.Height - toolStrip1.Size.Height);
            if (!(ShowNames))
            {
                if (Splitter.Width > 20)
                {
                    Splitter.SplitterDistance = this.Size.Width;
                }
                Splitter.Panel2Collapsed = true;
                Splitter.Panel2.Hide();
            }
            else
            {
                Splitter.Panel2Collapsed = false;
                Splitter.Panel2.Show();
                if (Splitter.Width > 20)
                {
                    Splitter.SplitterDistance = Math.Max(Splitter.Panel1MinSize, this.Size.Width - lastSplitterDistance);
                }
            }


            Plot.Location = new Point(0, 15);
            Plot.Width = Splitter.Panel1.Width;
            Plot.Height = Splitter.Panel1.Height - 15;

            dgvplots.Width = Splitter.Panel2.Width;
            dgvplots.Columns[1].Width = dgvplots.Width - dgvplots.Columns[0].Width - 23;
            dgvplots.Height = Splitter.Panel2.Height;
        }

        private void SplitterMouseUp(object sender, MouseEventArgs e)
        {
            lastSplitterDistance = this.Size.Width - e.Location.X;
            FrmResize(this, new System.EventArgs());
        }

        private void FrmClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            updateTimer.Dispose();
        }

        private void updateSignalNames()
        {
            if (!(DB == null))
            {
                SignalNames = DB.Signals();

                DataGridViewComboBoxColumn Col = (DataGridViewComboBoxColumn)dgvplots.Columns[1];
                Col.DataSource = SignalNames;
            }
        }

        private void dgvplots_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            updateSignalNames();

            if (e.ColumnIndex == 0)
            {

            }
        }

        private void TB_XAxisTime_KeyPress(Object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                this.SelectNextControl(this.ActiveControl, true, true, true, true);
            }
        }

        private void TB_XAxisTime_Validating(Object sender, CancelEventArgs e)
        {
            String newText = TB_XAxisTime.Text;
            if (TB_XAxisTime.Text.Trim().Equals('*'))
            {

            }
            else if (Regex.Match(TB_XAxisTime.Text.Trim(), @"^(\d*\.\d*|\d+)$").Success)
            {
                try
                {
                    Single.TryParse(TB_XAxisTime.Text.Trim(), out XAxisLength);
                }
                catch (Exception)
                {

                }
            }
            else
            {
                e.Cancel = true;
            }

        }
        

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (ShowNames)
            {
                btn_ShowNames.Text = "<";
                ShowNames = false;
                FrmResize(this, new System.EventArgs());
            }
            else
            {
                btn_ShowNames.Text = ">";
                ShowNames = true;
                FrmResize(this, new System.EventArgs());

            }
        }


        /**Plotting Stuff************************************************************/

        private void Update(Object stateInfo)
        {
            if (!(btn_PauseRun.Checked))
            {

            if (this.IsDisposed)
            {
                return;
            }

            int Lock = DB.requestLock();
            if (Lock >= 0)
            {
                for (int rw = 0; rw < dgvplots.Rows.Count; rw++)
                {
                    DataGridViewCheckBoxCell visibleCell = (DataGridViewCheckBoxCell)dgvplots.Rows[rw].Cells[0];
                    DataGridViewComboBoxCell paramNameCell = (DataGridViewComboBoxCell)dgvplots.Rows[rw].Cells[1];

                    if (!(paramNameCell.Value == null) && (DB.Signals().Contains(paramNameCell.Value.ToString())))
                    {
                        if ((bool)visibleCell.EditedFormattedValue)
                        {
                            //Find the series which goes with this rw.
                            LineSeries Ser = null;
                            foreach (LineSeries CSer in DataPlot.Series)
                            {
                                if (CSer.Tag.Equals(rw))
                                {
                                    Ser = CSer;
                                    break;
                                }
                            }

                            //If Series is still void, its not being plotted to add a new series to plot
                            if (Ser == null)
                            {
                                Ser = new LineSeries();
                                Ser.Title = paramNameCell.Value.ToString().Trim() ;
                                Ser.Tag = rw;
                                AddChartSeries(Ser);
                                //Ser = new Series(rw.ToString());
                                //Ser.ChartType = SeriesChartType.FastLine;
                                //Ser.ChartArea = Chart.ChartAreas[0].Name;
                                //Ser.BorderWidth = 2;
                                //Ser.LegendText = paramNameCell.Value.ToString();

                                if (!(this.Plot.IsDisposed))
                                {
                                    if (this.Plot.InvokeRequired)
                                    {
                                        addSeriesDelegate d = new addSeriesDelegate(AddChartSeries);
                                        this.Invoke(d, new object[] { Ser });
                                    }
                                    else
                                   {
                                        AddChartSeries(Ser);
                                    }
                                }
                            }
                            else
                            {
                                if (!(Ser.Tag.Equals(rw)))
                                {

                                    LineSeries Ser2 = new LineSeries();
                                        Ser2.Title = paramNameCell.Value.ToString();
                                        Ser2.Tag = rw;

                                    if (!(this.Plot.IsDisposed))
                                    {
                                        if (this.Plot.InvokeRequired)
                                        {
                                            removeSeriesDelegate d_rem = new removeSeriesDelegate(RemovedChartSeries);
                                            this.Invoke(d_rem, new object[] { Ser });
                                      
                                            addSeriesDelegate d_add = new addSeriesDelegate(AddChartSeries);
                                            this.Invoke(d_add, new object[] { Ser2 });
                                        }
                                        else
                                        {
                                            RemovedChartSeries(Ser);
                                            AddChartSeries(Ser2);
                                        }
                                    }
                                }
                            }


                            if (!(this.Plot.IsDisposed))
                            {
                                if (this.Plot.InvokeRequired)
                                {
                                    updateChartDelegate d = new updateChartDelegate(UpdateSeriesData);
                                    try
                                    {
                                        this.Invoke(d, new object[] { Ser, DB.Read(paramNameCell.Value.ToString(), Convert.ToSingle(DB.GetTimeMs()/1000 - XAxisLength - 10), Convert.ToSingle(DB.GetTimeMs() / 1000  )) });
                                    }
                                   catch (Exception ex)
                                    {
                                    }
                                }
                                else
                                {
                                    String SignalName = paramNameCell.Value.ToString();
                                    UpdateSeriesData(Ser, DB.Read(SignalName, Convert.ToSingle(DB.GetTimeMs() / 1000 - XAxisLength -10), Convert.ToSingle(DB.GetTimeMs() / 1000 )));
                                }
                            }
                        }
                        else
                        {

                            //Find the series which goes with this rw.
                            LineSeries Ser = null;
                            foreach (LineSeries CSer in DataPlot.Series)
                            {
                                if (CSer.Tag.Equals(rw))
                                {
                                    Ser = CSer;
                                    break;
                                }
                            }

                            if (!(Ser == null))
                            {

                                if (!(this.Plot.IsDisposed))
                                {
                                   if (this.Plot.InvokeRequired)
                                   {
                                        removeSeriesDelegate d_rem = new removeSeriesDelegate(RemovedChartSeries);
                                        this.Invoke(d_rem, new object[] { Ser });
                                    }
                                    else
                                   {
                                        RemovedChartSeries(Ser);
                                   }
                                }
                            }
                        }
                    }
                }
            }
            DB.returnLock(Lock);

            }
        }

        private void RemovedChartSeries(LineSeries Ser)
        {
            DataPlot.Series.Remove(Ser);
            DataPlot.InvalidatePlot(true);
        }

        private void AddChartSeries(LineSeries Ser)
        {
            if (!(DataPlot.Series.Contains(Ser))){
                DataPlot.Series.Add(Ser);
            }
            DataPlot.InvalidatePlot(true);
           
        }

        private void UpdateSeriesData(LineSeries Ser, List<Tuple<Single, Single>> Data)
        {
            try
            {
                List<DataPoint> SerPoints = new List<DataPoint>();
                for(int i = 0; i < Data.Count; i++)
                {
                    SerPoints.Add(new DataPoint(Data[i].Item1, Data[i].Item2));
                }
                Ser.Points.Clear();
                Ser.Points.AddMany(SerPoints);
                UpdateXAxis();
            }
            catch (Exception Ex)
            {

            }
            DataPlot.InvalidatePlot(true);
        }

        private void UpdateXAxis()
        {
            try
            {
                //Find the SCaling
                double min = DB.GetTimeMs() /1000 - XAxisLength;
                if (min < 0)
                {
                    min = 0;
                }

                //Find the Bottom Axis
                OxyPlot.Axes.Axis Ax = null;
                for(int i=0; i<DataPlot.Axes.Count; i++)
                {
                    if (DataPlot.Axes[i].Position == OxyPlot.Axes.AxisPosition.Bottom)
                    {
                        Ax = DataPlot.Axes[i];
                    }
                    Double NewMin = DataPlot.Axes[0].Maximum - XAxisScale;

                }
                if(!(Ax == null)) { 
                    Ax.Minimum = min;
                    Ax.Maximum = min + XAxisLength;
                }
            DataPlot.InvalidatePlot(true);
            }
            catch (Exception ex)
            {

            }

        }

        private void btn_PauseRun_Click(object sender, EventArgs e)
        {
            if (btn_PauseRun.Checked)
            {
                btn_PauseRun.Text = "Run";
            }
            else
            {
                btn_PauseRun.Text = "Pause";
            }
        }

        private void BTN_ShowLegend_Click(object sender, EventArgs e)
        {
            if(BTN_ShowLegend.Checked)
            {
                BTN_ShowLegend.Text = "Hide Legend";
                DataPlot.IsLegendVisible = true;
                DataPlot.InvalidatePlot(true);
            }
            else
            {
                BTN_ShowLegend.Text = "Show Legend";
                DataPlot.IsLegendVisible = false;
                DataPlot.InvalidatePlot(true);
            }
        }

        private void TB_XAxisTime_Click(object sender, EventArgs e)
        {

        }
    }
}
