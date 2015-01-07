using SiviliaFramework.Data;
using SiviliaFramework.IO;

namespace SiviliaFramework.Network.Packets.Client
{
    [PacketIdentifier(0x46, IO.PacketType.ClientPacket)]
    public class SelectRoleC46 : GamePacket<AccountInformation>
    {
        public SelectRoleC46(int roleSlot)
        {
            Slot = roleSlot;
        }
        public int Slot;
        [FIELD] public uint UID;
        [FIELD] public byte Unk;

        protected internal override void HandleData(AccountInformation data)
        {
            RoleInfo roleInfo = data.Roles[Slot];

            UID = roleInfo.UID;
            data.SelectedRole = roleInfo;
        }
    }
}
