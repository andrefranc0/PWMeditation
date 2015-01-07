using System.Collections.Generic;
using SiviliaFramework.Network;
using System;

namespace SiviliaFramework.Data
{
    public class DataBlock
    {
        public OOGHost Host { get; internal set; }

        virtual protected internal void ConnectionOpening() { }
        virtual protected internal void ConnectionClosing() { }
        virtual public void Reset() { }
    }
    public class ConnectionData : DataBlock
    {
        internal ConnectionData(OOGHost oogHost)
        {
            Host = oogHost;
            DataBlocks = new Dictionary<Type, DataBlock>();
        }
        private Dictionary<Type, DataBlock> DataBlocks { get; set; }

        internal bool TryGetDataBlock<T>(out T data) where T : DataBlock
        {
            Type type = typeof(T);
            DataBlock res;
            if (DataBlocks.TryGetValue(type, out res))
            {
                data = (T)res;
                return true;
            }
            data = default(T);
            return false;
        }
        internal T IncludeDataBlock<T>() where T : DataBlock
        {
            Type type = typeof(T);
            DataBlock res;
            if (DataBlocks.TryGetValue(type, out res))
                return (T)res;

            res = (T)type.GetConstructor(new Type[0]).Invoke(new object[0]);
            res.Host = Host;

            DataBlocks.Add(type, res);
            return (T)res;
        }

        protected internal override void ConnectionOpening()
        {
            foreach (DataBlock dataBlock in DataBlocks.Values)
                dataBlock.ConnectionOpening();
        }
        protected internal override void ConnectionClosing()
        {
            foreach (DataBlock dataBlock in DataBlocks.Values)
                dataBlock.ConnectionClosing();
        }
    }
}
