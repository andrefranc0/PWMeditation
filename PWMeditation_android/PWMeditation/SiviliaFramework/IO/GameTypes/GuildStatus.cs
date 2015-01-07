using System;
using System.Collections.Generic;
using System.Text;

namespace SiviliaFramework.IO.GameTypes
{
    public class GuildStatus
    {
        [FIELD] public byte Status;

        public override string ToString()
        {
            switch (Status)
            {
                case 2: return "Мастер";
                case 3: return "Маршал";
                case 4: return "Майор";
                case 5: return "Капитан";
                case 6: return "Член";
                default: return "null";
            }
        }
    }
}
