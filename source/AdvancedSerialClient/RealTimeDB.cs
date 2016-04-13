using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace AdvancedSerial
{
    public class RealTimeDB
    {
        //Hide the dictionary, force people to use functions
        private Dictionary<String, List<Tuple<Single,Single>>> Data;
        private int LockKey = -1;
        private Stopwatch sw = new Stopwatch();
        private Dictionary<String, List<Tuple<Single, Single>>> Buffer;

        //Instantiation
        public RealTimeDB(){
            Data = new Dictionary<String, List<Tuple<Single, Single>>>();
            Buffer = new Dictionary<String, List<Tuple<Single, Single>>>();
            sw.Start();
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
            Buffer.Clear();
            sw.Reset();
            sw.Start();
        }

        //Signals
        public List<String> Signals()
        {
            return Data.Keys.ToList();
        }

        //Read Data
        public List<Tuple<Single,Single>> Read(String ParamName)
        {
            if(Data.Keys.Contains(ParamName)){
                return Data[ParamName];
            }else{
                //Return an empty List Data not Found;
                return new List<Tuple<Single,Single>>();
            }
        }

        public Double GetTimeMs() {
            return sw.ElapsedMilliseconds;
        }

        public void Write(String ParamName, Single TimeSec, Single ParamData)
        {
            if (!Data.Keys.Contains(ParamName))
            {
                Data.Add(ParamName, new List<Tuple<Single,Single>>());
            }
            Data[ParamName].Add(new Tuple<Single,Single>(TimeSec,ParamData));
        }


        public void Write(String ParamName, List<Tuple<Single,Single>> ParamData)
        {
            if (!Data.Keys.Contains(ParamName))
            {
                Data.Add(ParamName, new List<Tuple<Single, Single>>());
            }
            Data[ParamName].AddRange(ParamData);
        }

    }
}
