using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using OOGLibrary.IO.PacketBase.Client;

namespace OOGLibrary.Network.Templates
{
    class KeepAliveSender
    {
        Client C;
        AsyncCallback async;
        /// <summary>
        /// Статус работы
        /// </summary>
        public bool Work { get; private set; }
        /// <summary>
        /// KeepAliveSender - класс для поддержки подключения
        /// </summary>
        /// <param name="C">Екземпляр клиента</param>
        public KeepAliveSender(Client C)
        {
            this.C = C;
            async = new AsyncCallback(Send);
        }
        private void Send(IAsyncResult res)
        {
            if (!C.Work || !Work) return;
            C.Send(new KeepAliveC5A());
            Thread.Sleep(5000);
            async.BeginInvoke(null, null, null);
        }
        /// <summary>
        /// Начинает отправку KeepAlive
        /// </summary>
        public void Start()
        {
            if (Work) return;
            Work = true;
            async.BeginInvoke(null, null, null);
        }
        /// <summary>
        /// Прекращает отправку KeepAlive
        /// </summary>
        public void Stop()
        {
            Work = false;
        }
    }
}
