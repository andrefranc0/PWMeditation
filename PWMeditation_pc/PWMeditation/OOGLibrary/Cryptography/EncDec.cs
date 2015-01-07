using System;
using System.Collections.Generic;
using System.Text;

namespace OOGLibrary.Cryptography
{
    class EncDec
    {
        RC4 enc;
        RC4 dec;
        MPPCUnpack mppc;
        public void CreateEnc(byte[] key)
        {
            enc = new RC4();
            enc.Shuffle(key);
        }
        public void CreateDec(byte[] key)
        {
            dec = new RC4();
            dec.Shuffle(key);
            mppc = new MPPCUnpack();
        }
        public byte[] Encode(byte[] buf)
        {
            if (enc == null) return buf;

            byte[] ret = new byte[buf.Length];
            for (int i = 0; i < ret.Length; i++)
                ret[i] = enc.Encode(buf[i]);

            return ret;
        }
        public byte[] Decode(byte[] buf, int len)
        {
            byte[] newbuf = new byte[len];
            Array.Copy(buf, newbuf, len);

            if (dec == null || mppc == null)
                return newbuf;

            List<byte> ret = new List<byte>();
            foreach (byte bt in newbuf)
                ret.AddRange(mppc.Unpack(dec.Encode(bt)));
            return ret.ToArray();
        }
    }
}
