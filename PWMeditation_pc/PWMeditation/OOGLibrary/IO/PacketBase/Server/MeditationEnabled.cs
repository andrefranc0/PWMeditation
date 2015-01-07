using System;
using System.Collections.Generic;
using System.Text;

namespace OOGLibrary.IO.PacketBase.Server
{
  internal class MeditationEnabledS14A : DataStream, IServerContainer
  {
    public bool Flag { get; private set; }

    public DataStream Deserialize()
    {
      ReadDword();
      Flag = ReadBoolean();
      return this;
    }
  }
}
