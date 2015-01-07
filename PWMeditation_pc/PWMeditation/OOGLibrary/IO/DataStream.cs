using System;
using System.Collections.Generic;
using System.Text;
using OOGLibrary.GameTypes;

namespace OOGLibrary.IO
{
    class DataStream
    {
        #region Main
        #region Initialize
        public DataStream() : this(0) { }
        public DataStream(uint type)
        {
            Type = type;
        }
        #endregion
        #region MainData
        public uint Type { get; set; }
        List<byte> buffer = new List<byte>();
        public byte[] Buffer
        {
            get { return buffer.ToArray(); }
            set { Reset(); WriteArray(value); }
        }
        public byte this[int index]
        {
            get
            {
                if (index > Length)
                    throw new IndexOutOfRangeException();
                return buffer[index];
            }
            set
            {
                if (index > Length)
                    throw new IndexOutOfRangeException();
                buffer[index] = value;
            }
        }
        public int Length { get { return buffer.Count; } }
        int shift;
        public int Shift
        {
            get { return shift; }
            set
            {
                if (shift > Length)
                    throw new IndexOutOfRangeException();
                shift = value;
            }
        }
        public bool Swaped { get; set; }
        #endregion
        #region Functions
        public void Skip(int len)
        {
            if (shift + len > Length && shift + len < 0) throw new IndexOutOfRangeException();
            shift += len;
        }
        public void Clear()
        {
            shift = 0;
            buffer.Clear();
            Type = 0;
            Swaped = false;
        }
        public void Reset()
        {
            shift = 0;
        }
        public bool CanRead()
        {
            return shift < Length;
        }
        public bool CanRead(int len)
        {
            return shift + len < Length;
        }
        #endregion
        #endregion
        #region Write
        public DataStream Write(object data)
        {
            return Write(data, Swaped);
        }
        public DataStream Write(object data, bool swap)
        {
            if (data is string) WriteUString((string)data);
            if (data is uint) WriteDword((uint)data, swap);
            if (data is ushort) WriteWord((ushort)data, swap);
            if (data is int) WriteInt32((int)data, swap);
            if (data is short) WriteInt16((short)data, swap);
            if (data is float) WriteFloat((float)data, swap);
            if (data is UnixTime) WriteTime((UnixTime)data, swap);
            if (data is Point3F) WritePoint((Point3F)data, swap);
            if (data is byte[]) WriteArray((byte[])data, swap);
            if (data is byte) WriteByte((byte)data);
            if (data is bool) WriteBoolean((bool)data);
            return this;
        }
        #endregion
        #region String
        public string ReadUString()
        {
            return Encoding.Unicode.GetString(ReadData());
        }
        public DataStream WriteUString(string text)
        {
            return WriteData(Encoding.Unicode.GetBytes(text));
        }

        public string ReadAString()
        {
            return Encoding.ASCII.GetString(ReadData());
        }
        public DataStream WriteAString(string text)
        {
            return WriteData(Encoding.ASCII.GetBytes(text));
        }

        // dword/
        public string ReadUString(bool dword)
        {
            return Encoding.Unicode.GetString(ReadData(dword));
        }
        public DataStream WriteUString(string text, bool dword)
        {
            return WriteData(Encoding.Unicode.GetBytes(text), dword);
        }

        public string ReadAString(bool dword)
        {
            return Encoding.ASCII.GetString(ReadData(dword));
        }
        public DataStream WriteAString(string text, bool dword)
        {
            return WriteData(Encoding.ASCII.GetBytes(text), dword);
        }
        #endregion
        #region Data
        public DataStream WriteDataStream(DataStream ds)
        {
            return WriteData(ds.Buffer, false);
        }
        public DataStream ReadDataStream()
        {
            return new DataStream() { Buffer = ReadData(false) };
        }
        public DataStream WriteDataStream(DataStream ds, bool int32)
        {
            return WriteData(ds.Buffer, int32);
        }
        public DataStream ReadDataStream(bool int32)
        {
            return new DataStream() { Buffer = ReadData(int32) };
        }

        public DataStream WriteData(byte[] buf)
        {
            return WriteData(buf, false);
        }
        public byte[] ReadData()
        {
            return ReadData(false);
        }

        public DataStream WriteData(byte[] buf, bool int32)
        {
            if (int32) WriteInt32(buf.Length, false);
            else WriteCUInt32((uint)buf.Length);
            WriteArray(buf, false);
            return this;
        }
        public byte[] ReadData(bool int32)
        {
            int length;
            length = int32 ? ReadInt32(false) : (int)ReadCUInt32();
            return ReadArray(length, false);
        }
        #endregion
        #region Time
        public UnixTime ReadTime()
        {
            return ReadTime(Swaped);
        }
        public UnixTime ReadTime(bool swap)
        {
            int value = ReadInt32(swap);
            return new UnixTime(value);
        }
        public DataStream WriteTime(UnixTime time)
        {
            return WriteTime(time, Swaped);
        }
        public DataStream WriteTime(UnixTime time, bool swap)
        {
            WriteInt32(time.Timestamp, swap);
            return this;
        }
        #endregion
        #region
        public CoreVersion ReadCoreVersion()
        {
            return new CoreVersion(ReadByte(), ReadByte(), ReadByte(), ReadByte());
        }
        #endregion
        #region Point
        public DataStream WritePoint(Point3F point)
        {
            return WritePoint(point, Swaped);
        }
        public DataStream WritePoint(Point3F point, bool swap)
        {
            return point.Serialize(this, swap);
        }
        public Point3F ReadPoint()
        {
            return ReadPoint(Swaped);
        }
        public Point3F ReadPoint(bool swap)
        {
            Point3F ret = new Point3F();
            ret.X = ReadFloat(swap);
            ret.Y = ReadFloat(swap);
            ret.Z = ReadFloat(swap);
            return ret;
        }
        #endregion
        #region Numeric
        #region CUInt32
        public uint ReadCUInt32()
        {
            byte code = ReadByte();
            switch (code & 0xE0)
            {
                case 0xE0:
                    return BitConverter.ToUInt32(ReadArray(4, true), 0);
                case 0xC0:
                    byte[] bt = ReadArray(3, true);
                    return BitConverter.ToUInt32(new byte[] { bt[2], bt[1], bt[0], code }, 0) & 0x1FFFFFFF;
                case 0x80:
                case 0xA0:
                    return (uint)(BitConverter.ToUInt16(new byte[] { ReadByte(), code }, 0) & 0x3FFF);
            }
            return (uint)code;
        }

        public DataStream WriteCUInt32(uint value)
        {
            if (value <= 0x7F)
            {
                WriteByte((byte)value);
                return this;
            }
            if (value <= 0x3FFF)
            {
                byte[] bt = BitConverter.GetBytes((ushort)(value + 0x8000));
                WriteArray(bt, true);
                return this;
            }
            if (value <= 0x1FFFFFFF)
            {
                byte[] bt = BitConverter.GetBytes((uint)(value + 0xC0000000));
                WriteArray(bt, true);
                return this;
            }
            if (value <= 0xFFFFFFFF)
            {
                List<byte> bt = new List<byte>();
                bt.Add(0xE0);
                byte[] arrbt = BitConverter.GetBytes((uint)value);
                bt.AddRange(arrbt);
                WriteArray(bt.ToArray(), true);
                return this;
            }
            return this;
        }
        #endregion
        #region Float
        public DataStream WriteFloat(float value, bool swap)
        {
            return WriteArray(BitConverter.GetBytes(value), swap);
        }
        public float ReadFloat(bool swap)
        {
            byte[] bt = ReadArray(4, swap);
            return BitConverter.ToSingle(bt, 0);
        }

        public DataStream WriteFloat(float value)
        {
            return WriteFloat(value, Swaped);
        }
        public float ReadFloat()
        {
            return ReadFloat(Swaped);
        }
        #endregion
        #region Int32
        public int ReadInt32()
        {
            return ReadInt32(Swaped);
        }
        public int ReadInt32(bool swap)
        {
            return BitConverter.ToInt32(ReadArray(4, swap), 0);
        }

        public DataStream WriteInt32(int value)
        {
            return WriteInt32(value, Swaped);
        }
        public DataStream WriteInt32(int value, bool swap)
        {
            return WriteArray(BitConverter.GetBytes(value), swap);
        }
        #endregion
        #region Int16
        public int ReadInt16()
        {
            return ReadInt16(Swaped);
        }
        public short ReadInt16(bool swap)
        {
            return BitConverter.ToInt16(ReadArray(2, swap), 0);
        }

        public DataStream WriteInt16(short value)
        {
            return WriteInt16(value, Swaped);
        }
        public DataStream WriteInt16(short value, bool swap)
        {
            return WriteArray(BitConverter.GetBytes(value), swap);
        }
        #endregion
        #region Dword
        public DataStream WriteDword(uint value)
        {
            return WriteArray(BitConverter.GetBytes(value), Swaped);
        }
        public DataStream WriteDword(uint value, bool swap)
        {
            return WriteArray(BitConverter.GetBytes(value), swap);
        }
        public uint ReadDword()
        {
            return BitConverter.ToUInt32(ReadArray(4, Swaped), 0);
        }
        public uint ReadDword(bool swap)
        {
            return BitConverter.ToUInt32(ReadArray(4, swap), 0);
        }
        #endregion
        #region Word
        public DataStream WriteWord(ushort value)
        {
            return WriteArray(BitConverter.GetBytes(value), Swaped);
        }
        public DataStream WriteWord(ushort value, bool swap)
        {
            return WriteArray(BitConverter.GetBytes(value), swap);
        }
        public ushort ReadWord()
        {
            return BitConverter.ToUInt16(ReadArray(2, Swaped), 0);
        }
        public ushort ReadWord(bool swap)
        {
            return BitConverter.ToUInt16(ReadArray(2, swap), 0);
        }
        #endregion
        #endregion
        #region Boolean
        public bool ReadBoolean()
        {
            return ReadByte() == 1;
        }
        public DataStream WriteBoolean(bool v)
        {
            return WriteByte((byte)(v ? 1 : 0));
        }
        #endregion
        #region Bytes
        #region Array
        public DataStream WriteArray(int length)
        {
            return WriteArray(new byte[length], false);
        }
        public DataStream WriteArray(byte[] buf)
        {
            return WriteArray(buf, Swaped);
        }
        public DataStream WriteArray(byte[] buf, bool swap)
        {
            if (swap) Array.Reverse(buf);
            buffer.AddRange(buf);
            return this;
        }

        public byte[] ReadArray(int length)
        { return ReadArray(length, Swaped); }
        public byte[] ReadArray(int length, bool swap)
        {
            byte[] ret = new byte[length];
            for (int i = 0; i < length; i++)
                ret[i] = ReadByte();
            if (swap) Array.Reverse(ret);
            return ret;
        }
        #endregion
        #region Byte
        public DataStream WriteByte()
        {
            return WriteByte(0x00);
        }
        public DataStream WriteByte(byte bt)
        {
            buffer.Add(bt);
            return this;
        }
        public byte ReadByte()
        {
            if (shift >= buffer.Count)
            {
                throw new IndexOutOfRangeException();
            }
            byte ret = buffer[shift];
            shift++;
            return ret;
        }
        #endregion
        #endregion
    }
}
