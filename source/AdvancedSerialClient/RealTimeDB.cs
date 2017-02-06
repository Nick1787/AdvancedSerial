using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;

namespace AdvancedSerial
{
    public class RealTimeDB
    {
        //Hide the dictionary, force people to use functions
        private Dictionary<String, List<Tuple<Single,Single>>> Data;
        private int LockKey = -1;
        private Stopwatch sw = new Stopwatch();
        private Dictionary<String, List<Tuple<Single, Single>>> Buffer;

        SqlConnection SQLDB;

        //Instantiation
        public RealTimeDB(){
            Data = new Dictionary<String, List<Tuple<Single, Single>>>();
            Buffer = new Dictionary<String, List<Tuple<Single, Single>>>();
            sw.Start();

            //generate a unique filename
            string mdfName = "AdvSerialDB.mdf";
            string dir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string DBFilename =  dir + "\\" + mdfName;
            string DBName = System.IO.Path.GetFileNameWithoutExtension(DBFilename);

            //Create the database
            if (!System.IO.File.Exists(mdfName))
            {
                CreateDatabase(DBName, DBFilename);
            }
            else
            {
                System.IO.File.Delete(DBFilename);
                CreateDatabase(DBName, DBFilename);
            }

            SQLDB = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\AdvSerialDB.mdf;Integrated Security=True;MultipleActiveResultSets=true");
            SQLDB.Open();
            
            //Delete any existing tables
            DataTable schema = SQLDB.GetSchema("Tables");
            List<string> TableNames = new List<string>();
            foreach (DataRow row in schema.Rows) { 
                SqlCommand SQLCMD = new SqlCommand("DELETE " + row, SQLDB);
                SQLCMD.ExecuteNonQuery();
            };

            //Create a Table
            string createTable = "CREATE TABLE DBDATA(signal char(50), time FLOAT, value FLOAT );";
            SqlCommand SQLCMD2 = new SqlCommand(createTable, SQLDB);
            SQLCMD2.ExecuteNonQuery();
            
        }

        //Request to Lock Database
        public int requestLock()
        {
            if (LockKey == -1)
            {
                Random rnd = new Random();
                LockKey = rnd.Next(0, 1000);
                return LockKey;
            }
            else
            {
                return -1;
            }
        }

        //Return Lock
        public void returnLock(int Key)
        {
            if (Key == LockKey)
            {
                LockKey = -1;
            }
        }

        //Reset
        public void Reset()
        {
            Data.Clear();

            //Clear out the DB
            string cmdStr = "DELETE * FROM DBDATA;";
            SqlCommand SQLCMD = new SqlCommand(cmdStr, SQLDB);
            SQLCMD.ExecuteNonQuery();

            Buffer.Clear();
            sw.Reset();
            sw.Start();
        }

        //Signals
        public List<String> Signals()
        {
            // return Data.Keys.ToList();
            List<String> retSignals = new List<String>();

            string cmdStr = "SELECT DISTINCT signal FROM DBDATA;";
            SqlDataReader reader;
            SqlCommand SQLCMD = new SqlCommand(cmdStr, SQLDB);
            reader = SQLCMD.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    String signal = reader.GetString(0);
                    retSignals.Add(signal);
                }
            }
            else
            {
                Console.WriteLine("No rows found.");
            }
            reader.Close();

            return retSignals;
        }

        //StoreCSV
        public bool storeCSV(string outFileName, Single startTime, Single endTime, Single DataRate)
        {

            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(outFileName))
                {

                    // return Data.Keys.ToList();
                    Dictionary<String, List<Tuple<Single, Single>>> csvData = new Dictionary<String, List<Tuple<Single, Single>>>();
                    List<String> SignalNames = Signals();

                    //Write CSV Header
                    file.WriteLine("time," + String.Join(",", SignalNames));

                    //Get all the Data
                    Single minTime = Single.MaxValue;
                    foreach (String sig in SignalNames)
                    {
                        List<Tuple<Single, Single>> Data = Read(sig, startTime, endTime);
                        if (Data.Count > 0) { 
                            minTime = Math.Min(minTime, Data[0].Item1);
                            csvData.Add(sig, Data);
                        }
                    }

                    //Write Data Line by Line
                    for( Single time = 0; time<=(endTime-startTime); time = time + DataRate)
                    {
                        String LineData = time.ToString() + ",";
                        bool allFound = true;
                        foreach (String sig in SignalNames)
                        {
                            List<Tuple<Single, Single>> Data = csvData[sig];

                            //Find the data point which lines up with the time, if needed, interpolate
                            bool found = false;
                            int lasti = 0;
                            for (int i = 0; i < Data.Count-1; i++)
                            {
                                lasti = i;
                               if (Data[i+1].Item1 >= time + minTime)
                               {
                                    Single interpValue = Data[i].Item2 + ((time + minTime) - Data[i].Item1) * (Data[i + 1].Item2 - Data[i].Item2) / (Data[i + 1].Item1 - Data[i].Item1);
                                    found = true;
                                    LineData  = LineData + interpValue.ToString() + ",";
                                    break;
                                }
                               if(i == Data.Count - 1)
                                {
                                    //End of Data set.
                                    found = true;
                                    break;
                                }
                            }

                            if (!found)
                            {
                                allFound = false;
                            }

                        }

                        if (allFound)
                        {
                            file.WriteLine(LineData);
                        }
                    }
                    file.Close();
                }
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }

        }

        //Read Data
        public List<Tuple<Single,Single>> Read(String ParamName)
        {
                        
            string cmdStr = "SELECT time,value FROM DBDATA WHERE signal='" + ParamName + "'";
            SqlDataReader reader;
            SqlCommand SQLCMD = new SqlCommand(cmdStr, SQLDB);
            reader = SQLCMD.ExecuteReader();

            List<Tuple<Single, Single>> retData = new List<Tuple<Single, Single>>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Single time = Convert.ToSingle(reader.GetDouble(0));
                    Single value = Convert.ToSingle(reader.GetDouble(1));
                    retData.Add(new Tuple<Single, Single>( time, value) );
                }
            }
            else
            {
                Console.WriteLine("No rows found.");
            }
            reader.Close();

            return retData;
        }
        
        //Read Data and Specify a time range
        public List<Tuple<Single, Single>> Read(String ParamName, Single StartTime, Single EndTime)
        {

            string cmdStr = "SELECT time,value FROM DBDATA WHERE signal='" + ParamName + "' AND time>=" + StartTime.ToString() + " AND time<=" + EndTime.ToString();
            SqlDataReader reader;
            SqlCommand SQLCMD = new SqlCommand(cmdStr, SQLDB);
            reader = SQLCMD.ExecuteReader();

            List<Tuple<Single, Single>> retData = new List<Tuple<Single, Single>>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Single time = Convert.ToSingle(reader.GetDouble(0));
                    Single value = Convert.ToSingle(reader.GetDouble(1));
                    retData.Add(new Tuple<Single, Single>(time, value));
                }
            }
            else
            {
                Console.WriteLine("No rows found.");
            }
            reader.Close();

            return retData;
        }

        public Double GetTimeMs() {
            return sw.ElapsedMilliseconds;
        }
        public Double GetTimeSec()
        {
            return sw.ElapsedMilliseconds/1000;
        }

        public void Write(String ParamName, Single TimeSec, Single ParamData)
        {
            //Write Data to DB
            string cmdStr = "INSERT INTO DBDATA (signal, time, value) VALUES ('" + ParamName + "'," + TimeSec.ToString() + "," + ParamData.ToString() + ")";
            SqlCommand SQLCMD = new SqlCommand(cmdStr, SQLDB);
            SQLCMD.ExecuteNonQuery();
        }

        public void Write(String ParamName, List<Tuple<Single,Single>> ParamData)
        {
            foreach( Tuple<Single,Single> Data in ParamData)
            {
                double time = GetTimeSec();
                Write(ParamName, Data.Item1, Data.Item2);
            }
        }

        public static bool CreateDatabase(string dbName, string dbFileName)
        {
            try
            {
                string connectionString = String.Format(@"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True");
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = connection.CreateCommand();

                    String ldfName = System.IO.Path.GetDirectoryName(dbFileName) + "\\" + System.IO.Path.GetFileNameWithoutExtension(dbFileName) + "_log.ldf";
                    if(System.IO.File.Exists(ldfName))
                    {
                        System.IO.File.Delete(ldfName);
                    }
                    DetachDatabase(dbName);
                    cmd.CommandText = String.Format("CREATE DATABASE {0} ON (NAME = '{0}', FILENAME = '{1}')", dbName, dbFileName);
                    cmd.ExecuteNonQuery();
                }

                if (File.Exists(dbFileName)) return true;
                else return false;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public static bool DetachDatabase(string dbName)
        {
            try
            {
                string connectionString = String.Format(@"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True");
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = String.Format("exec sp_detach_db '{0}'", dbName);
                    cmd.ExecuteNonQuery();

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }




    }
}
