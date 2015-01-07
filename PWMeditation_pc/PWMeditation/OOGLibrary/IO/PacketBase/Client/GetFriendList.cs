using System;
using System.Collections.Generic;
using System.Text;

namespace OOGLibrary.IO.PacketBase.Client
{
    class GetFriendListCCE : DataStream, IClientPacket
    {
        public uint UID { get; set; }
        public GetFriendListCCE(uint uid)
        {
            UID = uid;
        }

        public DataStream Serialize()
        {
            Swaped = true;
            Type = 0xCE;
            return Write(UID).Write(0);
        }
    }
}
