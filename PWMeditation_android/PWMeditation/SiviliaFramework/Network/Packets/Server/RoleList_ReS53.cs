using SiviliaFramework.Data;
using SiviliaFramework.IO;

namespace SiviliaFramework.Network.Packets.Server
{
    [PacketIdentifier(0x53, IO.PacketType.ServerPacket)]
    public class RoleList_ReS53 : GamePacket<AccountInformation>
    {
        [FIELD] public int Unk1;
        [FIELD] public int NextSlot;
        [FIELD] public uint AccountID;
        [FIELD] public uint UnkID;
        [FIELD] public bool IsChar;
        [FIELD][IF("IsChar")] public RoleInfo Role;

        protected internal override void HandleData(AccountInformation data)
        {
            if (IsChar)
            {
                data.Roles.Add(Role);
            }
        }
    }
}
