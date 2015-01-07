using System;
using System.Security.Cryptography;
using System.Text;

namespace SiviliaFramework.Network.Security
{
    class MD5Hash
    {
        private byte[] hash;
        private byte[] login;

        public byte[] GetHash(string login, string password, byte[] key)
        {
            byte[] loginData = Encoding.ASCII.GetBytes(login);
            byte[] authData = Encoding.ASCII.GetBytes(login + password);


            this.login = loginData;
            this.hash = new HMACMD5(MD5.Create()
                .ComputeHash(authData))
                .ComputeHash(key);

            return this.hash;
        }
        public byte[] GetKey(byte[] key)
        {
            byte[] hash02 = new byte[key.Length + hash.Length];

            Buffer.BlockCopy(hash, 0, hash02, 0, hash.Length);
            Buffer.BlockCopy(key, 0, hash02, hash.Length, key.Length);

            return new HMACMD5(login).ComputeHash(hash02);
        }
    }
}
