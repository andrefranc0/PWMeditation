using SiviliaFramework.Data;
using SiviliaFramework.IO;
using System;

namespace SiviliaFramework.Network
{
    public delegate void OnPacketReceiveEventHandler(object sender, PacketReceiveEventArgs e);
    public class PacketReceiveEventArgs
    {
        public PacketIdentifier PacketId { get; private set; }
        public DataStream PacketStream { get; private set; }
        public PacketReceiveEventArgs(PacketIdentifier packetId, DataStream packetStream)
        {
            PacketId = packetId;
            PacketStream = packetStream;
        }

        public void Reset()
        {
            PacketStream.Reset();
        }
    }
    public class OOGHost
    {
        public Connection Connection { get; private set; }
        private ConnectionData Data { get; set; }
        private PluginManager pluginManager { get; set; }
        private PacketHandler packetHandler { get; set; }

        private ConnectionStatus ConnectionStatus;

        public event OnPacketReceiveEventHandler OnPacketReceive;

        public HandlerInfo PacketHandler<T>() where T : GamePacket
        {
            return packetHandler.Handler<T>();
        }
        public T IncludePlugin<T>() where T : Plugin
        {
            return pluginManager.IncludePlugin<T>();
        }
        public T IncludeDataBlock<T>() where T : DataBlock
        {
            return Data.IncludeDataBlock<T>();
        }
        public bool TryGetDataBlock<T>(out T data) where T : DataBlock
        {
            return Data.TryGetDataBlock<T>(out data);
        }

        public OOGHost(string host) : this(new GameServer(host))
        {
        }
        public OOGHost(string host, int port) : this(new GameServer(host, port))
        {
        }
        public OOGHost(GameServer gameServer)
        {
            packetHandler = new PacketHandler(this);
            pluginManager = new PluginManager(this);

            // Блок с клиентскими данными
            Data = new ConnectionData(this);
            ConnectionStatus = Data.IncludeDataBlock<ConnectionStatus>();
            ConnectionStatus.GameServer = gameServer;

            // Оболочка для Socket с шифрацией и дешифрацией трафика
            Connection = new Connection(this, Data);

            PacketsRegistry.RegisterPackets();
        }

        public void Connect()
        {
            if (Connection.IsWork) return;


            Connection.Connect();
            pluginManager.Open();
        }
        public void Close()
        {
            if (!Connection.IsWork) return;

            pluginManager.Close();
            Connection.Close();
        }

        public void Send(GamePacket packet)
        {
            if (Connection.IsWork)
            {
                Type type = packet.GetType();

                object[] attributes = type.GetCustomAttributes(typeof(PacketIdentifier), false);
                if (attributes.Length == 0)
                    throw new Exception("Unknown type");

                PacketIdentifier packetId = attributes[0] as PacketIdentifier;

                if (packetId.PacketType == PacketType.Unknown)
                    throw new Exception("Unknown pack");

                packet.HandleData(Data);

                DataStream packetStream = new DataStream();
                if (packetId.PacketType != PacketType.ClientContainer
                    )
                    packetStream.IsSwaped = true;

                DataStreamSerializer.Serialize(packet, packetStream);

                Send(packetId.PacketId, packetStream.GetBytes(), packetId.PacketType); 
            }
        }

        public void Send(uint packetId, DataStream ds, PacketType packetType)
        {
            Send(packetId, ds.GetBytes(), packetType);
        }
        
        private void Send(uint packetId, byte[] packetBuffer, PacketType packetType)
        {
            if (Connection.IsWork)
            {
                DataStream sendData = new DataStream();
                switch (packetType)
                {
                    case PacketType.ClientPacket: // cuint(Type) cuint(Len) byte[](Buffer)
                        sendData.WriteCUInt32(packetId);
                        sendData.WriteCUInt32((uint)packetBuffer.Length);
                        sendData.WriteBytes(packetBuffer);

                        Connection.Send(sendData.GetBytes());

                        return;

                    case PacketType.ClientContainer: // word(Type) byte[](Buffer)
                        sendData.WriteCUInt32((uint)(2 + packetBuffer.Length));
                        sendData.WriteWord((ushort)packetId);
                        sendData.WriteBytes(packetBuffer);

                        Send(0x22, sendData.GetBytes(), PacketType.ClientPacket);

                        return;
                    case PacketType.ClientContainerC25: // dword(Type) int32(Len) byte[](Buffer)

                        sendData.WriteDword(packetId);
                        sendData.WriteInt32(packetBuffer.Length);
                        sendData.WriteBytes(packetBuffer);

                        Send(0x25, sendData.GetBytes(), PacketType.ClientContainer);

                        return;
                }
            }
        }

        internal void HandleReceivedStream(DataStream ds)
        {
            while (true)
            {
                uint type;
                uint len;
                DataStream dataStream;

                if (!ds.TryReadCUInt32(out type))
                    break;
                if (!ds.TryReadCUInt32(out len))
                    break;
                if (!ds.CanRead((int)len))
                    break;

                dataStream = new DataStream(ds.ReadBytes((int)len));
                dataStream.IsSwaped = true;
                InitializePacket(type, PacketType.ServerPacket, dataStream);

                if (type == 0x00) // Если это контейнер, то читаем из него данные
                {
                    dataStream.Reset();
                    dataStream.IsSwaped = false;

                    while (dataStream.CanRead())
                    {
                        uint containerType;
                        uint containerLen;
                        DataStream containerDataStream;
                        PacketType packetType;

                        if (dataStream.ReadCUInt32() == 0x22) // type
                        {
                            dataStream.ReadCUInt32(); // buffer len

                            containerLen = dataStream.ReadCUInt32() - 2;
                            containerType = dataStream.ReadWord();
                            containerDataStream = new DataStream(dataStream.ReadBytes((int)containerLen));

                            packetType = PacketType.ServerContainer;
                        }
                        else
                        {
                            containerType = 0x45;
                            containerDataStream = dataStream.ReadDataStream();
                            containerDataStream.IsSwaped = true;

                            packetType = PacketType.ServerPacket;
                        }
                        InitializePacket(containerType, packetType, containerDataStream);
                    }
                }
                ds.Flush();
            }
            ds.Reset();
        }
        private void InitializePacket(uint opcode, PacketType packetType, DataStream ds)
        {
            if(OnPacketReceive != null)
            {
                OnPacketReceive(this, new PacketReceiveEventArgs(new PacketIdentifier(opcode, packetType), ds));
                ds.Reset();
            }

            Type type = null;
            if (packetType == PacketType.ServerPacket) type = PacketsRegistry.GetPacket(opcode);
            if (packetType == PacketType.ServerContainer) type = PacketsRegistry.GetContainer(opcode);

            if (type == null)
                return;
            GamePacket packet;
            try
            {
                packet = DataStreamSerializer.Deserialize(ds, type) as GamePacket;
            }
            catch { return; }
            packet.HandleData(Data);
            packetHandler.Handle(packet);
        }
    }
}
