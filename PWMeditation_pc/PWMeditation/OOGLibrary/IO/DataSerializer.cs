using System;
using System.Collections.Generic;
using System.Text;
using OOGLibrary.IO.PacketBase.Client;
using OOGLibrary.IO.PacketBase.Server;

namespace OOGLibrary.IO
{
    class DataSerializer
    {
        public DataStream Deserialize(DataStream ds)
        {
            DataStream dsret;
            switch (ds.Type)
            {
                case 0x00: dsret = new ContainerS00(); break;
                case 0x01: dsret = new ServerInfoS01(); break;
                case 0x02: dsret = new SMKeyS02(); break;
                case 0x04: dsret = new OnlineAnnounceS04(); break;
                case 0x05: dsret = new ErrorInfo(); break;
                case 0x47: dsret = new SelectRole_ReS47(); break;
                case 0x53: dsret = new RoleList_ReS53(); break;
                case 0x60: dsret = new PrivateMessageS60(); break;
                case 0x8F: dsret = new LastOnlineS8F(); break;
                default: return ds;
            }
            dsret.Type = ds.Type;
            dsret.Buffer = ds.Buffer;
            dsret.Swaped = true;
            return ((PacketBase.IServerPacket)dsret).Deserialize();
        }
        public DataStream DeserializeContainer(DataStream ds)
        {
            DataStream dsret;
            switch (ds.Type)
            {
                case 0x2B: dsret = new InventoryInfoS2B(); break;
                case 0x26: dsret = new RoleInfoUpdateS26(); break;
                case 0x52: dsret = new MoneyInfoS52(); break;
                case 0x149: dsret = new MeditationInfoS149(); break;
                case 0x14A: dsret = new MeditationEnabledS14A(); break;
                default: return ds;
            }
            dsret.Type = ds.Type;
            dsret.Buffer = ds.Buffer;
            return ((PacketBase.IServerContainer)dsret).Deserialize();
        }
        public DataStream Serialize(DataStream ds)
        {
            if (ds is PacketBase.IClientPacket)
            {
                ds.Swaped = true;
                return ((PacketBase.IClientPacket)ds).Serialize();
            }
            if (ds is PacketBase.IClientContainer)
                return SerializeContainer(((PacketBase.IClientContainer)ds).Serialize());
            if (ds is PacketBase.IClientContainerC25)
                return SerializeContainerC25(((PacketBase.IClientContainerC25)ds).Serialize());
            return ds;
        }
        private DataStream SerializeContainer(DataStream ds)
        {
            DataStream ret = new DataStream(0x22);
            ret.WriteCUInt32((uint)ds.Length + 2).
                Write((ushort)ds.Type).
                Write(ds.Buffer);
            return ret;
        }
        private DataStream SerializeContainerC25(DataStream ds)
        {
            DataStream ret = new DataStream(0x25);
            ret.Write(ds.Type).
                Write(ds.Length).
                Write(ds.Buffer);
            return SerializeContainer(ret);
        }
    }
}
