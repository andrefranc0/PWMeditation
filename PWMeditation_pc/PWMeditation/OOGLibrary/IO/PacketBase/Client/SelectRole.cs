using System;
using System.Collections.Generic;
using System.Text;

namespace OOGLibrary.IO.PacketBase.Client
{
    class SelectRoleC46 : DataStream, IClientPacket
    {
        public uint UID { get; private set; }
        public SelectRoleC46(uint uid)
        {
            UID = uid;
        }
        public DataStream Serialize()
        {
            Type = 0x46;
            WriteDword(UID, true);
            WriteByte();
            return this;
        }
    }
}
