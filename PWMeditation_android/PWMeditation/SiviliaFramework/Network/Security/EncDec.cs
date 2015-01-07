using System;
using System.Collections.Generic;
using SiviliaFramework.IO;

namespace SiviliaFramework.Network.Security
{
    class EncDec
    {
        RC4 enc;
        RC4 dec;
        MppcUnpacker mppc;

        DataStream decBuffer = new DataStream();

        public void CreateEncode(byte[] key)
        {
            enc = new RC4();

            enc.Shuffle(key);
        }
        public void CreateDecode(byte[] key)
        {
            mppc = new MppcUnpacker();
            dec = new RC4();

            dec.Shuffle(key);
        }
        public byte[] Encode(byte[] data)
        {
            if (enc == null)
            {
                return data;
            }

            byte[] result = new byte[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                result[i] = enc.Encode(data[i]);
            }

            return result;
        }
        public byte[] Decode(byte[] buffer, int length)
        {
            byte[] data = new byte[length];

            Buffer.BlockCopy(buffer, 0, data, 0, length);

            if (dec == null || mppc == null)
            {
                return data;
            }


            foreach (byte bt in data)
            {
                decBuffer.WriteBytes(mppc
                    .Unpack(dec
                    .Encode(bt)));
            }

            data = decBuffer.GetBytes();
            decBuffer.Clear();

            return data;
        }
    }
}
