using System;
using System.Collections.Generic;
using System.Text;
using SiviliaFramework.Data.DataTypes;

namespace SiviliaFramework.Data
{
    public class WorldRoleList : DataBlock
    {
        public Dictionary<uint, OtherRoleMainData> Roles = new Dictionary<uint, OtherRoleMainData>();


        protected internal override void ConnectionClosing()
        {
            lock (Roles)
            {
                Roles.Clear();
            }
        }
    }
}
