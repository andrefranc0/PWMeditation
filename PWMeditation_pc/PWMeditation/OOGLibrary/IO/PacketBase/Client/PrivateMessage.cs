using System;
using System.Collections.Generic;
using System.Text;

namespace OOGLibrary.IO.PacketBase.Client
{
    class PrivateMessageC60 : DataStream, IClientPacket, ISetName, ISetUID
    {
        public PrivateMessageC60(bool automessage, string name, uint uid, string message)
        {
            AutoMessage = automessage;

            SendToName = name;
            SendToUID = uid;

            Message = message;
        }
        public bool AutoMessage { get; set; }

        public string Name { get; set; }
        public uint UID { get; set; }

        public string SendToName { get; set; }
        public uint SendToUID { get; set; }

        public string Message { get; set; }

        public DataStream Serialize()
        {
            Swaped = true;

            Type = 0x60;

            Write(AutoMessage);
            Write((byte)0);
            Write(Name);
            Write(UID);
            Write(SendToName);
            Write(SendToUID);
            Write(Message);
            Write((byte)0);
            Write(SendToUID);

            return this;
        }
    }
}
