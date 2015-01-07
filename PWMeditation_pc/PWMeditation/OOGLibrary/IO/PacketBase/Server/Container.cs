using System;
using System.Collections.Generic;
using System.Text;

namespace OOGLibrary.IO.PacketBase.Server
{
    class ContainerS00 : DataStream, IServerPacket
    {
        List<DataStream> dslist = new List<DataStream>();
        public int Count { get { return dslist.Count; } }
        public new DataStream this[int index]
        {
            get
            {
                return dslist[index];
            }
        }
        public DataStream Deserialize()
        {
            while (CanRead())
            {
                DataStream ds = new DataStream();
                ReadCUInt32();
                ReadCUInt32();
                DataStream buf = ReadDataStream();
                ds.Type = buf.ReadWord();
                ds.Buffer = buf.ReadArray(buf.Length - buf.Shift);
                dslist.Add(ds);
            }
            return this;
        }
    }
}
