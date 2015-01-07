using System;
using System.Collections.Generic;
using System.Text;
using OOGLibrary.IO;
using OOGLibrary.IO.PacketBase.Client;
using OOGLibrary.IO.PacketBase.Server;

namespace OOGLibrary.Network.Templates
{
    delegate void ReceiveMessageEventHandler(object sender, ReceiveMessageEventArgs  e);
    class ReceiveMessageEventArgs
    {
        public ReceiveMessageEventArgs(string name, string message)
        {
            Name = name;
            Message = message;
        }

        public string Name { get; private set; }
        public string Message { get; private set; }
    }
    class AutoMessage
    {
        OOGAccountHost C;

        public event ReceiveMessageEventHandler OnMessageReceived;

        public string Message { get; set; }
        public bool Enabled { get; set; }

        public AutoMessage(OOGAccountHost C)
        {
            this.C = C;
            C.Receive += C_Receive;
        }

        void C_Receive(object sender, ReceiveEventArgs e)
        {
            if (e.Stream is PrivateMessageS60)
            {
                PrivateMessageS60 privatemessage = (PrivateMessageS60)e.Stream;

                if (privatemessage.MessageType == 0 && OnMessageReceived != null)
                    OnMessageReceived(this, new ReceiveMessageEventArgs(privatemessage.RecvFromName, privatemessage.Message));

                if (privatemessage.MessageType != 0 || // Нам прислали автоответ или еще что-то
                    !C.PlayersLoaded || // Мы еще не в игре, хотя это глупо
                    !Enabled || // Автоответ отключен
                    string.IsNullOrEmpty(Message)) return; // Нет сообщения для автоответа

                if (Message.Length > 0x4F)
                    Message = Message.Substring(0, 0x4F);

                PrivateMessageC60 sendmessage = new PrivateMessageC60(true, privatemessage.RecvFromName, privatemessage.RecvFromUID, Message);
                C.Send(sendmessage);
            }
        }
    }
}
