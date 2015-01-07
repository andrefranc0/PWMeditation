using SiviliaFramework.IO;

namespace SiviliaFramework.Network.Packets.Server
{
    [PacketIdentifier(0x47, IO.PacketType.ServerPacket)]
    public class SelectRole_ReS47 : GamePacket
    {
        [FIELD] public bool Unk1;
        [FIELD] public uint Unk2;
    }
}
