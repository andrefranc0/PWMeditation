using System;

namespace SiviliaFramework.IO
{
    [AttributeUsage(AttributeTargets.All)]
    public class SKIP : Attribute
    {
        public int Length { get; private set; }
        public SKIP(int length)
        {
            Length = length;
        }
    }
}
