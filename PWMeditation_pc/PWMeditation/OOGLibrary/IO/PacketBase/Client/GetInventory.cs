using System;
using System.Collections.Generic;
using System.Text;

namespace OOGLibrary.IO.PacketBase.Client
{
    class GetInventoryC27 : DataStream, IClientContainer
    {
        public bool Flag1 { get; set; }
        public bool Flag2 { get; set; }
        public bool Flag3 { get; set; }

        public GetInventoryC27() : this(true, true, false)
        {
        }
        public GetInventoryC27(bool flag1, bool flag2, bool flag3)
        {
            Flag1 = flag1;
            Flag2 = flag2;
            Flag3 = flag3;
        }


        public DataStream Serialize()
        {
            Type = 0x27;

            Write(Flag1);
            Write(Flag2);
            Write(Flag3);
            
            return this;
        }
    }
}
