using System;
using SiviliaFramework.Data;

namespace SiviliaFramework.IO
{
    public class GamePacket
    {
        virtual internal protected void HandleData(ConnectionData connectionData)
        {
        }
    }
    public class GamePacket<T> : GamePacket where T : DataBlock
    {
        virtual internal protected void HandleData(T data)
        {
        }
        protected internal override void HandleData(ConnectionData connectionData)
        {
            T res;
            if (connectionData.TryGetDataBlock<T>(out res))
            {
                HandleData(res);
            }
        }
    }
}
