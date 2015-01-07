using System;
using System.Collections.Generic;
using System.Text;

namespace OOGLibrary.IO.PacketBase.Server
{
    class MoneyInfoS52 : DataStream, IServerContainer
    {
        public int Money { get; private set; }
        public int MaxMoney { get; private set; }

        public DataStream Deserialize()
        {
            Money = ReadInt32();
            MaxMoney = ReadInt32();
            return this;
        }
    }
}
