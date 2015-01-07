using System;
using System.Collections.Generic;
using System.Text;

namespace OOGLibrary.IO.PacketBase.Server
{
    class OnlineAnnounceS04 : DataStream, IServerPacket
    {
        public uint AccountKey { get; private set; }

        public DataStream Deserialize()
        {
            Swaped = true;
            AccountKey = ReadDword();
            return this;
        }
    }
}
