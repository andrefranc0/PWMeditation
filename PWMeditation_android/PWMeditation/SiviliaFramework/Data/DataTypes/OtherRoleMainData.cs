using System;
using System.Collections.Generic;
using System.Text;
using SiviliaFramework.IO;
using SiviliaFramework.IO.GameTypes;

namespace SiviliaFramework.Data.DataTypes
{
    public class OtherRoleMainData :IDeserializableType
    {

        public uint Uid;
        public Point3F Position;
        public byte Angle;

        public uint GuildId;
        public GuildStatus GuildStatus;

        public ushort TitleId;
        public byte RebornsCount;

        public void Deserialize(DataStream ds)
        {
            uint mask1, mask2;

            Uid = ds.ReadDword();
            Position = ds.ReadPoint();
            ds.Skip(4);
            Angle = ds.ReadByte();
            ds.Skip(1);
            mask1 = ds.ReadDword();
            mask2 = ds.ReadDword();

            if ((uint)(mask1 & 0x400) > 0) ds.Skip(8);

            if ((uint)(mask1 & 0x01) > 0) ds.Skip(1);
            if ((uint)(mask1 & 0x02) > 0) ds.Skip(1);
            if ((uint)(mask1 & 0x40) > 0) ds.Skip(16);
            if ((uint)(mask1 & 0x800) > 0) ds.Read<uint>(out GuildId).
                                              Read<GuildStatus>(out GuildStatus);
            if ((uint)(mask1 & 0x1000) > 0) ds.Skip(1);
            if ((uint)(mask1 & 0x10000) > 0) ds.ReadData();

            if ((uint)(mask1 & 0x08) > 0) ds.Skip(1);
            if ((uint)(mask1 & 0x80000) > 0) ds.Skip(6);
            if ((uint)(mask1 & 0x100000) > 0) ds.Skip(5);
            if ((uint)(mask1 & 0x800000) > 0) ds.Skip(4);
            if ((uint)(mask1 & 0x1000000) > 0) ds.Skip(1);
            if ((uint)(mask1 & 0x4000000) > 0) ds.Skip(1);
            if ((uint)(mask1 & 0x8000000) > 0) ds.Skip(1);
            if ((uint)(mask1 & 0x10000000) > 0) ds.Skip(1);
            if ((uint)(mask1 & 0x20000000) > 0) ds.Skip(4);

            if ((uint)(mask2 & 0x02) > 0) ds.Read<ushort>(out TitleId);
            if ((uint)(mask2 & 0x04) > 0) ds.Read<byte>(out RebornsCount);
            if ((uint)(mask2 & 0x08) > 0) ds.Skip(1);
            if ((uint)(mask2 & 0x20) > 0) ds.Skip(1);

        }
    }
}
