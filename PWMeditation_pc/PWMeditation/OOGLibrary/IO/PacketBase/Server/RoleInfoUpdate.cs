using System;
using System.Collections.Generic;
using System.Text;

namespace OOGLibrary.IO.PacketBase.Server
{
    class RoleInfoUpdateS26 : DataStream, IServerContainer
    {
        public int Level { get; private set; }
        public int HP { get; private set; }
        public int MaxHP { get; private set; }
        public int MP { get; private set; }
        public int MaxMP { get; private set; }
        public int Experience { get; private set; }
        public int Spirit { get; private set; }
        public int Chi { get; private set; }
        public int MaxChi { get; private set; }

        public DataStream Deserialize()
        {
            Level = ReadInt16();
            Skip(2);
            HP = ReadInt32();
            MaxHP = ReadInt32();
            MP = ReadInt32();
            MaxMP = ReadInt32();
            Experience = ReadInt32();
            Spirit = ReadInt32();
            Chi = ReadInt32();
            MaxChi = ReadInt32();

            return this;
        }
    }
}
