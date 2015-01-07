using System;
using System.Collections.Generic;
using System.Text;
using SiviliaFramework.IO;

namespace SiviliaFramework.Network.Packets.Server
{
    [PacketIdentifier(0x60, PacketType.ServerPacket)]
    public class PrivateMessageS60 : GamePacket
    {
        [FIELD] public byte MessageType;
        [FIELD] public byte Unk1;
        [FIELD] public string NameSend;
        [FIELD] public uint UidSend;
        [FIELD] public string Name;
        [FIELD] public uint Uid1;
        [FIELD] public string Message;
    }
}
