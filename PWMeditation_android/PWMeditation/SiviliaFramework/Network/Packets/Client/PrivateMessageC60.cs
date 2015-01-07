using SiviliaFramework.Data;
using SiviliaFramework.IO;

namespace SiviliaFramework.Network.Packets.Client
{
    [PacketIdentifier(0x60, PacketType.ClientPacket)]
    public class PrivateMessageC60 : GamePacket<AccountInformation>
    {
        public PrivateMessageC60(string name, string message) : this(name, message, 0)
        {
        }
        public PrivateMessageC60(string name, string message, byte type)
        {
            if (message.Length > 128) message = message.Substring(0, 128);

            Name = name;
            Message = message;
            MessageType = type;
            Data = new byte[0];

            if (type == 4)
            {
                Emotion = 0xDC;
            }
        }

        [FIELD] public byte MessageType;
        [FIELD] public byte Emotion;
        [FIELD] public string NameSend;
        [FIELD] public uint UidSend;
        [FIELD] public string Name;
        [FIELD] public uint Uid;
        [FIELD] public string Message;
        [FIELD] public byte[] Data;
        [FIELD] public uint Unk;

        protected internal override void HandleData(AccountInformation data)
        {
            UidSend = data.SelectedRole.UID;
            NameSend = data.SelectedRole.Name;
        }
    }
}
