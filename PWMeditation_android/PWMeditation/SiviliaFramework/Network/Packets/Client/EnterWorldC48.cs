using SiviliaFramework.IO;
using SiviliaFramework.Data;

namespace SiviliaFramework.Network.Packets.Client
{
    [PacketIdentifier(0x48, IO.PacketType.ClientPacket)]
    public class EnterWorldC48 : GamePacket<AccountInformation>
    {
        [FIELD] public uint UID;
        [ARRAY(false)]
        [FIELD] public byte[] Unknown = new byte[20];

        protected internal override void HandleData(AccountInformation data)
        {
            UID = data.SelectedRole.UID;
        }
    }
}
