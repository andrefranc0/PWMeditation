using System;
using System.Collections.Generic;
using System.Text;

namespace OOGLibrary.IO.PacketBase.Server
{
    class ServerInfoS01 : DataStream, IServerPacket
    {
        public byte[] Key { get; private set; }
        public GameTypes.CoreVersion Version { get; private set; }
        public byte AuthType { get; private set; }
        public string CRC { get; private set; }
        public byte Rates { get; private set; }

        public DataStream Deserialize()
        {
            Key = ReadData();
            Version = ReadCoreVersion();
            AuthType = ReadByte();
            CRC = ReadAString();
            Rates = ReadByte();
            return this;
        }
    }
}
