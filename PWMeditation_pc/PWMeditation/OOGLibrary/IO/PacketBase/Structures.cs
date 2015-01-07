using System;
using System.Collections.Generic;
using System.Text;

namespace OOGLibrary.IO.PacketBase
{
    interface IClientPacket
    {
        DataStream Serialize();
    }
    interface IClientContainer
    {
        DataStream Serialize();
    }
    interface IClientContainerC25
    {
        DataStream Serialize();
    }
    interface IServerPacket
    {
        DataStream Deserialize();
    }
    interface IServerContainer
    {
        DataStream Deserialize();
    }
    interface ISetUID
    {
        uint UID { get; set; }
    }
    interface ISetName
    {
        string Name { get; set; }
    }
}
