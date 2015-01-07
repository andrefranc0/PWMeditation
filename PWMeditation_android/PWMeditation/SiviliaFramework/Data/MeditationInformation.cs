using System;
using System.Collections.Generic;
using System.Text;
using SiviliaFramework.IO;

namespace SiviliaFramework.Data
{
    public class MeditationInformation : DataBlock
    {
        public bool MeditationEnabled { get; set; }
        public uint SecondsToday { get; set; }

        public MeditationType[] MeditationTypes = new MeditationType[4];

        protected internal override void ConnectionClosing()
        {
            MeditationEnabled = false;
            SecondsToday = 0;
            foreach (MeditationType meditationType in MeditationTypes)
            {
                if (meditationType != null)
                {
                    meditationType.Seconds = 0;
                }
            }
        }
    }
    public class MeditationType
    {
        [FIELD] public uint Type;
        [FIELD] public uint Seconds;
        [FIELD] public uint Unknown;
    }
}
