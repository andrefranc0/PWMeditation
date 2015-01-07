using SiviliaFramework.Network.Security;
using SiviliaFramework.IO;
using System;
using System.Net.Sockets;
using SiviliaFramework.Data;

namespace SiviliaFramework.Network
{
    public class Connection
    {
        Socket socket;
        EncDec EncDec;

        object socketLock = new object();

        public OOGHost Host { get; private set; }
        private ConnectionData ConnectionData { get; set; }
        private ConnectionStatus ConnectionStatus { get; set; }

        public bool IsWork { get; private set; }

        public event EventHandler Connected;
        public event EventHandler Disconnected;

        SocketAsyncEventArgs socketArgsRecv;
        DataStream ReceiveStream;

        internal Connection(OOGHost oogHost, ConnectionData connectionData)
        {
            Host = oogHost;
            ConnectionData = connectionData;

            ReceiveStream = new DataStream();
            socketArgsRecv = new SocketAsyncEventArgs();

            socketArgsRecv.SetBuffer(new byte[1024], 0, 1024);

            socketArgsRecv.Completed += socketArgsRecv_Completed;
        }

        public void Connect()
        {
            lock (socketLock)
            {
                if (IsWork)
                    return;
                IsWork = true;
                ConnectionData.ConnectionOpening();
            }
            ConnectionStatus = ConnectionData.IncludeDataBlock<ConnectionStatus>();

            EncDec = ConnectionStatus.EncDec;
            ReceiveStream.Clear();

            try
            {
                socket = ConnectionStatus.GameServer.Connect();
                StartReceive();

                if (Connected != null)
                    Connected(this, new EventArgs());
            }
            catch
            {
                IsWork = false;
                if (Disconnected != null)
                    Disconnected(this, new EventArgs());
            }
        }
        public void Close()
        {
            lock (socketLock)
            {
                if (!IsWork) return;
                IsWork = false;
                ConnectionData.ConnectionClosing();
            }

            if (socket != null)
                socket.Close();

            if (Disconnected != null)
                Disconnected(this, new EventArgs());
        }


        // RECEIVE ASYNC
        private void StartReceive()
        {
            try
            {
                if (!socket.ReceiveAsync(socketArgsRecv))
                {
                    ReceiveProcess(socketArgsRecv);
                }
            }
            catch 
            {
                Close();
            }
        }
        private void socketArgsRecv_Completed(object sender, SocketAsyncEventArgs e)
        {
            ReceiveProcess(e);
        }
        private void ReceiveProcess(SocketAsyncEventArgs socketArgs)
        {
            if (!IsWork)
                return;
            try
            {
                if (socketArgs.SocketError == SocketError.SocketError ||
                    socketArgs.BytesTransferred == 0)
                {
                    this.Close();
                    return;
                }
                ConnectionStatus.ReceivedBytes += socketArgs.BytesTransferred;
                ReceiveStream.WriteBytes(EncDec.Decode(
                    socketArgs.Buffer,
                    socketArgs.BytesTransferred));
            }
            catch
            {
                Close();
            }

            Host.HandleReceivedStream(ReceiveStream);
            StartReceive();
        }

        public void Send(byte[] buffer)
        {
            if (!IsWork) return;
            try
            {
                lock (socket)
                {
                    buffer = EncDec.Encode(buffer);
                    socket.Send(buffer);
                    ConnectionStatus.SendedBytes += buffer.Length;
                }
            }
            catch
            {
                Close();
            }
        }
    }
}
