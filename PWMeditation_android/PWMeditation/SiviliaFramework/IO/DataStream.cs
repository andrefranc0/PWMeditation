using SiviliaFramework.IO.GameTypes;
using System;
using System.Text;

namespace SiviliaFramework.IO
{
    public class DataStream
    {
        public DataStream()
        {
        }
        public DataStream(byte[] buffer) : this(buffer, false)
        {
        }
        public DataStream(bool swaped)
        {
            IsSwaped = swaped;
        }
        public DataStream(byte[] buffer, bool swaped)
        {
            WriteBytes(buffer);
            IsSwaped = swaped;
        }

        public bool IsSwaped { get; set; }
        private bool SwapValue { get; set; }

        int pos;
        int count;
        byte[] buffer;

        public int Position
        {
            get { return pos; }
            set
            {
                if (pos > count)
                    throw new Exception("pos >= count");
                pos = value;
            }
        }
        public int Count
        {
            get { return count; }
            private set { count = value; }
        }
        public byte[] Buffer
        {
            get { return buffer; }
            set 
            {
                buffer = value; 
            }
        }

        // Округляет размер до степени двойки (16, 32, 64, 128, 256, 512 etc)
        private static int Roundup(int length)
        {
            var i = 16;

            while (length > i)
            {
                i <<= 1;
            }

            return i;
        }

        public void Reserve(int count)
        {
            if (buffer == null) buffer = new byte[Roundup(count)];
            if (count > buffer.Length)
            {
                Array.Resize(ref buffer, Roundup(count));
            }
        }
        public bool CanRead()
        {
            return CanRead(1);
        }
        public bool CanRead(int length)
        {
            return pos + length <= count;
        }
        public void Skip(int count)
        {
            if (pos + count > Count)
                throw new Exception("pos + count > Count");

            pos += count;
        }
        public void Flush()
        {
            if (pos == 0) return;

            int len = count - pos;

            System.Buffer.BlockCopy(buffer, pos, buffer, 0, len);

            Position = 0;
            Count = len;
        }
        public void Reset()
        {
            pos = 0;
        }
        public void Clear()
        {
            pos = 0;
            count = 0;
        }

        public void SaveSwap()
        {
            SwapValue = IsSwaped;
        }
        public void LoadSwap()
        {
            IsSwaped = SwapValue;
        }

        public byte[] GetBytes()
        {
            byte[] ret = new byte[Count];
            System.Buffer.BlockCopy(Buffer, 0, ret, 0, ret.Length);

            return ret;
        }

        // WRITE
        public DataStream Write(object data)
        {
            if (data is byte[]) return WriteBytes((byte[])data);

            return Write(data, IsSwaped, FieldType.Other);
        }
        public DataStream Write(object data, FieldType fieldType)
        {
            return Write(data, IsSwaped, fieldType);
        }
        public DataStream Write(object data, bool swaped)
        {
            return Write(data, swaped, FieldType.Other);
        }
        public DataStream Write(object data, bool swaped, FieldType type)
        {
            switch (type)
            {
                case FieldType.AString: return WriteAString((string)data);
                case FieldType.CUInt32: return WriteCUInt32((uint)data);
                case FieldType.DwordAString: return WriteAString((string)data, true);
                case FieldType.DwordUString: return WriteUString((string)data, true);
            }

            if (data is DataStream) return WriteDataStream((DataStream)data);
            if (data is string)     return WriteUString((string)data);
            if (data is uint)       return WriteDword((uint)data, swaped);
            if (data is ushort)     return WriteWord((ushort)data, swaped);
            if (data is int)        return WriteInt32((int)data, swaped);
            if (data is short)      return WriteInt16((short)data, swaped);
            if (data is float)      return WriteFloat((float)data, swaped);
            if (data is UnixTime)   return WriteTime((UnixTime)data, swaped);
            if (data is Point3F)    return WritePoint((Point3F)data, swaped);
            if (data is byte[])     return WriteBytes((byte[])data, swaped);
            if (data is byte)       return WriteByte((byte)data);
            if (data is bool)       return WriteBoolean((bool)data);

            DataStreamSerializer.Serialize(data, this);

            return this;
        }

        // READ
        public T Read<T>()
        {
            return Read<T>(IsSwaped);
        }
        public T Read<T>(bool swaped)
        {
            T data;
            Read<T>(out data, swaped);
            return data;
        }
        public T Read<T>(FieldType fieldType)
        {
            T data;
            Read<T>(out data, fieldType);
            return data;
        }
        public DataStream Read<T>(out T data)
        {
            return Read<T>(out data, IsSwaped);
        }
        public DataStream Read<T>(out T data, bool swaped)
        {
            object obj;
            Read(out obj, typeof(T), FieldType.Other, swaped);

            data = (T)obj;
            return this;
        }
        public DataStream Read<T>(out T data, FieldType fieldType)
        {
            object obj;
            Read(out obj, typeof(T), fieldType, false);

            data = (T)obj;
            return this;
        }
        public DataStream Read(out object data, Type type, FieldType fieldType, bool swaped)
        {
            switch(fieldType)
            {
                case FieldType.AString: data = ReadAString();          return this;
                case FieldType.CUInt32: data = ReadCUInt32();          return this;
                case FieldType.DwordAString: data = ReadAString(true); return this;
                case FieldType.DwordUString: data = ReadUString(true); return this;
            }
            switch (type.Name)
            {
                case "DataStream": data = ReadDataStream();  return this;
                case "String":     data = ReadUString();     return this;
                case "Single":     data = ReadFloat(swaped); return this;
                case "Int32":      data = ReadInt32(swaped); return this;
                case "Int16":      data = ReadInt16(swaped); return this;
                case "UInt32":     data = ReadDword(swaped); return this;
                case "UInt16":     data = ReadWord(swaped);  return this;
                case "Byte":       data = ReadByte();        return this;
                case "Boolean":    data = ReadBoolean();     return this;
                default:           data = DataStreamSerializer.Deserialize(this, swaped, type); return this;
            }
        }

        // ARRAY
        public DataStream WriteArray(Array array)
        {
            return WriteArray(array, false, IsSwaped);
        }
        public DataStream WriteArray(Array array, bool lenToInt)
        {
            return WriteArray(array, lenToInt, IsSwaped);
        }
        public DataStream WriteArray(Array array, bool lenToInt, bool swaped)
        {
            if (lenToInt) WriteInt32(array.Length);
            else WriteCUInt32((uint)array.Length);

            foreach (object obj in array)
            {
                Write(obj, swaped);
            }

            return this;
        }

        // TEXT
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

        // TEXT //dword
        public string ReadUString(bool int32)
        {
            return Encoding.Unicode.GetString(ReadData(int32));
        }
        public DataStream WriteUString(string text, bool int32)
        {
            return WriteData(Encoding.Unicode.GetBytes(text), int32);
        }

        public string ReadAString(bool int32)
        {
            return Encoding.ASCII.GetString(ReadData(int32));
        }
        public DataStream WriteAString(string text, bool int32)
        {
            return WriteData(Encoding.ASCII.GetBytes(text), int32);
        }

        // DATASTREAM
        public DataStream ReadDataStream()
        {
            return new DataStream(ReadData(false));
        }
        public DataStream ReadDataStream(bool int32)
        {
            return new DataStream(ReadData(int32));
        }
        public DataStream WriteDataStream(DataStream ds)
        {
            return WriteData(ds.GetBytes(), false);
        }
        public DataStream WriteDataStream(DataStream ds, bool int32)
        {
            return WriteData(ds.GetBytes(), int32);
        }
        
        // DATA
        public byte[] ReadData()
        {
            return ReadData(false);
        }
        public byte[] ReadData(bool int32)
        {
            int length;
            length = int32 ? ReadInt32(false) : (int)ReadCUInt32();
            return ReadBytes(length, false);
        }
        public DataStream WriteData(byte[] bytes)
        {
            return WriteData(bytes, false);
        }
        public DataStream WriteData(byte[] bytes, bool int32)
        {
            if (int32) WriteInt32(bytes.Length, false);
            else WriteCUInt32((uint)bytes.Length);

            WriteBytes(bytes, false);

            return this;
        }

        // UNIXTIME
        public UnixTime ReadTime()
        {
            return ReadTime(IsSwaped);
        }
        public UnixTime ReadTime(bool swaped)
        {
            int value = ReadInt32(swaped);
            return new UnixTime(value);
        }
        public DataStream WriteTime(UnixTime time)
        {
            return WriteTime(time, IsSwaped);
        }
        public DataStream WriteTime(UnixTime time, bool swaped)
        {
            WriteInt32(time.Timestamp, swaped);
            return this;
        }

        // POINT
        public Point3F ReadPoint()
        {
            return ReadPoint(IsSwaped);
        }
        public Point3F ReadPoint(bool swaped)
        {
            Point3F ret = new Point3F();
            ret.X = ReadFloat(swaped);
            ret.Z = ReadFloat(swaped);
            ret.Y = ReadFloat(swaped);
            return ret;
        }
        public DataStream WritePoint(Point3F point)
        {
            return WritePoint(point, IsSwaped);
        }
        public DataStream WritePoint(Point3F point, bool swaped)
        {
            WriteFloat(point.X, swaped);
            WriteFloat(point.Z, swaped);
            WriteFloat(point.Y, swaped);

            return this;
        }

        // CUINT32
        public bool TryReadCUInt32(out uint value)
        {
            value = 0;

            if (!CanRead()) return false;

            switch (buffer[pos] & 0xE0)
            {
                case 0xE0:
                    if (!CanRead(5))
                        return false;

                    ReadByte();

                    value = ReadDword(true);

                    break;
                case 0xC0:
                    if (!CanRead(4))
                        return false;

                    value = ReadDword(true) & 0x3FFFFFFF;
                    break;
                case 0x80:
                case 0xA0:
                    if (!CanRead(2))
                        return false;

                    value = (uint)ReadWord(true) & 0x7FFF;
                    break;
                default:
                    value = ReadByte();
                    break;
            }

            return true;
        }
        public uint ReadCUInt32()
        {
            if (!CanRead())
                throw new Exception("!CanRead()");
            switch (buffer[pos] & 0xE0)
            {
                case 0xE0:
                    ReadByte();
                    return ReadDword(true);
                case 0xC0:
                    return ReadDword(true) & 0x3FFFFFFF;
                case 0x80:
                case 0xA0:
                    return (uint)ReadWord(true) & 0x7FFF;
                default:
                    return ReadByte();
            }

        }
        public DataStream WriteCUInt32(uint value)
        {
            if (value < 0x80)
                return WriteByte((byte)value);

            if (value < 0x4000)
                return WriteWord((ushort)(value | 0x8000), true);

            if (value < 0x20000000)
                return WriteDword(value | 0xC0000000, true);

            WriteByte(0xE0);

            return WriteDword(value, true);
        }

        // FLOAT
        public float ReadFloat()
        {
            return ReadFloat(IsSwaped);
        }
        public float ReadFloat(bool swaped)
        {
            byte[] bt = ReadBytes(4, swaped);
            return BitConverter.ToSingle(bt, 0);
        }
        public DataStream WriteFloat(float value)
        {
            return WriteFloat(value, IsSwaped);
        }
        public DataStream WriteFloat(float value, bool swaped)
        {
            return WriteBytes(BitConverter.GetBytes(value), swaped);
        }

        // INT32
        public int ReadInt32()
        {
            return ReadInt32(IsSwaped);
        }
        public int ReadInt32(bool swaped)
        {
            return BitConverter.ToInt32(ReadBytes(4, swaped), 0);
        }
        public DataStream WriteInt32(int value)
        {
            return WriteInt32(value, IsSwaped);
        }
        public DataStream WriteInt32(int value, bool swaped)
        {
            return WriteBytes(BitConverter.GetBytes(value), swaped);
        }

        // INT16
        public short ReadInt16()
        {
            return ReadInt16(IsSwaped);
        }
        public short ReadInt16(bool swaped)
        {
            return BitConverter.ToInt16(ReadBytes(2, swaped), 0);
        }
        public DataStream WriteInt16(short value)
        {
            return WriteInt16(value, IsSwaped);
        }
        public DataStream WriteInt16(short value, bool swaped)
        {
            return WriteBytes(BitConverter.GetBytes(value), swaped);
        }

        // DWORD
        public uint ReadDword()
        {
            return BitConverter.ToUInt32(ReadBytes(4, IsSwaped), 0);
        }
        public uint ReadDword(bool swaped)
        {
            return BitConverter.ToUInt32(ReadBytes(4, swaped), 0);
        }
        public DataStream WriteDword(uint value)
        {
            return WriteBytes(BitConverter.GetBytes(value), IsSwaped);
        }
        public DataStream WriteDword(uint value, bool swaped)
        {
            return WriteBytes(BitConverter.GetBytes(value), swaped);
        }

        // WORD
        public ushort ReadWord()
        {
            return BitConverter.ToUInt16(ReadBytes(2, IsSwaped), 0);
        }
        public ushort ReadWord(bool swaped)
        {
            return BitConverter.ToUInt16(ReadBytes(2, swaped), 0);
        }
        public DataStream WriteWord(ushort value)
        {
            return WriteBytes(BitConverter.GetBytes(value), IsSwaped);
        }
        public DataStream WriteWord(ushort value, bool swaped)
        {
            return WriteBytes(BitConverter.GetBytes(value), swaped);
        }

        // BOOLEAN
        public bool ReadBoolean()
        {
            return ReadByte() != 0;
        }
        public DataStream WriteBoolean(bool b)
        {
            return WriteByte(b ? (byte)0x01 : (byte)0x00);
        }

        // BYTES
        public byte[] ReadBytes(int len)
        {
            return ReadBytes(len, false);
        }
        public byte[] ReadBytes(int len, bool swaped)
        {
            if (pos + len > count)
                throw new Exception("pos + len > count");

            byte[] ret = new byte[len];
            System.Buffer.BlockCopy(buffer, pos, ret, 0, len);
            pos += len;

            if (swaped) Array.Reverse(ret);

            return ret;
        }
        public DataStream WriteBytes(int len)
        {
            Reserve(count + len);
            count += len;

            return this;
        }
        public DataStream WriteBytes(byte[] bytes)
        {
            return WriteBytes(bytes, 0, bytes.Length, false);
        }
        public DataStream WriteBytes(byte[] bytes, bool swaped)
        {
            return WriteBytes(bytes, 0, bytes.Length, swaped);
        }
        public DataStream WriteBytes(byte[] bytes, int len)
        {
            return WriteBytes(bytes, 0, len, false);
        }
        public DataStream WriteBytes(byte[] bytes, int pos, int len)
        {
            return WriteBytes(bytes, pos, len, false);
        }
        public DataStream WriteBytes(byte[] bytes, int pos, int len, bool swaped)
        {
            if (swaped)
                Array.Reverse(bytes);

            Reserve(count + len);
            System.Buffer.BlockCopy(bytes, pos, buffer, count, len);
            count += bytes.Length;

            return this;
        }

        // BYTE
        public byte ReadByte()
        {
            if (pos >= count)
                throw new Exception("pos >= count");

            return buffer[pos++];
        }
        public DataStream WriteByte()
        {
            return WriteByte(0x00);
        }
        public DataStream WriteByte(byte b)
        {
            Reserve(Count + 1);
            buffer[Count++] = b;

            return this;
        }
    }
}
