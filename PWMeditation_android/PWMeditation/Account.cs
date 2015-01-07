using System;
using System.Collections.Generic;
using SiviliaFramework.Network;
using SiviliaFramework.Network.Plugins;
using SiviliaFramework.Network.Packets.Server;
using SiviliaFramework.Data;
using System.Threading;

namespace PWMeditation
{
    public class Account
    {
        public OOGHost Host;
        public AuthPlugin Auth;
        public ReloginPlugin Relogin;
        public MeditationPlugin Meditation;
        public EmuPlugin Emu;
        public ConnectionStatus ConnectionStatus;

        public Account()
        {
            Host = new OOGHost("");

            Auth = Host.IncludePlugin<AuthPlugin>();
            Relogin = Host.IncludePlugin<ReloginPlugin>();
            Meditation = Host.IncludePlugin<MeditationPlugin>();
            Emu = Host.IncludePlugin<EmuPlugin>();

            Meditation.Enabled = true;

            Host.PacketHandler<MeditationEnabledS14A>().OnReceive += (object sender, SiviliaFramework.IO.GamePacket e) => RefreshAccount();
            Host.PacketHandler<MeditationInfoS149>().OnReceive += (object sender, SiviliaFramework.IO.GamePacket e) => RefreshAccount();
            Host.PacketHandler<RoleInfoUpdateS26>().OnReceive += (object sender, SiviliaFramework.IO.GamePacket e) => RefreshAccount();
            Host.PacketHandler<RoleStatsInfoS32>().OnReceive += (object sender, SiviliaFramework.IO.GamePacket e) => RefreshAccount();
            Relogin.ReloginStatusUpdate += (object sender, EventArgs args) => RefreshAccount();
            Host.Connection.Connected += (object sender, EventArgs e) => RefreshAccount();
            Host.Connection.Disconnected += (object sender, EventArgs e) => RefreshAccount();
            Auth.OnEnteredWorld += (object sender, EventArgs e) => RefreshAccount();

            ConnectionStatus = Host.IncludeDataBlock<ConnectionStatus>();

            Auth.RolesListLoaded += Auth_RolesLoaded;

            ServerName = string.Empty;
            ServerAddress = string.Empty;
            Login = string.Empty;
            Password = string.Empty;
            AutoMessage = string.Empty;
        }

        private void RefreshAccount()
        {
            Settings.Main.RefreshAccountInfo(this);
        }

        public string ServerName;
        public string ServerAddress;

        public string Login;
        public string Password;
        public string RoleName
        {
            get
            {
                if (SelectedRole < 0 || SelectedRole >= Roles.Count)
                    return string.Empty;
                return Roles[SelectedRole];
            }
        }

        public int SelectedRole = -1;
        public List<string> Roles = new List<string>();

        public bool Force;
        public bool ShowOnline;

        public string AutoMessage;
        public bool UseAutoMessage;

        public void LoadRolesList()
        {
            if (Host.Connection.IsWork)
                return;
            Relogin.Enabled = false;

            Host.Connect();
            Roles.Clear();
            while (Host.Connection.IsWork && !Auth.AccountInformation.RolesLoaded)
            {
                Thread.Sleep(1);
            }
            while (Host.Connection.IsWork && Auth.AccountInformation.Roles.Count != Roles.Count)
            {
                Thread.Sleep(1);
            }
            Host.Close();
        }

        public bool IsWork { get; internal set; }

        public void Open()
        {
            if (IsWork)
                return;
            IsWork = true;

            Relogin.Enabled = true;
            Host.Connect();

            RefreshAccount();
        }
        public void Close()
        {
            if (!IsWork)
                return;
            IsWork = false;

            Relogin.Enabled = false;
            Host.Close();

            RefreshAccount();
        }

        public void CompleteSettings()
        {
            ConnectionStatus.GameServer.SetServer(ServerAddress);
            Relogin.RoleName = RoleName;
            Auth.SetAuthData(Login, Password, Force);

            Emu.ShowOnline = ShowOnline;
            Emu.UseAutoMessage = UseAutoMessage;
            Emu.AutoMessage = AutoMessage;
        }
        public void CompleteSettings(int accountIndex)
        {
            CompleteSettings();
            Settings.Main.RefreshAccount(accountIndex);
        }

        private void Auth_RolesLoaded(object sender, EventArgs e)
        {
            Roles.Clear();
            if (Host.Connection.IsWork)
            {
                foreach (RoleInfo role in Auth.AccountInformation.Roles)
                {
                    Roles.Add(role.Name);
                }
            }
        }

        public override string ToString()
        {
            return string.Format("[{0}] {1} - {2}", ServerName, Login, RoleName);
        }
    }
}

