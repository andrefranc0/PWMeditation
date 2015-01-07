using System;
using SiviliaFramework.IO;
using SiviliaFramework.Data;

namespace SiviliaFramework.Network.Packets.Server
{
    [PacketIdentifier(0x149, PacketType.ServerContainer)]
    public class MeditationInfoS149 : GamePacket<MeditationInformation>
    {
        [FIELD] public uint SecondsToday;
        [FIELD] public int MeditationTypesCount;
        [ARRAY("MeditationTypesCount")]
        [FIELD] public MeditationType[] MeditationTypes;

        internal protected override void HandleData(MeditationInformation data)
        {
            data.SecondsToday = SecondsToday;
            foreach (MeditationType meditationType in MeditationTypes)
            {
                if (meditationType.Type < data.MeditationTypes.Length)
                    data.MeditationTypes[meditationType.Type] = meditationType;
            }
        }
    }
}

