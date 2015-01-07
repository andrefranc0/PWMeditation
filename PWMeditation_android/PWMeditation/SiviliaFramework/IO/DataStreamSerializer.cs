using System;
using System.Reflection;

namespace SiviliaFramework.IO
{
    public class DataStreamSerializer
    {
        public static object Deserialize(DataStream ds, Type type)
        {
            return Deserialize(ds, ds.IsSwaped, type);
        }
        public static object Deserialize(DataStream ds, bool swaped, Type type)
        {
            // Create object
            object ret = type.GetConstructor(new Type[0]).Invoke(new object[0]);

            // Deserialize(DataStream ds);
            if (ret is IDeserializableType)
            {
                bool saveSwap = ds.IsSwaped;
                ds.IsSwaped = swaped;

                ((IDeserializableType)ret).Deserialize(ds);

                ds.IsSwaped = saveSwap;
            }
            // Fields
            foreach (FieldInfo field in type.GetFields())
            {
                ReadField(type, field, ret, ds, swaped);
            }

            return ret;
        }

        #region Serialize
        public static void Serialize(object obj, DataStream ds)
        {
            Serialize(obj, ds, ds.IsSwaped);
        }
        public static void Serialize(object obj, DataStream ds, bool swaped)
        {
            Type type = obj.GetType();
            // Seriaize(DataStream ds);
            if (obj is ISerializableType)
            {
                bool saveSwap = ds.IsSwaped;
                ds.IsSwaped = swaped;

                ((ISerializableType)obj).Serialize(ds);

                ds.IsSwaped = saveSwap;
            }
            // Arrays. Записывает в указанные переменные длины массивов
            foreach (FieldInfo field in type.GetFields())
            {
                object[] attributes = field.GetCustomAttributes(typeof(ARRAY), false);
                foreach (object attribute in attributes)
                {
                    ARRAY array = attribute as ARRAY;
                    array.SetLength(obj, type, field);
                }
            }
            // Fields
            foreach (FieldInfo field in type.GetFields())
            {
                WriteField(type, field, obj, ds, swaped);
            }
        }
        #endregion

        #region ReadField
        private static void ReadField(Type type, FieldInfo field, object obj, DataStream ds, bool swaped)
        {
            object[] attributes = field.GetCustomAttributes(false);

            FIELD FieldInfo = null;
            int Length = 0;
            bool ReadCUInt32 = true;
            bool Swaped = swaped;
            bool HaveSwapAttribute = false;

            // Attributes
            foreach (object attribute in attributes)
            {
                if (attribute is FIELD)
                {
                    FieldInfo = attribute as FIELD;
                }
                if (attribute is ARRAY)
                {
                    ARRAY array = attribute as ARRAY;
                    if (array.CUInt32)
                        ReadCUInt32 = true;
                    else
                    {
                        ReadCUInt32 = false;
                        Length = array.GetLength(obj, type);
                    }
                }
                if (attribute is SWAP)
                {
                    Swaped = ((SWAP)attribute).Swaped;
                    HaveSwapAttribute = true;
                }
                if (attribute is IF)
                {
                    IF If = attribute as IF;
                    if (!If.Check(obj, type))
                        return;
                }
                if (attribute is SKIP)
                {
                    ds.Skip(((SKIP)attribute).Length);
                }
            }

            if (FieldInfo == null)
                return;

            // Read array
            if (field.FieldType.IsArray)
            {
                if (ReadCUInt32)
                {
                    Length = (int)ds.ReadCUInt32();
                }
                // Bytes
                if (field.FieldType.Name == "Byte[]")
                {
                    if(!HaveSwapAttribute)
                        Swaped = false;

                    field.SetValue(obj, ds.ReadBytes(Length, Swaped));
                    return;
                }

                // Other
                Type elementType = field.FieldType.GetElementType();
                Array array = Array.CreateInstance(elementType, Length);

                for (int i = 0; i < array.Length; i++)
                {
                    object data;
                    ds.Read(out data, elementType, FieldInfo.FieldType, Swaped);
                    array.SetValue(data, i);
                }

                field.SetValue(obj, array);
                return;
            }
            // Read value
            else
            {
                object data;
                ds.Read(out data, field.FieldType, FieldInfo.FieldType, Swaped);
                field.SetValue(obj, data);
            }
        }
        #endregion
        #region WriteField
        private static void WriteField(Type type, FieldInfo field, object obj, DataStream ds, bool swaped)
        {
            object[] attributes = field.GetCustomAttributes(false);

            FIELD FieldInfo = null;
            bool WriteCUInt32 = true;
            bool Swaped = swaped;
            bool HaveSwapAttribute = false;

            // Attributes
            foreach (object attribute in attributes)
            {
                if (attribute is FIELD)
                {
                    FieldInfo = attribute as FIELD;
                }
                if (attribute is ARRAY)
                {
                    ARRAY array = attribute as ARRAY;
                    WriteCUInt32 = array.CUInt32;
                }
                if (attribute is SWAP)
                {
                    Swaped = ((SWAP)attribute).Swaped;
                    HaveSwapAttribute = true;
                }
                if (attribute is IF)
                {
                    IF If = attribute as IF;
                    if (!If.Check(obj, type))
                        return;
                }
            }

            if (FieldInfo == null)
                return;

            // Write array
            if (field.FieldType.IsArray)
            {
                Array array = (Array)field.GetValue(obj);
                Type elementType = field.FieldType.GetElementType();

                if (WriteCUInt32)
                {
                    ds.WriteCUInt32((uint)array.Length);
                }

                // Bytes
                if (field.FieldType.Name == "Byte[]")
                {
                    if (!HaveSwapAttribute)
                        Swaped = false;
                    ds.WriteBytes((byte[])array, Swaped);
                    return;
                }

                // Other

                foreach (object item in array)
                {
                    ds.Write(item, Swaped, FieldInfo.FieldType);
                }

                return;
            }
            // Write value
            else
            {
                object data = field.GetValue(obj);
                ds.Write(data, Swaped, FieldInfo.FieldType);
            }
        }
        #endregion
    }
}
