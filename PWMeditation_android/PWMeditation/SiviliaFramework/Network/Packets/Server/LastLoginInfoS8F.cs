using System;
using System.Collections.Generic;
using System.Text;
using SiviliaFramework.IO;
using SiviliaFramework.Data;
using SiviliaFramework.IO.GameTypes;

namespace SiviliaFramework.Network.Packets.Server
{
    [PacketIdentifier(0x8F, PacketType.ServerPacket)]
    public class LastLoginInfoS8F : GamePacket<AccountInformation> 
    {
        [FIELD] public uint AccountID;
        [FIELD] public uint UnkID;
        [FIELD] public UnixTime LastLoginTime;
        [FIELD][ARRAY(4)] public byte[] LastLoginIP;
        [FIELD][ARRAY(4)] public byte[] CurrentIP;

        protected internal override void HandleData(AccountInformation data)
        {
            data.LastLoginTime = LastLoginTime;
            data.LastLoginIP = IpToString(LastLoginIP);
            data.CurrentIP = IpToString(CurrentIP);
        }
        private string IpToString(byte[] ip)
        {
            return String.Format("{0}.{1}.{2}.{3}", ip[3], ip[2], ip[1], ip[0]);
        }
    }
}
