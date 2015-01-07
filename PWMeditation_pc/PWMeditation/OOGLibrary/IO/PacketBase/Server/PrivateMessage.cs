using System;
using System.Collections.Generic;
using System.Text;

namespace OOGLibrary.IO.PacketBase.Server
{
    class PrivateMessageS60 : DataStream, IServerPacket
    {
        public byte MessageType { get; set; }

        public string Name { get; set; }
        public uint UID { get; set; }

        public string RecvFromName { get; set; }
        public uint RecvFromUID { get; set; }

        public string Message { get; set; }

        public DataStream Deserialize()
        {
            MessageType = ReadByte();
            Skip(1);
            RecvFromName = ReadUString();
            RecvFromUID = ReadDword();
            Name = ReadUString();
            UID = ReadDword();
            Message = ReadUString();

            return this;
        }
    }
}
