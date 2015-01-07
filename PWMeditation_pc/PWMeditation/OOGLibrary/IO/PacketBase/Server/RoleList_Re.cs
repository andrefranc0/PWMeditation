using System;
using System.Collections.Generic;
using System.Text;

namespace OOGLibrary.IO.PacketBase.Server
{
    class RoleList_ReS53 : DataStream, IServerPacket
    {
        public int NextSlot { get; private set; }
        public uint AccountKey { get; private set; }
        public bool IsChar { get; private set; }

        public uint UID { get; private set; }
        public byte Gender { get; private set; }
        public byte Race { get; private set; }
        public byte Occupation { get; private set; }
        public uint Level { get; private set; }
        public uint SecondLevel { get; private set; }
        public string Name { get; private set; }
        public List<uint> EquipList { get; private set; }
        public GameTypes.UnixTime DeleteTime { get; private set; }
        public GameTypes.UnixTime CreateTime { get; private set; }
        public GameTypes.UnixTime LastOnline { get; private set; }
        public GameTypes.Point3F WorldPos { get; private set; }
        public uint WorldID { get; private set; }

        public DataStream Deserialize()
        {
            Swaped = true;
            Skip(4);
            NextSlot = ReadInt32();
            AccountKey = ReadDword();
            Skip(4);
            IsChar = ReadBoolean();
            if (IsChar)
            {
                UID = ReadDword();
                Gender = ReadByte();
                Race = ReadByte();
                Occupation = ReadByte();
                Level = ReadDword();
                SecondLevel = ReadDword();
                Name = ReadUString();
                ReadData();
                uint EquipCount = ReadCUInt32();
                EquipList = new List<uint>();
                for (uint i = 0; i < EquipCount; i++)
                {
                    EquipList.Add(ReadDword());
                    Skip(12);
                    ReadData();
                    Skip(20);
                }
                ReadByte();
                DeleteTime = ReadTime();
                CreateTime = ReadTime();
                LastOnline = ReadTime();
                WorldPos = ReadPoint();
                WorldID = ReadDword();
            }
            return this;
        }
    }
}
