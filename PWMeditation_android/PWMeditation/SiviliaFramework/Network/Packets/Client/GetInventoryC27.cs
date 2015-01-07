using System;
using System.Collections.Generic;
using System.Text;
using SiviliaFramework.IO;
using SiviliaFramework.Network;
using SiviliaFramework.Data;

namespace SiviliaFramework.Network.Packets.Client
{
    [PacketIdentifier(0x27, IO.PacketType.ClientContainer)]
    public class GetInventoryC27 : GamePacket
    {
        public GetInventoryC27() : this (true,true,false)
        {
        }
        public GetInventoryC27(bool flag1, bool flag2, bool flag3)
        {
            Flag1 = flag1;
            Flag2 = flag2;
            Flag3 = flag3;
        }

        [FIELD] public bool Flag1;
        [FIELD] public bool Flag2;
        [FIELD] public bool Flag3;
    }
}
