using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace OOGLibrary.Network
{
    class GameServer
    {
        public string Name { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public IPAddress IP
        {
            get
            {
                IPAddress[] IPArray = Dns.GetHostAddresses(Host);
                if (IPArray == null || IPArray.Length == 0) return null;
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
        public GameServer() { }
        public GameServer(string host, int port) : this("", host, port) { }
        public GameServer(string name, string host, int port)
        {
            Name = name;
            Host = host;
            Port = port;
        }
    }
}
