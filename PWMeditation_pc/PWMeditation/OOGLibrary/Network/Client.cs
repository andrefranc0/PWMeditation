using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using OOGLibrary.IO;

namespace OOGLibrary.Network
{
    class Client : IO.DataStreamHandler
    {
        Socket skt { get; set; }
        public bool Work { get; private set; }
        public GameServer Server { get; internal set; }

        public event EventHandler Connected;
        public event EventHandler Disconnected;

        DataStreamCreate create;
        AsyncCallback asyncrec;
        AsyncCallback asyncsend;
        byte[] buf;

        public Client() : this(new GameServer()) { }
        public Client(GameServer server)
        {
            Server = server;
            asyncrec = new AsyncCallback(EndReceive);
            asyncsend = new AsyncCallback(EndSend);
            buf = new byte[1024];
            create = new DataStreamCreate();
        }
        #region Open/Close
        public void Connect()
        {
            if (Work) return;
            Work = true;
            try
            {
                skt = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                skt.Connect(Server.EndPoint);
                if (Work)
                    skt.BeginReceive(buf, 0, buf.Length, SocketFlags.None, asyncrec, null);
                if (Connected != null)
                    Connected(this, new EventArgs());
            }
            catch
            {
                Work = false;
                if (Disconnected != null)
                    Disconnected(this, new EventArgs());
            }
        }
        public void Close()
        {
            if (!Work) return;
            Work = false;
            skt.Close();
            if (Disconnected != null)
                Disconnected(this, new EventArgs());
        }
        #endregion
        #region Receive
        private void EndReceive(IAsyncResult res)
        {
            if (!Work) return;
            try
            {
                int len = skt.EndReceive(res);
                if (len == 0) { Close(); return; }
                byte[] buf = Crypt.Decode(this.buf, len);
                foreach (byte bt in buf)
                {
                    DataStream ds = create.WriteByte(bt);
                    if (ds != null) Read(ds);
                }
                skt.BeginReceive(this.buf, 0, this.buf.Length, SocketFlags.None, asyncrec, null);
            }
            catch { Close(); }
        }
        #endregion
        #region Send
        List<DataStream> ToSend = new List<DataStream>();
        bool sendbegined;
        public void Send(DataStream ds)
        {
            ToSend.Add(ds);
            if (sendbegined) return;
            sendbegined = true;
            BeginSend();
        }
        private void BeginSend()
        {
            if (ToSend.Count == 0 || !Work)
            {
                sendbegined = false;
                return;
            }
            try
            {
                DataStream ds = ToSend[0];
                ToSend.RemoveAt(0);
                byte[] buf = Crypt.Encode(create.GetBytes(Write(ds)));
                skt.BeginSend(buf, 0, buf.Length, SocketFlags.None, asyncsend, null);
            }
            catch { Close(); }
        }
        private void EndSend(IAsyncResult res)
        {
            if (!Work) return;
            try
            {
                skt.EndSend(res);
                BeginSend();
            }
            catch { Close(); }
        }
        #endregion
    }
}
