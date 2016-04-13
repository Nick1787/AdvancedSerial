using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Threading;
using Microsoft.Research.DynamicDataDisplay;
using System.Text.RegularExpressions;

namespace AdvancedSerial
{
    public partial class Plotter : Form
    {
        //Chart
        //Microsoft.Research.DynamicDataDisplay.TimeChartPlotter Chart = new TimeChartPlotter();
        //Microsoft.Research.DynamicDataDisplay.ChartPlotter ChartPlt = new ChartPlotter();

        //Update Plotting Stuff
        System.Threading.Timer updateTimer;
        public bool UpdateLock = false;
        private delegate void updateChartDelegate(Series Ser, List<Tuple<Single,Single>> Data);
        private delegate void addSeriesDelegate(Series Ser);
        private delegate void removeSeriesDelegate(Series Ser);

        //DataBase
        private RealTimeDB DB;
        private List<String> SignalNames;

        //Form Stuff
        bool ShowNames = true;
        int lastSplitterDistance = 200;
        private Double XAxisScale = 10;

        /**Iastantiation**********************************************************/

        public Plotter(RealTimeDB Database)
        {
            //Initialize the Form
            InitializeComponent();

            //Attatch to the database
            DB = Database;
            updateSignalNames();

            //Format X axis to show tens of timescale
            Chart.ChartAreas[0].AxisX.LabelStyle.Format = "#.##";

            //Event Handlers
            this.Resize += new EventHandler(FrmResize);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(FrmClosing);


            // Create a Timer to update the Plot Data.
            AutoResetEvent autoEvent     = new AutoResetEvent(false);
            TimerCallback tcb = Update;
            updateTimer = new System.Threading.Timer(tcb, autoEvent, 100, 200);

            this.Chart.ChartAreas[0].AxisX.Minimum = 0;
            TB_XAxisTime.Text = XAxisScale.ToString();
            FrmResize(this, new System.EventArgs());
        }
        
        /**FORM STUFF*************************************************************/
        private void FrmResize(object sender, System.EventArgs e){


            Splitter.Size = new Size(this.Size.Width, this.Size.Height - toolStrip1.Size.Height);
            if (!(ShowNames))
            {
                Splitter.SplitterDistance = this.Size.Width;
                Splitter.Panel2Collapsed = true;
                Splitter.Panel2.Hide();
            }
            else
            {
                Splitter.Panel2Collapsed = false;
                Splitter.Panel2.Show();
                Splitter.SplitterDistance =  this.Size.Width - lastSplitterDistance;
            }


            Chart.Location = new Point(0, 0);
            Chart.Width = Splitter.Panel1.Width;
            Chart.Height = Splitter.Panel1.Height - 15;

            dgvplots.Width = Splitter.Panel2.Width;
            dgvplots.Columns[1].Width = dgvplots.Width - dgvplots.Columns[0].Width - 23;
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

        private void TB_XAxisTime_Validating(Object sender, CancelEventArgs e)
        {
            String newText = TB_XAxisTime.Text;
            if (TB_XAxisTime.Text.Trim().Equals('*'))
            {

            }
            else if (Regex.Match(TB_XAxisTime.Text.Trim(), @"^(\d*\.\d*|\d+)$").Success)
            {

            }
            else
            {
                e.Cancel = true;
            }

        }

        private void TB_XAxisTime_Click(object sender, EventArgs e)
        {

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

            if (this.IsDisposed)
            {
                return ;
            }

            int Lock = DB.requestLock();
            if (Lock >=0 ){
                for(int rw=0; rw<dgvplots.Rows.Count; rw++)
                {
                    DataGridViewCheckBoxCell visibleCell = (DataGridViewCheckBoxCell)dgvplots.Rows[rw].Cells[0];
                    DataGridViewComboBoxCell paramNameCell = (DataGridViewComboBoxCell)dgvplots.Rows[rw].Cells[1];

                    if (!(paramNameCell.Value == null) && (DB.Signals().Contains(paramNameCell.Value.ToString())))
                    {
                        if ((bool)visibleCell.EditedFormattedValue)
                        {
                            //Find the series which goes with this rw.
                            Series Ser = null;
                            foreach (Series CSer in Chart.Series)
                            {
                                if (CSer.Name.Equals(rw.ToString()))
                                {
                                    Ser = CSer;
                                    break;
                                }
                            }

                            //If Series is still void, its not being plotted to add a new series to plot
                            if (Ser == null)
                            {
                                Ser = new Series(rw.ToString());
                                Ser.ChartType = SeriesChartType.FastLine;
                                Ser.ChartArea = Chart.ChartAreas[0].Name;
                                Ser.BorderWidth = 2;
                                Ser.LegendText = paramNameCell.Value.ToString();

                                if (!(this.Chart.IsDisposed))
                                {
                                    if (this.Chart.InvokeRequired)
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
                                if (!(Ser.Name.Equals(rw.ToString())))
                                {

                                    Series Ser2 = new Series(rw.ToString());
                                    Ser2.ChartType = SeriesChartType.FastLine;
                                    Ser2.ChartArea = Chart.ChartAreas[0].Name;
                                    Ser2.BorderWidth = 2;
                                    Ser2.LegendText = paramNameCell.Value.ToString();

                                    if (!(this.Chart.IsDisposed))
                                    {
                                        if (this.Chart.InvokeRequired)
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


                            if (!(this.Chart.IsDisposed))
                            {
                                if (this.Chart.InvokeRequired)
                                {
                                    updateChartDelegate d = new updateChartDelegate(UpdateSeriesData);
                                    try
                                    {
                                        this.Invoke(d, new object[] { Ser, DB.Read(paramNameCell.Value.ToString()) });
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                }
                                else
                                {
                                    String SignalName = paramNameCell.Value.ToString();
                                    UpdateSeriesData(Ser, DB.Read(SignalName));
                                }
                            }
                        }
                        else
                        {

                            //Find the series which goes with this rw.
                            Series Ser = null;
                            foreach (Series CSer in Chart.Series)
                            {
                                if (CSer.Name.Equals(rw.ToString()))
                                {
                                    Ser = CSer;
                                    break;
                                }
                            }

                            if (!(Ser == null))
                            {

                                if (!(this.Chart.IsDisposed))
                                {
                                    if (this.Chart.InvokeRequired)
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

        private void RemovedChartSeries(Series Ser)
        {
            Chart.Series.Remove(Ser);
        }

        private void AddChartSeries(Series Ser)
        {
            Chart.Series.Add(Ser);
        }

        private void UpdateSeriesData(Series Ser, List<Tuple<Single,Single>> Data)
        {
            try
            {
                Ser.Points.DataBindXY(Data, "Item1", Data, "Item2");
                UpdateXAxis();
                Chart.Refresh();
            }
            catch (Exception Ex)
            {

            }
        }

        private void UpdateXAxis(){
            try {
                Double NewMin = Chart.ChartAreas[0].AxisX.Maximum - XAxisScale;
                if ( NewMin >  Chart.ChartAreas[0].AxisX.Maximum){
                    Chart.ChartAreas[0].AxisX.Minimum = NewMin;
                    Chart.ChartAreas[0].AxisX.Interval = XAxisScale / 10;
                }
            }
            catch (Exception ex)
            {

            }

        }

    }

}
