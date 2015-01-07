using System;
using System.Globalization;

namespace SiviliaFramework.IO
{
    public enum ClauseType
    {
        Equal = 0,
        NotEqual = 1,
        Mask
    }

    [AttributeUsage(AttributeTargets.All)]
    public class IF : Attribute
    {
        /// <summary>
        /// Название переменной, значение которой надо проверять
        /// </summary>
        public string Name;
        /// <summary>
        /// Условия
        /// </summary>
        public ClauseType ClauseType;
        /// <summary>
        /// Второй значение
        /// </summary>
        public object Value;

        /// <summary>
        /// ClauseType = Equal, Value = true
        /// </summary>
        /// <param name="name"></param>
        public IF(string name)
        {
            this.Name = name;
            this.Value = true;
            ClauseType = ClauseType.Equal;
        }
        /// <summary>
        /// ClauseType = Equal
        /// </summary>
        /// <param name="name">Название проверяемой переменной(</param>
        /// <param name="value">Значение</param>
        public IF(string name, object value)
        {
            this.Name = name;
            this.Value = value;
            this.ClauseType = ClauseType.Equal;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">Название проверяемой переменной</param>
        /// <param name="clauseType">Условие</param>
        /// <param name="value">Значение</param>
        public IF(string name, ClauseType clauseType, object value)
        {
            this.Name = name;
            this.Value = value;
            this.ClauseType = clauseType;
        }

        public bool Check(object obj, Type type)
        {
            string value1 = type.GetField(Name).GetValue(obj).ToString();
            string value2 = Value.ToString();

            switch (ClauseType)
            {
                case ClauseType.Equal: return value1 == value2;
                case ClauseType.NotEqual: return value1 != value2;
                case ClauseType.Mask:
                    {
                        uint v1;
                        uint v2;

                        if (!TryParseUInt32(value1, out v1) ||
                            !TryParseUInt32(value2, out v2))
                            throw new Exception("Can't check mask");

                        return (v1 & v2) != 0;
                    }
                default: throw new Exception("Unknown clause type");
            }
        }
        private bool TryParseUInt32(string s, out uint value)
        {
            if (s.Length < 2 || s.Substring(0, 2) != "0x")
            {
                return uint.TryParse(s, out value);
            }

            try
            {
                value = Convert.ToUInt32(s, 16);
                return true;
            }
            catch
            {
                value = 0;
                return false;
            }
        }
    }
}
