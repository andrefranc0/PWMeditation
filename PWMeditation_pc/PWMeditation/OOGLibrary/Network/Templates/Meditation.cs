using System;
using System.Collections.Generic;
using System.Text;
using OOGLibrary.IO;
using OOGLibrary.IO.PacketBase.Client;
using OOGLibrary.IO.PacketBase.Server;
using OOGLibrary.GameTypes;
using PWMeditation;

namespace OOGLibrary.Network.Templates
{
    class Meditation
    {
        #region private
        Client Client { get; set; }
        bool enabled;
        #endregion
        #region Информация
        /// <summary>
        /// Начинает или прирывает медитацию
        /// </summary>
        public bool Enabled
        {
            get { return enabled; }
            set
            {
                Client.Send(new MeditationSetC86(UsedType, value));
            }
        }
        public int Today1 { get; private set; }
        public int Today2 { get; private set; }
        public int All { get; private set; }
        public int MeditationType { get; set; }
        public int UsedType { get; set; }
        #endregion
        /// <summary>
        /// Создаем экземпляр объекта Meditation
        /// </summary>
        /// <param name="client">Клиент</param>
        public Meditation(Client client)
        {
            MeditationType = -1;
            Client = client;
            client.Receive += client_Receive;

            /*All = new UnixTime(0);
            Today1 = new UnixTime(0);
            Today2 = new UnixTime(0);*/
        }
        private void client_Receive(object sender, ReceiveEventArgs e)
        {
            if (e.Stream is MeditationEnabledS14A)
            {
                enabled = ((MeditationEnabledS14A)e.Stream).Flag;
                RefreshMeditation();
            }
            if (e.Stream is MeditationInfoS149)
            {
                MeditationInfoS149 minfo = (MeditationInfoS149)e.Stream;

                if (minfo.Today1Updated)
                    Today1 = minfo.Today1;
                if (minfo.Today2Updated)
                    Today2 = minfo.Today2;
                All = minfo.Today;
                RefreshMeditation();
            }
        }

        /// <summary>
        /// Активирует нужный режим медитации
        /// </summary>
        public void RefreshMeditation()
        {
            if (All == 0) return;
            switch (MeditationType)
            {
                case 0:// Обычная
                    if (Today1 != 0 && !Enabled)
                    {
                        Start(0);
                    }
                    break;
                case 1:// Глубокая
                    if (Today2 != 0 && !Enabled)
                    {
                        Start(1);
                    }
                    break;
                case 2:// Сначала обычная, а потом глубокая
                    if (Today1 != 0 && (!Enabled || UsedType != 0))
                    {
                        Start(0);
                    }
                    else if (Today1 == 0 && Today2 != 0 && (!Enabled || UsedType != 1))
                    {
                        Start(1);
                    }
                    break;
                case 3:// Сначала глубокая, а потом обычная
                    if (Today2 != 0 && (!Enabled || UsedType != 1))
                    {
                        Start(1);
                    }
                    else if (Today2 == 0 && Today1 != 0 && (!Enabled || UsedType != 0))
                    {
                        Start(0);
                    }
                    break;
            }
        }
        /// <summary>
        /// Начать медитацию
        /// </summary>
        /// <param name="type">Тип медитации(0-обычная, 1-глубокая)</param>
        public void Start(int type)
        {
            UsedType = type;
            Start();
        }
        /// <summary>
        /// Начать медитацию
        /// </summary>
        public void Start()
        {
            Enabled = true;
        }
        /// <summary>
        /// Прервать медитацию
        /// </summary>
        public void Stop()
        {
            Enabled = false;
        }
    }
}
