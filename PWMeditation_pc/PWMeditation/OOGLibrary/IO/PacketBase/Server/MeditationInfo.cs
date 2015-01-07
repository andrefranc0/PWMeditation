using System;
using System.Collections.Generic;
using System.Text;
using OOGLibrary.GameTypes;

namespace OOGLibrary.IO.PacketBase.Server
{
    class MeditationInfoS149 : DataStream, IServerContainer
    {
        public int Today { get; private set; }
        public bool Today1Updated { get; private set; }
        public int Today1 { get; private set; }
        public bool Today2Updated { get; private set; }
        public int Today2 { get; private set; }

        public DataStream Deserialize()
        {
          Today = ReadInt32();
            int count = ReadInt32();
            for (int i = 0; i < count; i++)
            {
                int type = ReadInt32();
                UnixTime time = ReadInt32();
                Skip(4);

                if (type == 0)
                {
                    Today1 = time;
                    Today1Updated = true;
                }
                if (type == 1)
                {
                    Today2 = time;
                    Today2Updated = true;
                }
            }
            return this;
        }
    }
}
