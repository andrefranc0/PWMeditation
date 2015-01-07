using System;
using System.Collections.Generic;
using System.IO;
using SiviliaFramework.Network;

namespace PWMeditation
{
    public class Settings
    {
        static string path;
        public static void Initialize()
        {
            path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            path = Path.Combine(path, "settings.meditation");
            Load();
        }

        public static MainActivity Main;

        public static List<GameServer> GameServers = new List<GameServer>();
        public static List<Account> Accounts = new List<Account>();

        public static void Save()
        {
            BinaryWriter bw = new BinaryWriter(File.Create(path));

            bw.Write(GameServers.Count);
            foreach (GameServer gameServer in GameServers)
            {
                bw.Write(gameServer.Host);
                bw.Write(gameServer.Port);
                bw.Write(gameServer.Name);
            }

            bw.Write(Accounts.Count);
            foreach (Account account in Accounts)
            {
                bw.Write(account.ServerName);
                bw.Write(account.ServerAddress);

                bw.Write(account.Login);
                bw.Write(account.Password);
                bw.Write(account.SelectedRole);
                bw.Write(account.Roles.Count);
                foreach (string role in account.Roles)
                {
                    bw.Write(role);
                }

                bw.Write(account.Force);
                bw.Write(account.ShowOnline);

                bw.Write(account.AutoMessage);
                bw.Write(account.UseAutoMessage);
            }
            bw.Close();
        }
        static bool ConfigLoaded = false;
        public static void Load()
        {
            if (ConfigLoaded)
                return;
            ConfigLoaded = true;
            if (!File.Exists(path))
            {
                ServerList.InitializeServerList(GameServers);
            }
            else
            {
                BinaryReader br = new BinaryReader(File.OpenRead(path));
                try
                {

                    int serversCount = br.ReadInt32();
                    for (int i = 0; i < serversCount; i++)
                    {
                        GameServers.Add(new GameServer(br.ReadString(), br.ReadInt32(), br.ReadString()));
                    }

                    int accountsCount = br.ReadInt32();
                    for (int i = 0; i < accountsCount; i++)
                    {
                        Account account = new Account();
                        account.ServerName = br.ReadString();
                        account.ServerAddress = br.ReadString();

                        account.Login = br.ReadString();
                        account.Password = br.ReadString();
                        account.SelectedRole = br.ReadInt32();

                        int rolesCount = br.ReadInt32();
                        for (int j = 0; j < rolesCount; j++)
                        {
                            account.Roles.Add(br.ReadString());
                        }

                        account.Force = br.ReadBoolean();
                        account.ShowOnline = br.ReadBoolean();

                        account.AutoMessage = br.ReadString();
                        account.UseAutoMessage = br.ReadBoolean();

                        account.CompleteSettings();
                        Accounts.Add(account);
                    }
                }
                catch
                {
                    Accounts.Clear();
                    GameServers.Clear();
                    ServerList.InitializeServerList(GameServers);
                }
                br.Close();
            }
        }
    }
}

