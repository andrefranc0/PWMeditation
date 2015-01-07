using SiviliaFramework.Network.Security;
using SiviliaFramework.IO;
using System;
using SiviliaFramework.Data;

namespace SiviliaFramework.Network.Packets.Server
{
    [PacketIdentifier(0x02, IO.PacketType.ServerPacket)]
    public class SMKeyS02 : GamePacket<ConnectionStatus>
    {
        [FIELD] public byte[] SMKey;
        [FIELD] public bool Force;

        protected internal override void HandleData(ConnectionStatus data)
        {
            byte[] CMKey = new byte[16];

            Random rnd = new Random();
            rnd.NextBytes(CMKey);

            data.SMKey = SMKey;
            data.CMKey = CMKey;

            MD5Hash md5Hash = data.MD5;
            EncDec encDec = data.EncDec;

            byte[] decKey = md5Hash.GetKey(CMKey);
            byte[] encKey = md5Hash.GetKey(SMKey);

            encDec.CreateEncode(encKey);
            encDec.CreateDecode(decKey);
        }
    }
}
