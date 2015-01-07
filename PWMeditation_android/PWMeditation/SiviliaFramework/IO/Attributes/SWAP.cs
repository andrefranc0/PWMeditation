using System;

namespace SiviliaFramework.IO
{
    [AttributeUsage(AttributeTargets.All)]
    public class SWAP : Attribute
    {
        /// <summary>
        /// Порядок байт
        /// </summary>
        public bool Swaped { get; private set; }
        public SWAP()
        {
            Swaped = true;
        }
        public SWAP(bool swaped)
        {
            Swaped = swaped;
        }
    }
}
