using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OOGLibrary.IO;
using OOGLibrary.IO.PacketBase.Server;
using OOGLibrary.Network;
using OOGLibrary.Network.Templates;
using System.IO;

namespace PWMeditation
{
    partial class frmAccount : Form
    {

        public Settings AccountSettings { get; set; } // Собственно настройки(базовые)
        bool work; // флаг работы, дабы включить/отключить кнопку Load

        string[] names; // Имена персов на аккаунте
        public bool Save { get; private set; } // Флаг(сохранить ли настройки)

        string[] ServerTable = new string[]
        {
            // RU
            "link1.pwonline.ru",  // Орион
            "link2.pwonline.ru",  // Вега
            "link3.pwonline.ru",  // Сириус 
            "link4.pwonline.ru",  // Мира
            "link5.pwonline.ru",  // Таразед
            "link6.pwonline.ru",  // Альтаир
            "link7.pwonline.ru",  // Процион
            "link8.pwonline.ru",  // Астра
            "link9.pwonline.ru",  // Пегас
            "link10.pwonline.ru", // Антарес
            "link11.pwonline.ru", // Адара
            "link12.pwonline.ru", // Феникс
            "link13.pwonline.ru", // Лиридан
            "link15.pwonline.ru", // Омега
            // PWI
            "pwiwest4.perfectworld.com", // Archosaur
            "pwigc3.perfectworld.com",   // Lost City
            "pwigc2.perfectworld.com",   // Sanctuary
            "pwigc4.perfectworld.com",   // Heavens Tear
            "pwieast3.perfectworld.com", // Raging Tide
            "pwieast1.perfectworld.com", // Harshlands 
            "pwieast2.perfectworld.com", // Dreamweaver 
            "pwieu3.en.perfectworld.eu"  // Morai 
        };

        string ServerPath = @"C:\Program Files\Mail.Ru\Perfect World\element\userdata\currentserver.ini"; // Используемый сервер
        string ServerOldPath = @"C:\Windows.old\Program Files\Mail.Ru\Perfect World\element\userdata\currentserver.ini"; // Используемый сервер
        string UsedServer
        {
            get
            {
                string path;
                if (File.Exists(ServerPath))
                    path = ServerPath;
                else if (File.Exists(ServerOldPath))
                    path = ServerOldPath;
                else return string.Empty;

                string[] args = File.ReadAllLines(path, Encoding.GetEncoding(1251));

                string serverName = args[1].Split(new char[] { '=' })[1]; // Название

                if (cbServer.Items.Contains(serverName)) return serverName;
                else return args[2].Split(new char[] { '=' })[1]; // Название неизвестно, используем хост
            }
        }
        public frmAccount()
        {

            InitializeComponent();
            LoadServerList();

            AccountSettings = new Settings();

            cbServer.Text = UsedServer;
            cbForce.Checked = true;

            cbType.SelectedIndex = 3;

            cbShowOnline.Checked = true;
            tbMessage.Text = "Медитирую^_^";
        }
        public frmAccount(Settings settings)
        {
            InitializeComponent();
            LoadServerList();

            AccountSettings = settings;

            if (!string.IsNullOrEmpty(settings.PlayerName))
                cbRole.Items.Add(settings.PlayerName);

            if (
                settings.Port != 0 && // Порт указан
                !String.IsNullOrEmpty(settings.Server) && // Сервер указан
                String.IsNullOrEmpty(settings.ServerName) // Но название сервера не указано:(
                )
            {
                cbServer.Text = String.Format("{0}:{1}", settings.Server, settings.Port);
            }
            else // Название есть
            {
                cbServer.Text = settings.ServerName;
            }

            tbLogin.Text = settings.Login;
            tbPassword.Text = settings.Password;
            cbForce.Checked = settings.Force; // Усиленный вход

            cbRole.Text = settings.PlayerName; // Имя игрока(чтобы зайти за него)

            cbType.SelectedIndex = settings.MeditationType; // Тип медитации

            cbShowOnline.Checked = settings.ShowOnline; // Отображать онлайн в френдах/гильдии
            cbUseMessage.Checked = settings.UseAutoMessage;
            tbMessage.Text = settings.AutoMessage;
        }
        private void LoadServerList()
        {
            if (File.Exists("serverlist.txt"))
            {
                try
                {
                    string[] list = File.ReadAllLines("serverlist.txt", Encoding.GetEncoding(1251));
                    List<string> servers = new List<string>();
                    List<string> names = new List<string>();

                    foreach (string s in list)
                    {
                        string[] args = s.Replace(" ", "").Split('\t');
                        if (args.Length < 2) continue;

                        names.Add(args[0]);
                        servers.Add(args[1]);
                    }
                    ServerTable = servers.ToArray();
                    cbServer.Items.Clear();
                    foreach (string s in names)
                    {
                        cbServer.Items.Add(s);
                    }
                }
                catch
                {

                }
            }

        }
        private void bLoad_Click(object sender, EventArgs e)
        {
            work = true;
            GameServer server = GetServer();
            OOGAccountHost oog = new OOGAccountHost(server, tbLogin.Text, tbPassword.Text, cbForce.Checked);
            oog.Disconnected += oog_Disconnected;
            oog.OnPlayersLoaded += oog_OnPlayersLoaded;
            oog.Receive += oog_Receive;
            oog.BeginWork();
        }
        private void oog_Disconnected(object sender, EventArgs e)
        {
            if (work == false)
                return;
            work = false;
            GameServer server = GetServer();
            MessageBox.Show("Ошибка подключения к " + server.Host + ":" + server.Port, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void oog_OnPlayersLoaded(object sender, OnPlayersLoadedEventArgs e)
        {
            work = false;
            string[] names = new string[e.Count];
            for (int i = 0; i < e.Count; i++)
            {
                names[i] = e[i].Name;
            }
            this.names = names;
            ((OOGAccountHost)sender).Client.Close();
        }
        private void oog_Receive(object sender, ReceiveEventArgs e)
        {
            if (e.Stream is ErrorInfo)
            {
                work = false;
                string error = string.Format("Ошибка при авторизации\n{0}", ((ErrorInfo)e.Stream).Message);
                MessageBox.Show(error, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private GameServer GetServer()
        {
            string serverAddress = "localhost:29000";
            bool useName = false;
            if (cbServer.Items.Contains(cbServer.Text))
            {
                int pos = 0;
                foreach(string item in cbServer.Items)
                {
                    if(item == cbServer.Text)
                    {
                        serverAddress = ServerTable[pos];
                    }
                    pos++;
                }
                useName = true;
            }
            try
            {
                string host = "localhost";
                int port = 29000;

                string[] serverarg = serverAddress.Split(new char[] { ':' });

                if (serverarg.Length != 0)
                {
                    if (serverarg.Length > 1)
                    {
                        if (int.TryParse(serverarg[0], out port))
                        {
                            host = serverarg[1];
                        }
                        else if (int.TryParse(serverarg[1], out port))
                        {
                            host = serverarg[0];
                        }
                        else port = 29000;
                    }
                    else
                    {
                        host = serverarg[0];
                    }
                }

                GameServer srv = new GameServer();
                srv.Host = host;
                srv.Port = port;
                srv.Name = useName ? cbServer.Text : string.Empty;

                return srv;
            }
            catch
            {
                MessageBox.Show("Ошибка, сервер указан не правильно", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return new GameServer("", "localhost", 29000);
        }

        private void timeUpdate_Tick(object sender, EventArgs e)
        {
            bLoad.Enabled = !work;
            if (names != null)
            {
                cbRole.Items.Clear();
                cbRole.Items.AddRange(names);
                names = null;
            }
        }
        private void bOk_Click(object sender, EventArgs e)
        {
            Settings set = AccountSettings;
            GameServer srv = GetServer();

            set.ServerName = srv.Name;
            set.Server = srv.Host;
            set.Port = srv.Port;

            set.Login = tbLogin.Text;
            set.Password = tbPassword.Text;
            set.Force = cbForce.Checked;
            set.PlayerName = cbRole.Text;

            set.MeditationType = cbType.SelectedIndex;

            set.ShowOnline = cbShowOnline.Checked;
            set.AutoMessage = tbMessage.Text;
            set.UseAutoMessage = cbUseMessage.Checked;

            Save = true;

            this.Close();
        }

        private void cbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbType.SelectedIndex == -1) 
                cbType.SelectedIndex = 0;
        }
    }
}
