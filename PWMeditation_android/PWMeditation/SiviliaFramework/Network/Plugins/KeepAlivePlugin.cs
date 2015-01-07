using SiviliaFramework.IO;
using SiviliaFramework.Network.Packets.Client;
using SiviliaFramework.Network.Packets.Server;
using System;
using System.Threading;


namespace SiviliaFramework.Network.Plugins
{
    public class KeepAlivePlugin : Plugin
    {
        private bool enabled;
        public bool Enabled
        {
            get
            {
                return enabled;
            }
            set
            {
                if (value && !Host.Connection.IsWork)
                    return;
                enabled = value;
                if (value)
                {
                    thread = new Thread(SendKeepAlive);
                    thread.Start();
                }
            }
        }
        public int Ping { get; private set; }

        OOGHost Host;
        Thread thread;

        DateTime sendTime;
 
        internal protected override void Initialize(OOGHost oogHost)
        {
            Host = oogHost;
            Host.PacketHandler<OnlineAnnounceS04>().OnReceive += Receive_OnlineAnnounce;
            Host.PacketHandler<KeepAliveS5A>().OnReceive += Receive_KeepAlive;
            Host.Connection.Disconnected += Connection_Disconnected;
        }

        private void SendKeepAlive()
        {
            while (true)
            {
                sendTime = DateTime.Now;
                Host.Send(new KeepAliveC5A());
                for (int i = 0; i < 150; i++)
                {
                    Thread.Sleep(100);
                    if (!enabled) return;
                }
            }
        }

        private void Connection_Disconnected(object sender, EventArgs e)
        {
            Enabled = false;
        }
        private void Receive_OnlineAnnounce(object sender, GamePacket packet)
        {
            Enabled = true;
        }
        private void Receive_KeepAlive(object sender, GamePacket packet)
        {
            DateTime recvTime = DateTime.Now;

            TimeSpan delta = recvTime - sendTime;
            Ping = (int)delta.TotalMilliseconds;
        }
    }
}
