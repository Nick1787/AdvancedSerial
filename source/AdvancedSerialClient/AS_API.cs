using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Text.RegularExpressions;

namespace AdvancedSerial
{
    class AdvancedSerial_API
    {
        public enum asi_dtypes : byte { asi_bool = 0x0, asi_byte = 0x1, asi_short = 0x2, asi_int = 0x3, asi_uint = 0x4, asi_long = 0x5, asi_ulong = 0x6, asi_float = 0x7, asi_double = 0x8 };
            
        public class ASI_Signal{
            public String Name = "";
            public asi_dtypes Type = asi_dtypes.asi_bool;
        }

        //Database
        RealTimeDB DB;
        private Dictionary<Int32, List<Tuple<Single,Single>>> Buffer = new Dictionary<Int32, List<Tuple<Single,Single>>>();
        public Dictionary<Int32, ASI_Signal> ASI_Signals = new Dictionary<Int32, ASI_Signal>();

        //Instantiation
        public AdvancedSerial_API(RealTimeDB DatabaseRef){
            DB = DatabaseRef;
        }

        public void getSymbols(SerialPort ComPort){
            byte[] msgkey = {0xA0};
            UInt32 msgid = 01;
            byte[] idbuffer = BitConverter.GetBytes(msgid);
            ComPort.Write("#ASI:");
            ComPort.Write(msgkey,0,1);
            ComPort.Write(":");
            ComPort.Write(idbuffer, 0, 4);
            ComPort.Write("\r\n");
        }

        public void ProcessMessage(List<byte> inData)
        {

            byte msgkey =  inData[5];
            uint msgid = inData[7];

            int start = 9;
            switch(msgkey){
                case 0xB0:
                    //Recieved a new Symbol List
                    start = 9;
                    while (inData.Count() > (start  + 2 + 2))
                    {
                        Int32 SignalKey = BitConverter.ToInt16(inData.Skip(start).Take(2).ToArray(),0);

                        ASCIIEncoding encoding = new ASCIIEncoding();

                        //Get Name, read to null charcater
                        string SignalName = "";
                        int NameCharCnt = 0;
                        bool done = false;
                        while (!done)
                        {
                            if (inData[start + 2 + NameCharCnt] == (byte)'\0')
                            {
                                SignalName = encoding.GetString(inData.Skip(start + 2).Take(NameCharCnt).ToArray());
                                break;
                            }

                            if (NameCharCnt > 25)
                            {
                                done = true;
                                break;
                            }
                            NameCharCnt = NameCharCnt + 1;
                        }

                        byte SignalType = inData[start + 2 + NameCharCnt +1 ];
                        start = start + 2 + NameCharCnt + 2;
                        if(ASI_Signals.Keys.Contains(SignalKey)){
                            ASI_Signals[SignalKey].Name = SignalName;
                            ASI_Signals[SignalKey].Type = (asi_dtypes)SignalType;
                            Buffer[SignalKey].Clear();
                        }else{
                            ASI_Signal Signal = new ASI_Signal();
                            Signal.Name = SignalName;
                            Signal.Type = (asi_dtypes)SignalType;

                            ASI_Signals.Add(SignalKey, Signal);
                            Buffer[SignalKey] = new List<Tuple<Single, Single>>();
                        }
                    }
                    break;
                case 0xB1:
                    //Recieved Symbol Data
                    start = 9;
                    while (inData.Count() > (start + 1))
                    {
                        Int32 SignalKey = BitConverter.ToInt16(inData.Skip(start).Take(2).ToArray(),0);
                        if (ASI_Signals.Keys.Contains(SignalKey)){

                            asi_dtypes Type = ASI_Signals[SignalKey].Type;
                            String Name = ASI_Signals[SignalKey].Name;
                            Double TimeMS = DB.GetTimeMs();
                            Single TimeSec = (Single)(TimeMS / 1000);

                            switch(Type){
                                case(asi_dtypes.asi_bool):{
                                    bool Value = BitConverter.ToBoolean(inData.Skip(start + 2).Take(1).ToArray(), 0);
                                    int lck = DB.requestLock();
                                    if (lck >= 0)
                                    {
                                        //Flush the buffer
                                        DB.Write(Name, Buffer[SignalKey]);
                                        Buffer[SignalKey].Clear();

                                        //Write new data
                                        DB.Write(Name, TimeSec, Convert.ToSingle(Value));
                                        DB.returnLock(lck);
                                    }
                                    else
                                    {
                                        Buffer[SignalKey].Add(new Tuple<Single, Single>(TimeSec, Convert.ToSingle(Value)));
                                    }
                                    start = start + 3;
                                    break;
                                }
                                case(asi_dtypes.asi_byte):{
                                    byte Value = inData[start + 2];
                                    int lck = DB.requestLock();
                                    if (lck >= 0)
                                    {
                                        //Flush the buffer
                                        DB.Write(Name, Buffer[SignalKey]);
                                        Buffer[SignalKey].Clear();

                                        //Write new data
                                        DB.Write(Name, TimeSec, Convert.ToSingle(Value));
                                        DB.returnLock(lck);
                                    }
                                    else
                                    {
                                        Buffer[SignalKey].Add(new Tuple<Single, Single>(TimeSec, Convert.ToSingle(Value)));
                                    }
                                    start = start + 3;
                                    break;
                                }
                                case(asi_dtypes.asi_double):{
                                    double Value = BitConverter.ToDouble(inData.Skip(start+2).Take(8).ToArray(),0);
                                    int lck = DB.requestLock();
                                    if (lck >= 0)
                                    {
                                        //Flush the buffer
                                        DB.Write(Name, Buffer[SignalKey]);
                                        Buffer[SignalKey].Clear();

                                        //Write new data
                                        DB.Write(Name, TimeSec, Convert.ToSingle(Value));
                                        DB.returnLock(lck);
                                    }
                                    else
                                    {
                                        Buffer[SignalKey].Add(new Tuple<Single, Single>(TimeSec, Convert.ToSingle(Value)));
                                    }
                                    start = start + 10;
                                    break;
                                }
                                case(asi_dtypes.asi_float):{
                                    float Value = BitConverter.ToSingle(inData.Skip(start + 2).Take(4).ToArray(), 0);
                                    int lck = DB.requestLock();
                                    if (lck >= 0)
                                    {
                                        //Flush the buffer
                                        DB.Write(Name, Buffer[SignalKey]);
                                        Buffer[SignalKey].Clear();

                                        //Write new data
                                        DB.Write(Name, TimeSec, Convert.ToSingle(Value));
                                        DB.returnLock(lck);
                                    }
                                    else
                                    {
                                        Buffer[SignalKey].Add(new Tuple<Single, Single>(TimeSec, Convert.ToSingle(Value)));
                                    }
                                    start = start + 6;
                                    break;
                                }
                                case(asi_dtypes.asi_int):{
                                    Int16 Value = BitConverter.ToInt16(inData.Skip(start + 2).Take(2).ToArray(), 0);
                                    int lck = DB.requestLock();
                                    if (lck >= 0)
                                    {
                                        //Flush the buffer
                                        DB.Write(Name, Buffer[SignalKey]);
                                        Buffer[SignalKey].Clear();

                                        //Write new data
                                        DB.Write(Name, TimeSec, Convert.ToSingle(Value));
                                        DB.returnLock(lck);
                                    }
                                    else
                                    {
                                        Buffer[SignalKey].Add(new Tuple<Single, Single>(TimeSec, Convert.ToSingle(Value)));
                                    }
                                    start = start + 4;
                                    break;
                                }
                                case(asi_dtypes.asi_long):{
                                    Int32 Value = BitConverter.ToInt32(inData.Skip(start + 2).Take(4).ToArray(), 0);
                                    int lck = DB.requestLock();
                                    if (lck >= 0)
                                    {
                                        //Flush the buffer
                                        DB.Write(Name, Buffer[SignalKey]);
                                        Buffer[SignalKey].Clear();

                                        //Write new data
                                        DB.Write(Name, TimeSec, Convert.ToSingle(Value));
                                        DB.returnLock(lck);
                                    }
                                    else
                                    {
                                        Buffer[SignalKey].Add(new Tuple<Single, Single>(TimeSec, Convert.ToSingle(Value)));
                                    }
                                    start = start + 6;
                                    break;
                                }  
                                case(asi_dtypes.asi_short):{
                                    Int16 Value = BitConverter.ToInt16(inData.Skip(start + 2).Take(2).ToArray(), 0);
                                    int lck = DB.requestLock();
                                    if (lck >= 0)
                                    {
                                        //Flush the buffer
                                        DB.Write(Name, Buffer[SignalKey]);
                                        Buffer[SignalKey].Clear();

                                        //Write new data
                                        DB.Write(Name, TimeSec, Convert.ToSingle(Value));
                                        DB.returnLock(lck);
                                    }
                                    else
                                    {
                                        Buffer[SignalKey].Add(new Tuple<Single, Single>(TimeSec, Convert.ToSingle(Value)));
                                    }
                                    start = start + 4;
                                    break;
                                }
                                case(asi_dtypes.asi_uint):{
                                    UInt16 Value = BitConverter.ToUInt16(inData.Skip(start + 2).Take(2).ToArray(), 0);
                                    int lck = DB.requestLock();
                                    if (lck >= 0)
                                    {
                                        //Flush the buffer
                                        DB.Write(Name, Buffer[SignalKey]);
                                        Buffer[SignalKey].Clear();

                                        //Write new data
                                        DB.Write(Name, TimeSec, Convert.ToSingle(Value));
                                        DB.returnLock(lck);
                                    }
                                    else
                                    {
                                        Buffer[SignalKey].Add(new Tuple<Single, Single>(TimeSec, Convert.ToSingle(Value)));
                                    }
                                    start = start + 4;
                                    break;
                                }
                                case(asi_dtypes.asi_ulong):{
                                    UInt32 Value = BitConverter.ToUInt32(inData.Skip(start + 2).Take(4).ToArray(), 0);
                                    int lck = DB.requestLock();
                                    if (lck >= 0)
                                    {
                                        //Flush the buffer
                                        DB.Write(Name, Buffer[SignalKey]);
                                        Buffer[SignalKey].Clear();

                                        //Write new data
                                        DB.Write(Name, TimeSec, Convert.ToSingle(Value));
                                        DB.returnLock(lck);
                                    }
                                    else
                                    {
                                        Buffer[SignalKey].Add(new Tuple<Single, Single>(TimeSec, Convert.ToSingle(Value)));
                                    }
                                    start = start + 6;
                                    break;
                                }
                                default:{
                                    start = start+1;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            break;
                        }

                    }
                    break;
            }
        }
    }
}
