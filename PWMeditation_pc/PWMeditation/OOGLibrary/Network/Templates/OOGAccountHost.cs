using System;
using System.Collections.Generic;
using System.Text;
using OOGLibrary.IO;
using OOGLibrary.IO.PacketBase;
using OOGLibrary.IO.PacketBase.Client;
using OOGLibrary.IO.PacketBase.Server;

namespace OOGLibrary.Network.Templates
{
    class OOGAccountHost
    {
        #region Флаги - текущий статус работы
        /// <summary>
        ///  Статус соединения с сервером
        /// </summary>
        public bool Work { get; private set; }
        /// <summary>
        /// Загружен ли список персонажей
        /// </summary>
        public bool PlayersLoaded { get; private set; }
        /// <summary>
        /// Статус входа в мир
        /// </summary>
        public bool EnteredTheWorld { get; private set; }
        #endregion
        #region Клиент
        /// <summary>
        /// Возвращает Client - основной класс для работы с протоколом
        /// </summary>
        public Client Client { get; private set; }
        #endregion
        #region Параметры аккаунта
        /// <summary>
        /// Логин
        /// </summary>
        string Login { get; set; }
        /// <summary>
        /// Пароль
        /// </summary>
        string Password { get; set; }
        /// <summary>
        /// Усиленный вход
        /// </summary>
        bool Force { get; set; }
        #endregion

        #region Евенты
        public event EventHandler Connected;
        public event EventHandler Disconnected;
        public event ReceiveEventHandler Receive;
        public event OnPlayersLoadedEventHandler OnPlayersLoaded;
        public event EventHandler OnPlayerEnterTheWorld;
        #endregion

        #region Персонажи на аккаунте
        OnPlayersLoadedEventArgs args { get; set; }
        /// <summary>
        /// Номер выбранного персонажа
        /// </summary>
        public int SelectedID { get; set; }
        /// <summary>
        /// Выбранный персонаж
        /// </summary>
        public RoleList_ReS53 SelectedPlayer { get; private set; }
        #endregion

        public Meditation Meditation { get; private set; }
        public AutoMessage AutoMessage { get; private set; }

        /// <summary>
        /// OOGAccountHost - основной класс для работы с подключением
        /// </summary>
        /// <param name="server">Сервер</param>
        /// <param name="login">Логин</param>
        /// <param name="password">Пароль</param>
        public OOGAccountHost(GameServer server, string login, string password) : this(server, login, password, false) { }
        /// <summary>
        /// OOGAccountHost - основной класс для работы с подключением
        /// </summary>
        /// <param name="server">Сервер</param>
        /// <param name="login">Логин</param>
        /// <param name="password">Пароль</param>
        /// <param name="force">Усиленный вход</param>
        public OOGAccountHost(GameServer server, string login, string password, bool force)
        {
            Client = new Network.Client(server);
            Login = login;
            Password = password;
            Force = force;

            Meditation = new Templates.Meditation(Client);
            AutoMessage = new Templates.AutoMessage(this);

            args = new OnPlayersLoadedEventArgs();

            Client.Connected += new EventHandler(Client_Connected);
            Client.Disconnected += new EventHandler(Client_Disconnected);
            Client.Receive += new ReceiveEventHandler(Client_Receive);
        }
        /// <summary>
        /// Начинаем работу
        /// </summary>
        public void BeginWork()
        {
            if (Work) return;
            Work = true;
            Client.Connect();
        }
        /// <summary>
        /// Входим в мир
        /// </summary>
        /// <param name="id">Номер персонажа</param>
        public void Enter(int id)
        {
            if (!PlayersLoaded) return;
            SelectedID = id;
            SelectedPlayer = args[id];
            Client.Send(new SelectRoleC46(SelectedPlayer.UID));
        }
        public void LoadFriendList()
        {
            if (!EnteredTheWorld) return;
            Send(new GetFriendListCCE(SelectedPlayer.UID));
        }
        /// <summary>
        /// Посылаем данные серверу
        /// </summary>
        /// <param name="ds">Поток данных</param>
        public void Send(DataStream ds)
        {
            if (ds is ISetUID)
            {
                ISetUID setuid = (ISetUID)ds;
                setuid.UID = SelectedPlayer.UID;
            }
            if (ds is ISetName)
            {
                ISetName setname = (ISetName)ds;
                setname.Name = SelectedPlayer.Name;
            }
            Client.Send(ds);
        }

        private void Client_Connected(object sender, EventArgs e)
        {
            if (Connected != null)
                Connected(this, e);
        }
        private void Client_Disconnected(object sender, EventArgs e)
        {
            Work = false;
            EnteredTheWorld = false;
            PlayersLoaded = false;
            if (Disconnected != null)
                Disconnected(this, e);
        }
        private void Client_Receive(object sender, ReceiveEventArgs e)
        {
            if (Receive != null)
                Receive(this, e);
            if (e.IsContainer) return;
            DataStream ds = (DataStream)e.Stream;
            if (ds is ContainerS00 && !EnteredTheWorld)
            {
                EnteredTheWorld = true;
                Client.Send(new GetInventoryC27());
                if (OnPlayerEnterTheWorld != null)
                    OnPlayerEnterTheWorld(this, new EventArgs());
            }
            if (ds is ServerInfoS01)
            {
                Client.Send(new LogginAnnounce((ServerInfoS01)ds, Login, Password));
            }
            if (ds is SMKeyS02)
            {
                Client.Send(new CMKeyC02((SMKeyS02)ds, Force));
            }
            if (ds is OnlineAnnounceS04)
            {
                Client.Send(new RoleListC52((OnlineAnnounceS04)ds));
                new KeepAliveSender(Client).Start();
            }
            if (ds is SelectRole_ReS47)
            {
                Client.Send(new EnterWorldC48(SelectedPlayer.UID));
            }
            if (ds is RoleList_ReS53)
            {
                RoleList_ReS53 role = (RoleList_ReS53)ds;
                if (role.IsChar)
                {
                    Client.Send(new RoleListC52(role));
                    args.Add(role);
                }
                else
                {
                    PlayersLoaded = true;
                    if (OnPlayersLoaded != null)
                        OnPlayersLoaded(this, args);
                }
            }
        }
    }
}
