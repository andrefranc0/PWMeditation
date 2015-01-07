using System;
using System.Collections.Generic;
using System.Text;
using OOGLibrary.IO.PacketBase.Client;
using OOGLibrary.IO.PacketBase.Server;
using OOGLibrary.Cryptography;

namespace OOGLibrary.IO
{
    delegate void ReceiveEventHandler(object sender, ReceiveEventArgs e);
    class ReceiveEventArgs
    {
        public DataStream Stream { get; private set; }
        public bool IsContainer { get; private set; }
        public uint Type { get; private set; }
        public ReceiveEventArgs(DataStream ds, bool container)
        {
            Stream = ds;
            Type = ds.Type;
            IsContainer = container;
        }
    }
    class DataStreamHandler
    {
        public event ReceiveEventHandler Receive;

        DataSerializer Serializer { get; set; }
        internal EncDec Crypt { get; set; }
        MD5Hash md5 { get; set; }

        public DataStreamHandler()
        {
            Serializer = new DataSerializer();
            Crypt = new EncDec();
            md5 = new MD5Hash();
        }
        protected void Read(DataStream ds)// Десериализация пакета и разбиение его на контейнеры
        {
            ds = Serializer.Deserialize(ds);
            if (Receive != null)
                Receive(this, new ReceiveEventArgs(ds, false));
            if (ds is ContainerS00)
            {
                ContainerS00 container = (ContainerS00)ds;
                for (int i = 0; i < container.Count; i++)
                {
                    if (Receive != null)
                    {
                        ReceiveEventArgs args = new ReceiveEventArgs(Serializer.DeserializeContainer(container[i]), true);
                        Receive(this, args);
                    }
                }
            }
        }
        protected DataStream Write(DataStream ds)// Сериализация пакета, генерация ключей
        {
            if (ds is LogginAnnounce)
                ((LogginAnnounce)ds).GenHash(md5);
            if (ds is CMKeyC02)
            {
                CMKeyC02 key = (CMKeyC02)ds;
                Crypt.CreateEnc(md5.GetKey(key.ServerKey));
                Crypt.CreateDec(md5.GetKey(key.ClientKey));
            }
            return Serializer.Serialize(ds);
        }
    }
}
