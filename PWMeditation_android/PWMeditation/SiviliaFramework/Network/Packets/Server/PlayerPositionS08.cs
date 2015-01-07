using SiviliaFramework.Data;
using SiviliaFramework.IO.GameTypes;
using SiviliaFramework.IO;

namespace SiviliaFramework.Network.Packets.Server
{
    [PacketIdentifier(0x08, IO.PacketType.ServerContainer)]
    public class PlayerPositionS08 : GamePacket<AccountInformation> 
    {
        [FIELD] public int Experience;
        [FIELD] public int Spirit;
        [FIELD] public uint UID;
        [FIELD] public Point3F Position;

        protected internal override void HandleData(AccountInformation data)
        {
            RoleInfo role = data.SelectedRole;

            role.Experience = Experience;
            role.Spirit = Spirit;
            role.Position = Position;

            data.EnteredWorld = true;
        }
    }
}
