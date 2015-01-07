using System;
using System.Collections.Generic;
using System.Text;
using SiviliaFramework.IO;
using SiviliaFramework.Data;

namespace SiviliaFramework.Network.Packets.Client
{
    [PacketIdentifier(0xCE, IO.PacketType.ClientPacket)]
    public class GetFriendListCCE : GamePacket<AccountInformation>
    {
        [FIELD] public uint UID;
        [FIELD] public uint Unk;

        protected internal override void HandleData(AccountInformation data)
        {
            UID = data.SelectedRole.UID;
        }
    }
}
