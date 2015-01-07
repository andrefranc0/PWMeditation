using System;
using System.Collections.Generic;
using System.Text;

namespace OOGLibrary.IO.PacketBase.Server
{
    class SelectRole_ReS47 : DataStream, IServerPacket
    {
        public DataStream Deserialize()
        {
            Skip(5);
            return this;
        }
    }
}
