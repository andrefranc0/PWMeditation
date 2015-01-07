using SiviliaFramework.Network.Security;
using SiviliaFramework.Data;
using SiviliaFramework.IO;

namespace SiviliaFramework.Network.Packets.Client
{
    [PacketIdentifier(0x03, PacketType.ClientPacket)]
    public class LogginAnnounceC03 : GamePacket<ConnectionStatus>
    {
        string Password;
        [FIELD(FieldType.AString)] public string Login;
        [FIELD] public byte[] Hash;
        [FIELD] public byte Unk1;
        [FIELD] public byte[] Unk2 = new byte[4];

        public LogginAnnounceC03(string login, string password)
        {
            Login = login.ToLower();
            Password = password;
        }
        protected internal override void HandleData(ConnectionStatus data)
        {
            MD5Hash md5Hash = data.MD5;
            byte[] key = data.ServerHash;

            Hash = md5Hash.GetHash(Login, Password, key);
        }
    }
}
