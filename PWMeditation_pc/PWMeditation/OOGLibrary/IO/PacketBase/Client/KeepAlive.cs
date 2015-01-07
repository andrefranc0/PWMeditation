using System;
using System.Collections.Generic;
using System.Text;

namespace OOGLibrary.IO.PacketBase.Client
{
    class KeepAliveC5A : DataStream, IClientPacket
    {
        public DataStream Serialize()
        {
            Type = 0x5A;
            Write((byte)0x5A);
            return this;
        }
    }
}
