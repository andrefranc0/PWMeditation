using System;
using System.Collections.Generic;
using System.Text;
using SiviliaFramework.Data;
using SiviliaFramework.IO;

namespace SiviliaFramework.Network.Packets.Server
{
    [PacketIdentifier(0x26, IO.PacketType.ServerContainer)]
    public class RoleInfoUpdateS26 : GamePacket<AccountInformation>
    {
        [FIELD] public short Level;
        [FIELD] public short Unk;
        [FIELD] public int HP;
        [FIELD] public int HPMax;
        [FIELD] public int MP;
        [FIELD] public int MPMax;
        [FIELD] public int Experience;
        [FIELD] public int Spirit;
        [FIELD] public int Vigor;
        [FIELD] public int VigorMax;

        protected internal override void HandleData(AccountInformation data)
        {
            RoleInfo role = data.SelectedRole;

            role.Level = Level;
            role.Experience = Experience;
            role.Spirit = Spirit;

            role.Stats.HP = HP;
            role.Stats.HPMax = HPMax;
            role.Stats.MP = MP;
            role.Stats.MPMax = MPMax;
        }
    }
}
