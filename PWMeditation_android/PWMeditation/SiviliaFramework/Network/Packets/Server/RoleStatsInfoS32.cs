using SiviliaFramework.Data;
using SiviliaFramework.Data.DataTypes;
using SiviliaFramework.IO;

namespace SiviliaFramework.Network.Packets.Server
{
    [PacketIdentifier(0x32, IO.PacketType.ServerContainer)]
    public class RoleStatsInfoS32 : GamePacket<AccountInformation>
    {
        [FIELD] public RoleStatsInformation Stats;

        protected internal override void HandleData(AccountInformation data)
        {
            data.SelectedRole.Stats = Stats;
        }
    }
}
