using System;
using System.Collections.Generic;
using System.Text;
using OOGLibrary.IO;

namespace OOGLibrary.IO
{
    class DataStreamCreate
    {
        List<byte> bttype = new List<byte>();
        List<byte> btlen = new List<byte>();
        List<byte> btbuf = new List<byte>();
        uint len = 0;
        public DataStream WriteByte(byte bt)
        {
            try
            {
                if (!CheckCUInt32(bttype.ToArray()))
                {
                    bttype.Add(bt);
                    return null;
                }
                if (!CheckCUInt32(btlen.ToArray()))
                {
                    btlen.Add(bt);
                    if (CheckCUInt32(btlen.ToArray()))
                    {
                        len = ReadCUInt32(btlen.ToArray());
                        if (len > 0) return null;
                    }
                    else
                        return null;
                }
                if (btbuf.Count < len) btbuf.Add(bt);
                if (btbuf.Count == len)
                {
                    uint type = ReadCUInt32(bttype.ToArray());
                    byte[] buf = btbuf.ToArray();
                    DataStream ds = new DataStream() { Buffer = buf };
                    ds.Type = type;

                    btbuf.Clear();
                    bttype.Clear();
                    btlen.Clear();
                    len = 0;
                    return ds;
                }
            }
            catch { }
            return null;
        }
        public byte[] GetBytes(DataStream ds)
        {
            byte[] tp = WriteCUInt32(ds.Type);
            byte[] len = WriteCUInt32((uint)ds.Length);

            List<byte> listbuf = new List<byte>();
            listbuf.AddRange(tp);
            listbuf.AddRange(len);
            listbuf.AddRange(ds.Buffer);
            return listbuf.ToArray();
        }

        public uint ReadCUInt32(byte[] bt)
        {
            switch (bt[0] & 0xE0)
            {
                case 0xE0:
                    return BitConverter.ToUInt32(new byte[] { bt[4], bt[3], bt[2], bt[1] }, 0);
                case 0xC0:
                    return BitConverter.ToUInt32(new byte[] { bt[3], bt[2], bt[1], bt[0] }, 0) & 0x1FFFFFFF;
                case 0x80:
                case 0xA0:
                    return (uint)(BitConverter.ToUInt16(new byte[] { bt[1], bt[0] }, 0) & 0x3FFF);
            }
            return (uint)bt[0];
        }
        public byte[] WriteCUInt32(uint value)
        {
            if (value <= 0x7F)
            {
                return new byte[] { (byte)value };
            }
            if (value <= 0x3FFF)
            {
                byte[] bt = BitConverter.GetBytes((ushort)(value + 0x8000));
                Array.Reverse(bt);
                return bt;
            }
            if (value <= 0x1FFFFFFF)
            {
                byte[] bt = BitConverter.GetBytes((uint)(value + 0xC0000000));
                Array.Reverse(bt);
                return bt;
            }
            if (value <= 0xFFFFFFFF)
            {
                List<byte> bt = new List<byte>();
                bt.Add(0xE0);
                byte[] arrbt = BitConverter.GetBytes((uint)value);
                Array.Reverse(arrbt);
                bt.AddRange(arrbt);
                return bt.ToArray();
            }
            return new byte[0];
        }
        public bool CheckCUInt32(byte[] bt)
        {
            if (bt.Length == 0) return false;
            switch (bt[0] & 0xE0)
            {
                case 0xE0:
                    if (bt.Length != 5) return false;
                    return true;
                case 0xC0:
                    if (bt.Length != 4) return false;
                    return true;
                case 0x80:
                case 0xA0:
                    if (bt.Length != 2) return false;
                    return true;
            }
            return true;
        }
    }
}
