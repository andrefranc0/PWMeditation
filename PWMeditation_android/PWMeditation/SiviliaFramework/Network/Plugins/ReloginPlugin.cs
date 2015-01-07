using System;
using System.Collections.Generic;
using System.Text;
using SiviliaFramework.Network;
using SiviliaFramework.IO.GameTypes;
using System.Threading;
using SiviliaFramework.Network.Packets.Server;
using SiviliaFramework.Data;

namespace SiviliaFramework.Network.Plugins
{
    public delegate void ReloginEventHandler(object sender, ReloginEventArgs args);
    public enum ReloginStatus : int
    {
        Disconnected = 0,
        Relogin = 1,
        Connecting = 2,
        Connected = 3
    }
    public class ReloginEventArgs
    {
        public ReloginEventArgs(UnixTime lastLoginTime, int reloginInterval, int reconnectInterval)
        {
            LastLoginTime = lastLoginTime;
            ReloginInterval = reloginInterval;
            ReconnectInterval = reconnectInterval;
        }
        public UnixTime LastLoginTime { get; set; }
        public int ReloginInterval { get; private set; }
        public int ReconnectInterval { get; private set; }

        public int Sleep
        {
            get
            {
                int delta = new UnixTime().Timestamp - LastLoginTime.Timestamp;
                int sleep = ReloginInterval - delta;

                if (sleep < ReconnectInterval)
                    sleep = ReconnectInterval;

                return sleep;
            }
        }
        public UnixTime NextLoginTime
        {
            get
            {

                return new UnixTime() + Sleep;
            }
        }
    }
    public class ReloginPlugin : Plugin
    {
        OOGHost Host;
        AuthPlugin Auth;
        AccountInformation AccountInformation;

        AsyncCallback asyncRelogin;

        ReloginStatus status;

        public event ReloginEventHandler OnReconnect;
        public event EventHandler ReloginStatusUpdate;

        private int Sleep;
        public UnixTime LastLoginTime { get; private set; }
        public UnixTime NextLoginTime { get; private set; }
        public ReloginStatus Status
        {
            get
            {
                return status;
            }
            private set
            {
                status = value;
                if (ReloginStatusUpdate != null)
                    ReloginStatusUpdate(this, new EventArgs());
            }
        }

        // Настройки
        public int ReloginInterval { get; set; }
        public int ReconnectInterval { get; set; }

        public bool Enabled { get; set; }
        public string RoleName { get; set; }
        public int RolePosition { get; set; }

        // Статистика
        public int ReconnectCount { get; private set; }
        public int ReloginCount { get; private set; }

        protected internal override void Open()
        {
            ReconnectCount = 0;
            ReloginCount = 0;
        }
        protected internal override void Close()
        {
            Enabled = false;
        }
        internal protected override void Initialize(OOGHost oogHost)
        {
            Enabled = true;

            NextLoginTime = new UnixTime(0);
            LastLoginTime = new UnixTime(0);
            RolePosition = -1;

            asyncRelogin = new AsyncCallback(Relogin);

            ReloginInterval = 10000;
            ReconnectInterval = 5000;

            Host = oogHost;
            Auth = Host.IncludePlugin<AuthPlugin>();
            AccountInformation = Host.IncludeDataBlock<AccountInformation>();

            Host.PacketHandler<ServerInfoS01>().OnReceive += ServerInfoS01_OnReceive;
            Host.Connection.Disconnected += Connection_Disconnected;

            Auth.RolesListLoaded += Auth_RolesListLoaded;
            Auth.OnEnteredWorld += Auth_OnEnteredWorld;
        }


        void ServerInfoS01_OnReceive(object sender, IO.GamePacket e)
        {
            if (ReconnectCount > 0)
                ReloginCount++;

            Status = ReloginStatus.Connected;
            LastLoginTime = new UnixTime();
        }

        void Auth_RolesListLoaded(object sender, EventArgs e)
        {
            if (!Enabled || 
                (string.IsNullOrEmpty(RoleName) && (RolePosition == -1 || RolePosition >= AccountInformation.Roles.Count))) return;
            if (RolePosition != -1)
            {
                Auth.EnterWorld(RolePosition);
                return;
            }
            
            for (int i = 0; i < AccountInformation.Roles.Count; i++)
            {
                RoleInfo role = AccountInformation.Roles[i];
                if (role.Name == RoleName)
                {
                    RolePosition = i;
                    Auth.EnterWorld(i);
                    break;
                }
            }
        }

        void Auth_OnEnteredWorld(object sender, EventArgs e)
        {
            RoleName = AccountInformation.SelectedRole.Name;
        }

        private void Connection_Disconnected(object sender, EventArgs e)
        {
            if (Enabled)
            {
                Status = ReloginStatus.Relogin;
                ReloginEventArgs args = new ReloginEventArgs(LastLoginTime, ReloginInterval, ReconnectInterval);
                if (OnReconnect != null)
                    OnReconnect(this, args);

                Sleep = args.Sleep;
                NextLoginTime = args.NextLoginTime;

                asyncRelogin.BeginInvoke(null, null, null);

                ReconnectCount++;
            }
            else
            {
                Status = ReloginStatus.Disconnected;
            }
        }

        private void Relogin(IAsyncResult res)
        {
            if (Sleep > 0)
            {
                Thread.Sleep(Sleep);
            }
            if (!Enabled)
            {
                Status = ReloginStatus.Disconnected;
                return;
            }
            Status = ReloginStatus.Connecting;
            Host.Connect();
        }
    }
}
