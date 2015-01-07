using System;
using System.Collections.Generic;
using System.Text;
using SiviliaFramework.IO;
using SiviliaFramework.Data;

namespace SiviliaFramework.Network.Packets.Server
{
    [PacketIdentifier(0x0D, IO.PacketType.ServerContainer)]
    public class ObjectLeaveSliceS0D : IGamePacket, IHandleData
    {
        [FIELD] public uint ObjectId;

        public void HandleData(ConnectionData data)
        {
            WorldInformation world = data.WorldInformation;

            if (world.RoleList.ContainsKey(ObjectId))   world.RoleList.Remove(ObjectId);
        }
    }
}
