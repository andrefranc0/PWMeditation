using System;
using SiviliaFramework.Data;
using SiviliaFramework.IO;

namespace SiviliaFramework.Network.Packets.Server
{
    [PacketIdentifier(0x14A, PacketType.ServerContainer)]
    public class MeditationEnabledS14A : GamePacket<MeditationInformation>
    {
        [SKIP(4)]
        [FIELD] public bool Enabled;

        internal protected override void HandleData(MeditationInformation data)
        {
            data.MeditationEnabled = Enabled;
        }
    }
}

