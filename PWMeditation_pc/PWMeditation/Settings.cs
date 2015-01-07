using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using MonoDB;

namespace PWMeditation
{
    class Settings
    {
        public string ServerName { get; set; }

        public string Server { get; set; }
        public int Port { get; set; }

        public string Login { get; set; }
        public string Password { get; set; }
        public bool Force { get; set; }
        public string PlayerName { get; set; }

        public int MeditationType { get; set; }

        public bool ShowOnline { get; set; }
        public bool UseAutoMessage { get; set; }
        public string AutoMessage { get; set; }

        private static string path = "config.mono";


        public static Settings[] Load()
        {
            if (!File.Exists(path)) return new Settings[0];
            MonoNode node = MonoReader.Load(path);
            MonoNode accounts = node["accounts"];
            Settings[] ret = new Settings[accounts.Count];
            for (int i = 0; i < ret.Length; i++)
            {
                MonoNode account = accounts[i];

                ret[i] = new Settings();

                ret[i].Port = account["port"].ToWord;
                ret[i].Server = account["server"].ToAString;
                ret[i].ServerName = account["servername"].ToUString;

                ret[i].Login = account["login"].ToAString;
                ret[i].Password = account["password"].ToAString;
                ret[i].Force = account["force"].ToBoolean;
                ret[i].PlayerName = account["rolename"].ToUString;

                ret[i].MeditationType = !account["meditationtype"].IsEmpty ? account["meditationtype"].ToInt32 : 0;

                ret[i].ShowOnline = !account["online"].IsEmpty ? account["online"].ToBoolean : true;
                ret[i].UseAutoMessage = !account["usemessage"].IsEmpty ? account["usemessage"].ToBoolean : false;
                ret[i].AutoMessage = !account["message"].IsEmpty ? account["message"].ToUString : string.Empty;

            }
            Program.SettingsList.AddRange(ret);
            return ret;
        }
        public static void Save()
        {
            Save(Program.SettingsList.ToArray());
        }
        public static void Save(Settings[] config)
        {
            MonoNode tosave = new MonoNode("OOG PWMeditation");
            MonoNode accounts = tosave["accounts"];
            foreach (Settings set in config)
            {
                MonoNode account = accounts[accounts.Count.ToString()];

                account["servername"].ToUString = set.ServerName;

                account["server"].ToAString = set.Server;
                account["port"].ToWord = (ushort)set.Port;

                account["login"].ToAString = set.Login;
                account["password"].ToAString = set.Password;
                account["force"].ToBoolean = set.Force;
                account["rolename"].ToUString = set.PlayerName;

                account["meditationtype"].ToInt32 = set.MeditationType;

                account["online"].ToBoolean = set.ShowOnline;
                account["message"].ToUString = set.AutoMessage;
                account["usemessage"].ToBoolean = set.UseAutoMessage;
            }
            MonoReader.Save(path, tosave);
        }
    }
}
