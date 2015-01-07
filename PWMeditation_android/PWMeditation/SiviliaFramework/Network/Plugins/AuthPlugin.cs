using System;
using System.Collections.Generic;
using System.Text;
using SiviliaFramework.Network;
using SiviliaFramework.IO;
using SiviliaFramework.Network.Packets.Client;
using SiviliaFramework.Network.Packets.Server;
using SiviliaFramework.Data;

namespace SiviliaFramework.Network.Plugins
{
    public class AuthPlugin : Plugin
    {
        public event EventHandler RolesListLoaded;
        public event EventHandler OnEnteredWorld;

        public OOGHost Host { get; private set; }
        public AccountInformation AccountInformation { get; private set; }

        string login;
        string password;
        bool force;

        public void SetAuthData(string login, string password)
        {
            SetAuthData(login, password, true);
        }
        public void SetAuthData(string login, string password, bool force)
        {
            this.login = login;
            this.password = password;
            this.force = force;
        }
        public void LogIn()
        {
            LogIn(login, password, force);
        }
        public void LogIn(string login, string password)
        {
            LogIn(login, password, true);
            AccountInformation.Login = login;
        }
        public void LogIn(string login, string password, bool force)
        {
            if (!Host.Connection.IsWork)
            {
                this.login = login;
                this.password = password;
                this.force = force;

                Host.Connect();
            }
        }
        public void EnterWorld(int slot)
        {
            Host.Send(new SelectRoleC46(slot));
        }

        public void CreateRole(string name, byte occupation, byte gender, byte[] face)
        {
            Host.Send(new CreateRoleC54(name, gender, occupation, face));
        }

        internal protected override void Initialize(OOGHost oogHost)
        {
            Host = oogHost;

            Host.IncludePlugin<KeepAlivePlugin>();
            AccountInformation = Host.IncludeDataBlock<AccountInformation>();

            Host.PacketHandler<ServerInfoS01>().OnReceive += Receive_ServerInfo;
            Host.PacketHandler<SMKeyS02>().OnReceive += Receive_SMKey;
            Host.PacketHandler<OnlineAnnounceS04>().OnReceive += Receive_RoleSelectionPage;
            Host.PacketHandler<RoleLogoutS45>().OnReceive += Receive_RoleSelectionPage;
            Host.PacketHandler<RoleList_ReS53>().OnReceive += Receive_RoleList_Re;
            Host.PacketHandler<SelectRole_ReS47>().OnReceive += Receive_SelectRole_Re;
            Host.PacketHandler<PlayerPositionS08>().OnReceive += Receive_PlayerPosition;

            Host.Connection.Disconnected += Connection_Disconnected;
        }

        void Connection_Disconnected(object sender, EventArgs e)
        {
            AccountInformation.RolesLoaded = false;
        }

        private void Receive_ServerInfo(object sender, GamePacket packet)
        {
            Host.Send(new LogginAnnounceC03(login, password));
        }
        private void Receive_SMKey(object sender, GamePacket packet)
        {
            Host.Send(new CMKeyC02(force));
        }
        private void Receive_RoleSelectionPage(object sender, GamePacket packet)
        {
            Host.Send(new RoleListC52(-1));
        }
        private void Receive_RoleList_Re(object sender, GamePacket packet)
        {
            RoleList_ReS53 roleListRe = packet as RoleList_ReS53;
            if (roleListRe.IsChar)
            {
                Host.Send(new RoleListC52(roleListRe.NextSlot));
            }
            else
            {
                AccountInformation.RolesLoaded = true;
                if (RolesListLoaded != null)
                    RolesListLoaded(this, new EventArgs());
            }
        }
        private void Receive_SelectRole_Re(object sender, GamePacket packet)
        {
            Host.Send(new EnterWorldC48());
        }

        private void Receive_PlayerPosition(object sender, GamePacket e)
        {
            if (OnEnteredWorld != null)
                OnEnteredWorld(this, new EventArgs());
        }
    }
}
