using SiviliaFramework.Network.Security;
using SiviliaFramework.Network;

namespace SiviliaFramework.Data
{
    public class ConnectionStatus : DataBlock
    {
        public GameServer GameServer { get; set; }
        public float ServerStatus { get; internal set; }

        internal EncDec EncDec { get; set; }
        internal MD5Hash MD5 { get; set; }

        internal byte[] ServerHash { get; set; }

        internal byte[] SMKey { get; set; }
        internal byte[] CMKey { get; set; }

        public long SendedBytes { get; internal set; }
        public long ReceivedBytes { get; internal set; }

        protected internal override void ConnectionOpening()
        {
            EncDec = new EncDec();
            MD5 = new MD5Hash();
            SendedBytes = 0;
            ReceivedBytes = 0;
        }
    }
}
