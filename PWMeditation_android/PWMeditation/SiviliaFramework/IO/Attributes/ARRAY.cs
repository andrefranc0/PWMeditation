using System;
using System.Reflection;

namespace SiviliaFramework.IO
{
    [AttributeUsage(AttributeTargets.All)]
    public class ARRAY : Attribute
    {
        /// <summary>
        /// Длина данных
        /// </summary>
        public string Length { get; private set; }
        /// <summary>
        /// Читать или писать данные в CUInt32
        /// </summary>
        public bool CUInt32 { get; private set; }
        /// <summary>
        /// Записывать ли длину(Length) в поток
        /// </summary>
        public bool WriteLength { get; private set; }

        public ARRAY()
        {
            WriteLength = true;
            CUInt32 = true;
        }
        public ARRAY(bool writeLength)
        {
            WriteLength = writeLength;
        }
        public ARRAY(int length)
        {
            WriteLength = true;
            Length = length.ToString();
        }
        public ARRAY(string length)
        {
            WriteLength = true;
            Length = length;
        }

        /// <summary>
        /// Читает длину массива из переменной или же из Length
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public int GetLength(object obj, Type type)
        {
            int value;
            if (int.TryParse(Length, out value))
                return value;

            return Convert.ToInt32(type.GetField(Length).GetValue(obj));
        }
        /// <summary>
        /// Записывает длину field в указаную переменную
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        /// <param name="field"></param>
        public void SetLength(object obj, Type type, FieldInfo field)
        {
            if (CUInt32 || !WriteLength) return;

            int length = 1;
            if (field.FieldType.IsArray)
            {
                object item = field.GetValue(obj);
                length = (item as Array).Length;
            }

            FieldInfo fieldLength = type.GetField(Length);

            switch (fieldLength.FieldType.Name)
            {
                case "Int32": fieldLength.SetValue(obj, (Int32)length); return;
                case "Int16": fieldLength.SetValue(obj, (Int16)length); return;
                case "UInt32": fieldLength.SetValue(obj, (UInt32)length); return;
                case "UInt16": fieldLength.SetValue(obj, (UInt16)length); return;
                case "Byte": fieldLength.SetValue(obj, (Byte)length); return;
                default: throw new Exception("Unknown type");
            }
        }
    }
}
