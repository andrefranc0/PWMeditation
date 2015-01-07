using System;
using System.Collections.Generic;
using System.Text;

namespace OOGLibrary.IO.PacketBase.Client
{
    class RoleListC52 : DataStream, IClientPacket
    {
        public uint AccontKey { get; private set; }
        public int Slot { get; private set; }
        public RoleListC52(Server.OnlineAnnounceS04 s04)
        {
            AccontKey = s04.AccountKey;
            Slot = -1;
        }
        public RoleListC52(Server.RoleList_ReS53 s53)
        {
            AccontKey = s53.AccountKey;
            Slot = s53.NextSlot;
        }
        public DataStream Serialize()
        {
            Type = 0x52;
            Swaped = true;
            Write(AccontKey).Write(0).Write(Slot);
            return this;
        }
    }
}
