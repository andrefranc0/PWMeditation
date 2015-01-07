using SiviliaFramework.IO;

namespace SiviliaFramework.Network.Packets.Server
{
    [PacketIdentifier(0x5A, IO.PacketType.ServerPacket)]
    public class KeepAliveS5A : GamePacket
    {
        [FIELD] public byte KeepAlive;
    }
}
