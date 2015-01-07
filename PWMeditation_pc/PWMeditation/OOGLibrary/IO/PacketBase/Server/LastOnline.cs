using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace OOGLibrary.IO.PacketBase.Server
{
    class LastOnlineS8F : DataStream, IServerPacket
    {
        public uint AccountKey { get; private set; }
        public GameTypes.UnixTime LastOnlineDate { get; private set; }
        public IPAddress LastOnlineIP { get; private set; }
        public IPAddress NowIP { get; private set; }

        public DataStream Deserialize()
        {
            Swaped = true;
            AccountKey = ReadDword();
            ReadDword();
            LastOnlineDate = ReadTime();
            LastOnlineIP = new IPAddress(ReadArray(4, false));
            NowIP = new IPAddress(ReadArray(4, false));
            return this;
        }
    }
}
