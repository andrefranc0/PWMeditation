using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace OOGLibrary.Cryptography
{
    class MD5Hash
    {
        byte[] Hash = new byte[16];
        byte[] Login = new byte[16];
        public byte[] GetHash(string login, string password, byte[] key)
        {
            Login = Encoding.ASCII.GetBytes(login);
            byte[] pwdBytes = Encoding.ASCII.GetBytes(login + password);


            MD5 md = MD5.Create();
            Hash = new HMACMD5(md.ComputeHash(pwdBytes)).ComputeHash(key);

            return Hash;
        }
        public byte[] GetKey(byte[] key)
        {
            byte[] nhash = new byte[key.Length + Hash.Length];
            Array.Copy(Hash, nhash, Hash.Length);
            Array.Copy(key, 0, nhash, Hash.Length, key.Length);

            return new HMACMD5(Login).ComputeHash(nhash);
        }
    }
}
