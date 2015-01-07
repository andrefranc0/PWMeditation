using System;
using System.Collections.Generic;
using System.Text;

namespace OOGLibrary.IO.PacketBase.Client
{
    class EnterWorldC48 : DataStream, IClientPacket
    {
        public uint UID { get; private set; }
        public EnterWorldC48(uint uid)
        {
            UID = uid;
        }
        public DataStream Serialize()
        {
            Type = 0x48;
            WriteDword(UID, true);
            WriteArray(20);
            return this;
        }
    }
}
