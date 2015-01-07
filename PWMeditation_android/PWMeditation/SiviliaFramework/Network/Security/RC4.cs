using System;

namespace SiviliaFramework.Network.Security
{
    class RC4
    {
        byte shift1;
        byte shift2;
        byte[] table;

        public RC4()
        {
            table = new byte[256];
            for (int i = 0; i < 256; i++)
                table[i] = (byte)i;
        }

        public void Shuffle(byte[] key)
        {
            int shift = 0;

            for (int i = 0; i < 256; i++)
            {
                shift += key[i % key.Length] + table[i];
                shift %= 256;

                byte a = table[i];
                byte b = table[shift];

                table[i] = b;
                table[shift] = a;
            }
        }
        public byte Encode(byte bt)
        {
            shift1++;
            shift2 += table[shift1];

            byte a = table[shift1];
            byte b = table[shift2];

            table[shift2] = a;
            table[shift1] = b;

            byte d = table[(byte)(a + b)];

            return (byte)(bt ^ d);
        }
    }
}