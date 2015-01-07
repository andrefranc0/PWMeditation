using System;

namespace SiviliaFramework.IO
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PacketIdentifier : Attribute
    {
        public uint PacketId { get; private set; }
        public PacketType PacketType { get; private set; }

        public PacketIdentifier(uint packetId)
        {
            PacketId = packetId;
            PacketType = PacketType.ServerPacket;
        }
        public PacketIdentifier(uint packetId, PacketType packetType)
        {
            PacketId = packetId;
            PacketType = packetType;
        }

        public override string ToString()
        {
            string prefix = string.Empty;
            switch(PacketType)
            {
                case IO.PacketType.ClientContainer: prefix = "c22-";    break;
                case IO.PacketType.ServerContainer: prefix = "s00-";    break;
                case IO.PacketType.ClientContainerC25: prefix = "c25-"; break;
                case IO.PacketType.ClientPacket: prefix = "c";          break;
                case IO.PacketType.ServerPacket: prefix = "s";          break;
                case IO.PacketType.Unknown: prefix = "unk-";            break;
            }

            return prefix + PacketId.ToString("X2");
        }
    }
}
