using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using OOGLibrary.IO;
using OOGLibrary.IO.PacketBase.Server;
using OOGLibrary.Network;
using OOGLibrary.Network.Templates;

namespace PWMeditation
{
    internal class MeditationLogic
    {
        public static List<ReceiveMessageEventArgs> PrivateMessages = new List<ReceiveMessageEventArgs>();


        public static int ExpDelta;


        public static int MeditationTotal;
        public static int MeditationDeep;
        public static int MeditationNormal;

        private static bool enabled;
        public static bool Enabled
        {
            get { return enabled; }
            set
            {
                PrivateMessages.Clear();
                enabled = value;
                if (value) Start();
                else
                    Stop();
            }
        }

        public static string Status { get; private set; }

        public static string MeditationEnabled
        {
            get
            {
                if (!enabled) return string.Empty;
                if (oog == null || (oog != null && !oog.EnteredTheWorld))
                    return Status;
                return oog.Meditation.Enabled ? "Медитирует" : "Не медитирует";
            }
        }

        public static int Index { get; set; }

        public static OOGAccountHost oog { get; set; }

        public static Settings Settings
        {
            get { return Program.SettingsList[Index]; }
        }

        public static void Start()
        {
            if (oog != null || !enabled) return;
            Settings set = Settings;
            GameServer server = new GameServer(set.ServerName, set.Server, set.Port);
            oog = new OOGAccountHost(server, set.Login, set.Password, set.Force);
            oog.Disconnected += oog_Disconnected;
            oog.OnPlayersLoaded += oog_OnPlayersLoaded;
            oog.OnPlayerEnterTheWorld += oog_OnPlayerEnterTheWorld;
            oog.Receive += oog_Receive;

            oog.Meditation.MeditationType = Settings.MeditationType;
            oog.AutoMessage.Message = Settings.AutoMessage;
            oog.AutoMessage.Enabled = Settings.UseAutoMessage;
            oog.AutoMessage.OnMessageReceived += AutoMessage_OnMessageReceived;

            oog.BeginWork();
        }

        private static void Stop()
        {
            if (oog == null) return;

            RoleInfo.Level = 0;
            RoleInfo.Experience = 0;
            RoleInfo.Spirit = 0;
            MeditationTotal = 0;
            MeditationDeep = 0;
            MeditationNormal = 0;

            oog.Disconnected -= oog_Disconnected;
            oog.OnPlayersLoaded -= oog_OnPlayersLoaded;
            oog.OnPlayerEnterTheWorld -= oog_OnPlayerEnterTheWorld;
            oog.Receive -= oog_Receive;
            oog.AutoMessage.OnMessageReceived -= AutoMessage_OnMessageReceived;
            oog.Client.Close();
            oog = null;
        }

        public static void Relogin()
        {
            Stop();
            Status = "Релогин";
            Thread.Sleep(10000);
            Start();
        }

        private static void oog_OnPlayersLoaded(object sender, OnPlayersLoadedEventArgs e)
        {
            Status = "Вход за персонажа";
            for (int i = 0; i < e.Count; i++)
            {
                if (e[i].Name == Program.SettingsList[Index].PlayerName)
                {
                    oog.Enter(i);
                    return;
                }
            }
            Stop();
        }

        private static void oog_OnPlayerEnterTheWorld(object sender, EventArgs e)
        {
            if (Settings.ShowOnline)
                oog.LoadFriendList();
        }

        static void AutoMessage_OnMessageReceived(object sender, ReceiveMessageEventArgs e)
        {
            PrivateMessages.Add(e);
        }

        private static void oog_Receive(object sender, ReceiveEventArgs e)
        {
            if (e.Stream is RoleInfoUpdateS26)
            {
                RoleInfoUpdateS26 info = (RoleInfoUpdateS26)e.Stream;

                if (RoleInfo.Experience != 0)
                {
                    ExpDelta = info.Experience - RoleInfo.Experience;
                }

                RoleInfo.Level = info.Level;
                RoleInfo.Experience = info.Experience;
                RoleInfo.Spirit = info.Spirit;
            }

            if (e.Stream is MeditationInfoS149)
            {
                MeditationTotal = oog.Meditation.All;
                MeditationNormal = oog.Meditation.Today1;
                MeditationDeep = oog.Meditation.Today2;
            }
        }


        private static void oog_Disconnected(object sender, EventArgs e)
        {
            Status = "Отключено";
            new Thread(delegate() { Relogin(); }).Start();
        }
    }
}