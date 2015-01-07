using SiviliaFramework.IO.GameTypes;
using SiviliaFramework.IO;
using System.Collections.Generic;
using SiviliaFramework.Data.DataTypes;

namespace SiviliaFramework.Data
{
    public class AccountInformation : DataBlock
    {
        public AccountInformation()
        {
            Roles = new List<RoleInfo>();
        }

        public string Login { get; internal set; }

        public bool RolesLoaded { get; internal set; }

        public uint AccountID { get; set; }
        public uint UnkID { get; set; }

        public UnixTime LastLoginTime { get; set; }
        public string LastLoginIP { get; set; }
        public string CurrentIP { get; set; }

        public RoleInfo SelectedRole { get; set; }
        public List<RoleInfo> Roles { get; set; }

        public bool EnteredWorld { get; set; }

        protected internal override void ConnectionOpening()
        {
            Roles.Clear();
            SelectedRole = null;
        }
        protected internal override void ConnectionClosing()
        {
            EnteredWorld = false;
        }
    }

    public class RoleInfo
    {
        public RoleInfo()
        {
            Stats = new RoleStatsInformation();
        }
        public RoleStatsInformation Stats;

        public int Experience;
        public int MaxExperience
        {
            get
            {
                if (Level > 105) return 1800000000;
                return ExperienceTable[Level];
            }
        }
        public float ExpPercent
        {
            get
            {
                if (MaxExperience == 0) return 0;
                float ret = ((float)Experience / (float)MaxExperience) * 100;
                if (ret > 100) ret = 100;
                if (ret < 0) ret = 0;
                return ret;
            }
        }
        public int Spirit;

        [FIELD] public uint UID;
        [FIELD] public Gender Gender;
        [FIELD] public byte Race;
        [FIELD] public Occupation Occupation;
        [FIELD] public int Level;
        [FIELD] public int Unk;
        [FIELD] public string Name;
        [FIELD] public byte[] Face;

        [FIELD] public EquipInfo[] EquipList;

        [FIELD] public bool Activate;

        [FIELD] public UnixTime DeleteTime;
        [FIELD] public UnixTime CreateTime;
        [FIELD] public UnixTime LastOnline;

        [FIELD] public Point3F Position;
        [FIELD] public int WorldId;

        private static int[] ExperienceTable = new int[]
            {
                0,
                55,
                220,
                495,
                880,
                1400,
                2010,
                2765,
                3640,
                4635,
                5750,
                7040,
                8400,
                9945,
                11620,
                13500,
                15440,
                17595,
                19890,
                22325,
                25000,
                27825,
                30800,
                34040,
                37440,
                41125,
                44980,
                49140,
                53480,
                58145,
                63000,
                68045,
                73600,
                79365,
                85510,
                92050,
                99000,
                106190,
                113810,
                122070,
                130600,
                139810,
                149520,
                159745,
                170720,
                182250,
                194350,
                207270,
                221040,
                235445,
                250750,
                267240,
                284440,
                302895,
                322380,
                343200,
                365120,
                388455,
                413540,
                439845,
                468000,
                514186,
                564426,
                618633,
                677751,
                742105,
                811633,
                887040,
                968679,
                1057360,
                1153044,
                1257081,
                1437950,
                1640333,
                1866865,
                2119163,
                2400221,
                2712606,
                3060712,
                3446869,
                3875270,
                4350544,
                4876697,
                5457952,
                6102480,
                6813012,
                7598978,
                8464893,
                9418742,
                10470584,
                11688300,
                13394199,
                14933072,
                16613613,
                18447312,
                20448750,
                22631232,
                25006212,
                27596408,
                30412503,
                66954000,
                267816000,
                535632000,
                1339080000,
                1750000000,
                1750000000
            };
    }
    public class EquipInfo
    {
        [FIELD] public int ID;
        [FIELD] public int CellID;
        [FIELD] public int Count;
        [FIELD] public int MaxCount;
        [FIELD] public byte[] ItemData;

        [FIELD] public uint ProcType;
        [FIELD] public uint ExpireDate;
        [FIELD] public uint Unk1;
        [FIELD] public uint Unk2;
        [FIELD] public uint Mask;
    }
}
