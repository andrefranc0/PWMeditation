using System;
using System.Collections.Generic;
using System.Text;
using SiviliaFramework.IO;
using SiviliaFramework.IO.GameTypes;
using SiviliaFramework.Data;

namespace SiviliaFramework.Network.Packets.Client
{
    [PacketIdentifier(0x54, IO.PacketType.ClientPacket)]
    public class CreateRoleC54 : GamePacket<AccountInformation>
    {
        static byte[] ConstUnkData = new byte[512];

        public CreateRoleC54(string name, byte gender, byte occupation, byte[] face)
        {
            Name = name;
            Gender = new Gender(gender);
            Occupation = new Occupation(occupation);
            Face = face;

            UnkData = ConstUnkData;
        }

        [FIELD] public uint AccountID;
        [FIELD] public uint Unk1;
        [FIELD] public uint Unk2 = 0xFFFFFFFF;
        [FIELD] public Gender Gender;
        [FIELD] public byte Race;
        [FIELD] public Occupation Occupation;
        [FIELD] public uint Level;
        [FIELD] public uint Unk3;
        [FIELD] public string Name;
        [FIELD] public byte[] Face;
        [FIELD][ARRAY(false)] public byte[] UnkData;

        protected internal override void HandleData(AccountInformation data)
        {
            AccountID = data.AccountID;
        }
    }
}
