using System;

using SiviliaFramework.IO;

namespace SiviliaFramework.Network.Packets.Client
{
    [PacketIdentifier(0x86, IO.PacketType.ClientContainer)]
    public class MeditationSetC86 : GamePacket
    {
        public MeditationSetC86(int type, bool enabled)
        {
            Type = type;
            Enabled = enabled;
        }

        [FIELD] public int Type;
        [FIELD] public bool Enabled;
    }
}

