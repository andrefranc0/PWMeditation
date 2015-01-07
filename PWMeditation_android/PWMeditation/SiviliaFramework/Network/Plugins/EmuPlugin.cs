using System;
using System.Collections.Generic;
using System.Text;
using SiviliaFramework.IO;
using SiviliaFramework.Network.Packets.Client;
using SiviliaFramework.Network.Packets.Server;

namespace SiviliaFramework.Network.Plugins
{
    public class EmuPlugin : Plugin
    {
        OOGHost Host;

        public bool ShowOnline { get; set; }
        public bool UseAutoMessage { get; set; }
        public bool SendGetInventory { get; set; }
        public string AutoMessage { get; set; }

        protected internal override void Initialize(OOGHost oogHost)
        {
            ShowOnline = false;
            SendGetInventory = true;

            Host = oogHost;

            Host.IncludePlugin<AuthPlugin>().OnEnteredWorld += EmuPlugin_OnEnteredWorld;
            Host.PacketHandler<PrivateMessageS60>().OnReceive += EmuPlugin_PrivateMessageReceive;
        }

        private void EmuPlugin_PrivateMessageReceive(object sender, GamePacket e)
        {
            PrivateMessageS60 msg = e as PrivateMessageS60;
            if (UseAutoMessage && msg.MessageType == 0x00)
            {
                Host.Send(new PrivateMessageC60(msg.NameSend, AutoMessage));
            }
        }
        void EmuPlugin_OnEnteredWorld(object sender, EventArgs e)
        {
            if (ShowOnline)
            {
                Host.Send(new GetFriendListCCE());
            }
            if (SendGetInventory)
            {
                Host.Send(new GetInventoryC27());
            }
        }
    }
}
