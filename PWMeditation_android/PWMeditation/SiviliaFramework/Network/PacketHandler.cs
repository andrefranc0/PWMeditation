using SiviliaFramework.IO;
using System;
using System.Collections.Generic;

namespace SiviliaFramework.Network
{
    public delegate void PacketReceiveHandler(object sender, GamePacket e);
    public class PacketHandler
    {
        internal PacketHandler(OOGHost oogHost)
        {
            OOGHost = oogHost;
            handlerDict = new Dictionary<Type, HandlerInfo>();
        }

        private OOGHost OOGHost { get; set; }
        private Dictionary<Type, HandlerInfo> handlerDict { get; set; }

        internal HandlerInfo Handler<T>() where T : GamePacket
        {
            Type type = typeof(T);
            HandlerInfo handler;

            if (!handlerDict.TryGetValue(type, out handler))
            {
                handler = new HandlerInfo();
                handlerDict.Add(type, handler);
            }

            return handler;
        }

        internal void Handle(GamePacket packet)
        {
            HandlerInfo handler;
            if (handlerDict.TryGetValue(packet.GetType(), out handler))
            {
                handler.Handle(this, packet);
            }
        }
    }
    public class HandlerInfo
    {
        public event PacketReceiveHandler OnReceive;

        internal void Handle(object sender, GamePacket packet)
        {
            if (OnReceive != null)
                OnReceive(sender, packet);
        }
    }
}
