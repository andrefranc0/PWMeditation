using System;
using System.Collections.Generic;
using System.Text;

namespace OOGLibrary.IO.PacketBase.Client
{
    class LogginAnnounce : DataStream, IClientPacket
    {
        private byte[] Key { get; set; }
        private string Password { get; set; }

        public string Login { get; private set; }
        internal byte[] Hash { get; private set; }

        public LogginAnnounce(Server.ServerInfoS01 sinfo, string login, string password)
        {
            Key = sinfo.Key;
            Login = login.ToLower();
            Password = password;
        }
        internal void GenHash(Cryptography.MD5Hash md5)
        {
            Hash = md5.GetHash(Login, Password, Key);
        }
        public DataStream Serialize()
        {
            Type = 0x03;
            WriteAString(Login);
            WriteData(Hash);
            WriteByte();
            WriteByte(4);
            Write(0);
            return this;
        }
    }
}
