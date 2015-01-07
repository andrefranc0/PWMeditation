using System;
using System.Collections.Generic;
using System.Text;

namespace OOGLibrary.IO.PacketBase.Server
{
    class SMKeyS02 : DataStream, IServerPacket
    {
        internal byte[] Key { get; private set; }
        public bool Force { get; private set; }

        public DataStream Deserialize()
        {
            Key = ReadData();
            Force = ReadBoolean();
            return this;
        }
    }
}
