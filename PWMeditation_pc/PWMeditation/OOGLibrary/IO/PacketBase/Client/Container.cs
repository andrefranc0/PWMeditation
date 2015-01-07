using System;
using System.Collections.Generic;
using System.Text;

namespace OOGLibrary.IO.PacketBase.Client
{
    class ContainerC22 : DataStream, IClientContainer
    {
        public uint ContainerType { get; private set; }
        public byte[] ContainerBuffer { get; private set; }
        public ContainerC22(byte[] array)
        {
            Array.Reverse(array, 0, 2);
            ContainerType = BitConverter.ToUInt16(array, 0);
            byte[] buffer = new byte[array.Length - 2];
            Array.Copy(array, 2, buffer, 0, buffer.Length);
            ContainerBuffer = buffer;
        }
        public DataStream Serialize()
        {
            Type = ContainerType;
            Write(ContainerBuffer);
            return this;
        }
    }
}
