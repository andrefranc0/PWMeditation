using System;
using System.Collections.Generic;
using System.Text;
using SiviliaFramework.IO;

namespace SiviliaFramework.Network.Packets.Server
{
    [PacketIdentifier(0x7B, IO.PacketType.ServerPacket)]
    public class AccountBlockedS7B : GamePacket
    {
        [FIELD] public byte Type;
        [SKIP(8)]
        [FIELD] public uint Seconds;
        [SKIP(4)]
        [FIELD] public string Message;
    }
}
