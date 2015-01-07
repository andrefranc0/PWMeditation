using SiviliaFramework.Data;
using SiviliaFramework.IO;

namespace SiviliaFramework.Network.Packets.Server
{
    [PacketIdentifier(0x55, IO.PacketType.ServerPacket)]
    public class CreateRole_ReS55 : GamePacket<AccountInformation>
    {
        [FIELD] public CreateRoleResultCode ResultCode;
        [FIELD] public uint AccountId;
        [FIELD] public uint UnkId;
        [FIELD] public RoleInfo Role;
        protected internal override void HandleData(AccountInformation data)
        {
            if(ResultCode.ResultCode == 0)
            {
                data.Roles.Add(Role);
            }
        }
    }
    public class CreateRoleResultCode
    {
        [FIELD] public uint ResultCode;

        public override string ToString()
        {
            switch(ResultCode)
            {
                case 00: return "Персонаж успешно создан";
                case 25: return "Запрещено использовать такой ник";
                case 45: return "Ник уже используется";
                default: return "Unknown error: " + ResultCode;
            }
        }
    }
}
