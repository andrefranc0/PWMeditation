using System;
using System.Collections.Generic;
using System.Text;

namespace OOGLibrary.GameTypes
{
    class CoreVersion
    {
        byte[] ver = new byte[4];
        public byte Version1 { get { return ver[0]; } set { ver[0] = value; } }
        public byte Version2 { get { return ver[1]; } set { ver[1] = value; } }
        public byte Version3 { get { return ver[2]; } set { ver[2] = value; } }
        public byte Version4 { get { return ver[3]; } set { ver[3] = value; } }

        public CoreVersion() : this(0, 1, 4, 5) { }
        public CoreVersion(byte v1, byte v2, byte v3) : this(0, v1, v2, v3) { }
        public CoreVersion(byte v1, byte v2, byte v3, byte v4)
        {
            ver = new byte[] { v1, v2, v3, v4 };
        }
        public override string ToString()
        {
            if (ver[0] != 0)
                return String.Format("{0}.{1}.{2}.{3}", ver[0], ver[1], ver[2], ver[3]);
            else return String.Format("{0}.{1}.{2}", ver[1], ver[2], ver[3]);
        }
        public byte[] ToBytes()
        {
            return ver;
        }
    }
}
