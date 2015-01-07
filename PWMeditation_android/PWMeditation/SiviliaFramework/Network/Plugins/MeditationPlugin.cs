using System;

using SiviliaFramework.Data;
using SiviliaFramework.IO;
using SiviliaFramework.Network.Packets.Client;
using SiviliaFramework.Network.Packets.Server;

namespace SiviliaFramework.Network.Plugins
{
    public class MeditationPlugin : Plugin
    {
        OOGHost Host;
        AccountInformation AccountInformation { get; set; }

        public MeditationInformation MeditationInformation { get; private set; }
        public bool Enabled { get; set;}

        internal protected override void Initialize(OOGHost oogHost)
        {
            Host = oogHost;

            MeditationInformation = oogHost.IncludeDataBlock<MeditationInformation>();
            AccountInformation = oogHost.IncludeDataBlock<AccountInformation>();

            oogHost.PacketHandler<MeditationInfoS149>().OnReceive += (object sender, GamePacket e) => HandleUpdate();
            oogHost.PacketHandler<MeditationEnabledS14A>().OnReceive += (object sender, GamePacket e) => HandleUpdate();
        }

        private void HandleUpdate()
        {
            if (!Enabled || MeditationInformation.MeditationEnabled || AccountInformation.SelectedRole.WorldId != 150)
                return;

            if (MeditationInformation.MeditationTypes[1] != null && MeditationInformation.MeditationTypes[1].Seconds > 0)
            {
                StartMeditation(1);
                return;
            }
            else if (MeditationInformation.MeditationTypes[0] != null && MeditationInformation.MeditationTypes[0].Seconds > 0)
            {
                StartMeditation(0);
                return;
            }
        }

        public void StartMeditation(int meditationType)
        {
            Host.Send(new MeditationSetC86(meditationType, true));
        }
        public void StopMeditation(int meditationType)
        {
            Host.Send(new MeditationSetC86(meditationType, false));
        }
    }
}

