using System;
using System.Collections.Generic;
using System.Text;
using SiviliaFramework.IO;


namespace SiviliaFramework.Network.Packets.Server
{
    [PacketIdentifier(0x85, IO.PacketType.ServerPacket)]
    public class WorldMessageS85 : GamePacket
    {
        [FIELD] public byte Type;
        [FIELD] public byte SubType;
        [FIELD] public uint Uid;
        [FIELD] public string Name;
        [FIELD] public string Message;
    }
}
