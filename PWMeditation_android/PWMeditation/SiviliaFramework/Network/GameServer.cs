using System;
using System.Net;
using System.Net.Sockets;

namespace SiviliaFramework.Network
{
    public class GameServer
    {
        public string Name { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public IPAddress IP
        {
            get
            {
                IPAddress[] IPArray = Dns.GetHostAddresses(Host);
                if (IPArray == null || IPArray.Length == 0)
                    throw new Exception("IPArray == null || IPArray.Length == 0");

                return IPArray[new Random().Next(IPArray.Length)];
            }
        }
        public IPEndPoint EndPoint
        {
            get
            {
                return new IPEndPoint(IP, Port);
            }
        }
        public Socket Connect()
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(EndPoint);

            return socket;
        }

        public GameServer() { }
        public GameServer(string server)
        {
            SetServer(server);
        }
        public GameServer(string host, int port) : this(host, port, "") { }
        public GameServer(string host, int port, string name)
        {
            Name = name;
            Host = host;
            Port = port;
        }

        public void SetServer(string server)
        {
            string[] args = server.Replace(" ", "").Split(':', ';', '=', '\t');
            if (args.Length == 0)
            {
                Host = "127.0.0.1";
                Port = 29000;
                return;
            }
            if (args.Length == 1)
            {
                Host = args[0];
                Port = 29000;
                return;
            }

            int port;

            if (int.TryParse(args[0], out port))
            {
                Port = port;
                Host = args[1];
            }
            else
            {
                if (int.TryParse(args[1], out port))
                {
                    Port = port;
                    Host = args[0];
                }
                else
                {
                    Host = args[0];
                    Port = 29000;
                }
            }
        }

        public override string ToString()
        {
            string srvName = string.IsNullOrEmpty(Name) ? "" : Name + " ";
            return String.Format("{0}{1}:{2}", srvName, Host, Port);
        }
        public string ToShortString()
        {
            return string.IsNullOrEmpty(Name) ? String.Format("{0}:{1}", Host, Port) : Name;
        }

        public GameServer Clone()
        {
            return new GameServer(Host, Port, Name);
        }
    }
}
