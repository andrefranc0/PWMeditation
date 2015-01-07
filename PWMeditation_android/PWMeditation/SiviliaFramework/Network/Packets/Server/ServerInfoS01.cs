using SiviliaFramework.IO;
using SiviliaFramework.Data;

namespace SiviliaFramework.Network.Packets.Server
{
    [PacketIdentifier(0x01, IO.PacketType.ServerPacket)]
    public class ServerInfoS01 : GamePacket<ConnectionStatus>
    {
        [FIELD] public byte[] Key;
        [FIELD][ARRAY(4)] public byte[] ServerVersion;
        [FIELD] public bool HashToSha256;
        [FIELD(FieldType.AString)] public string CRC;
        [FIELD] public byte Raith;

        protected internal override void HandleData(ConnectionStatus data)
        {
            float serverStatus = (float)(Key[0] * 100) / 255;

            data.ServerStatus = serverStatus;
            data.ServerHash = Key;
        }
    }
}