using SiviliaFramework.IO;
using SiviliaFramework.Data;

namespace SiviliaFramework.Network.Packets.Server
{
    [PacketIdentifier(0x04, PacketType.ServerPacket)]
    public class OnlineAnnounceS04 : GamePacket<AccountInformation>
    {
        [FIELD] public uint AccountID;
        [FIELD] public uint UnkID;

        protected internal override void HandleData(AccountInformation data)
        {
            data.AccountID = AccountID;
        }
    }
}
