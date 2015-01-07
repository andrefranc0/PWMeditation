using System;
using System.Collections.Generic;
using System.Text;

namespace OOGLibrary.IO.PacketBase.Server
{
    class ErrorInfo : DataStream, IServerPacket
    {
        public byte Type { get; set; }
        public string Message { get; set; }

        public DataStream Deserialize()
        {
            Type = ReadByte();
            Message = ReadAString();
            return this;
        }
    }
}
