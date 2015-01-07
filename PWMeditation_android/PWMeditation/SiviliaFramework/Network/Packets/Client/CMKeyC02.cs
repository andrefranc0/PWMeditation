using SiviliaFramework.IO;
using SiviliaFramework.Data;

namespace SiviliaFramework.Network.Packets.Client
{
    [PacketIdentifier(0x02, IO.PacketType.ClientPacket)]
    public class CMKeyC02 : GamePacket<ConnectionStatus>
    {
        [FIELD] public byte[] CMKey;
        [FIELD] public bool Force;

        public CMKeyC02()
        {
        }
        public CMKeyC02(bool force)
        {
            Force = force;
        }

        protected internal override void HandleData(ConnectionStatus data)
        {
            CMKey = data.CMKey;
        }
    }
}
