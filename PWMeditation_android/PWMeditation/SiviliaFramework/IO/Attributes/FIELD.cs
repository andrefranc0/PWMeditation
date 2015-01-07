using System;

namespace SiviliaFramework.IO
{
    [AttributeUsage(AttributeTargets.All)]
    public class FIELD : Attribute
    {
        public FieldType FieldType { get; set; }

        public FIELD()
        {
            FieldType = FieldType.Other;
        }
        public FIELD(FieldType fieldtype)
        {
            FieldType = fieldtype;
        }
    }
}
