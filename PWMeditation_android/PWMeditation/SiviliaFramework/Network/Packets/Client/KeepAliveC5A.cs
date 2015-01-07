using SiviliaFramework.IO;

namespace SiviliaFramework.Network.Packets.Client
{
    [PacketIdentifier(0x5A, IO.PacketType.ClientPacket)]
    public class KeepAliveC5A : GamePacket
    {
        [FIELD] public byte KeepAlive = 0x5A;
    }
}
