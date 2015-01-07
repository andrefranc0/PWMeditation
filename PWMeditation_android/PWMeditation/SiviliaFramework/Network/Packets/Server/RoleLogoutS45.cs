using System;
using System.Collections.Generic;
using System.Text;
using SiviliaFramework.IO;
using SiviliaFramework.Data;

namespace SiviliaFramework.Network.Packets.Server
{
    [PacketIdentifier(0x45, IO.PacketType.ServerPacket)]
    public class RoleLogoutS45 : GamePacket<AccountInformation>
    {
        [SKIP(4)]
        [FIELD] public uint RoleId;
        [SKIP(4)]
        [FIELD] public uint ConnectionId;

        protected internal override void HandleData(AccountInformation data)
        {
            data.Roles.Clear();
        }
    }
}
