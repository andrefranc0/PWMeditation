using SiviliaFramework.IO;
using SiviliaFramework.Data;

namespace SiviliaFramework.Network.Packets.Client
{
    [PacketIdentifier(0x52, IO.PacketType.ClientPacket)]
    public class RoleListC52 : GamePacket<AccountInformation>
    {
        public RoleListC52(int slot)
        {
            Slot = slot;
        }
        [FIELD] public uint AccountID;
        [FIELD] public int Unk;
        [FIELD] public int Slot;

        protected internal override void HandleData(AccountInformation data)
        {
            AccountID = data.AccountID;
        }
    }
}
