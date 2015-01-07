using System;
using System.Collections.Generic;
using System.Text;

namespace OOGLibrary.IO.PacketBase.Server
{
    class InventoryInfoS2B : DataStream, IServerContainer
    {
        public byte Type { get; private set; }
        public byte SlotsCount { get; private set; }
        public int ItemsCount { get; private set; }
        public InventoryItem[] Items { get; private set; }

        public DataStream Deserialize()
        {
            Type = ReadByte();
            SlotsCount = ReadByte();
            ReadInt32();
            ItemsCount = ReadInt32();
            Items = new InventoryItem[ItemsCount];
            for (int i = 0; i < Items.Length; i++)
            {
                Items[i] = new InventoryItem();
                Items[i].Deserialize(this);
            }
            return this;
        }
    }
    class InventoryItem
    {
        public int Slot { get; private set; }
        public int ID { get; private set; }
        public int Count { get; private set; }

        public DataStream Deserialize(DataStream ds)
        {
            Slot = ds.ReadInt32();
            ID = ds.ReadInt32();
            ds.Skip(8);
            Count = ds.ReadInt32();
            ds.Skip(2);
            ds.Skip(ds.ReadInt16());
            return ds;
        }
    }
}
