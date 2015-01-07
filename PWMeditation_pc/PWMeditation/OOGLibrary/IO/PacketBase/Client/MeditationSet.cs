using System;
using System.Collections.Generic;
using System.Text;

namespace OOGLibrary.IO.PacketBase.Client
{
    class MeditationSetC86 : DataStream, IClientContainer
    {
        public int MeditationType { get; private set; }
        public bool Flag { get; private set; }
        public MeditationSetC86(int type, bool flag)
        {
            Flag = flag;
            MeditationType = type;
        }
        public DataStream Serialize()
        {
            Type = 0x86;

            Write(MeditationType);
            Write(Flag);
            return this;
        }
    }
}
