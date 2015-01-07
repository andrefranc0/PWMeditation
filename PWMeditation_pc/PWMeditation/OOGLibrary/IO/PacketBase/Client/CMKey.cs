using System;
using System.Collections.Generic;
using System.Text;

namespace OOGLibrary.IO.PacketBase.Client
{
    class CMKeyC02 : DataStream, IClientPacket
    {
        internal byte[] ServerKey { get; private set; }
        internal byte[] ClientKey { get; private set; }
        public bool Force { get; private set; }
        public CMKeyC02(Server.SMKeyS02 key, bool force)
        {
            ServerKey = key.Key;
            ClientKey = new byte[16]; new Random().NextBytes(ClientKey);
            Force = force;
        }
        public DataStream Serialize()
        {
            Type = 0x02;
            WriteData(ClientKey);
            Write(Force);
            return this;
        }
    }
}
