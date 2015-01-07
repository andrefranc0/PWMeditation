
namespace SiviliaFramework.IO
{
    public enum PacketType
    {
        Unknown,
        // C
        ClientPacket,
        // C22
        ClientContainer,
        // C22-25
        ClientContainerC25,
        // S
        ServerPacket,
        // S00-22
        ServerContainer
    }
}
