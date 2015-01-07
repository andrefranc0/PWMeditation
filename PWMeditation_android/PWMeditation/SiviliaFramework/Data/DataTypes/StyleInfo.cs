using System;
using System.Collections.Generic;
using System.Text;
using SiviliaFramework.IO;
using SiviliaFramework.IO.GameTypes;

namespace SiviliaFramework.Data.DataTypes
{
    public class StyleInfo
    {
        [FIELD] public string Name;
        [SKIP(7)]
        [FIELD] public Occupation Occupation;
        [FIELD] public Gender Gender;
        [FIELD] public byte[] Face;
    }
}
