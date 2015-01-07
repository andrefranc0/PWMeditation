using System;
using System.Collections.Generic;
using System.Text;
using SiviliaFramework.IO;
using SiviliaFramework.Data;

namespace SiviliaFramework.Network.Packets.Server
{
    [PacketIdentifier(0x04, IO.PacketType.ServerContainer)]
    public class RoleListS04 : IGamePacket, IHandleData
    {
        [FIELD] public ushort Count;
        [FIELD][ARRAY("Count")] public RoleMainStruct[] Roles;

        public void HandleData(ConnectionData data)
        {
            WorldInformation world = data.WorldInformation;
            foreach (RoleMainStruct role in Roles)
            {
                if (world.RoleList.ContainsKey(role.RoleId))
                {
                    world.RoleList[role.RoleId] = role;
                }
                else
                {
                    world.RoleList.Add(role.RoleId, role);
                }
            }
        }
    }
}
