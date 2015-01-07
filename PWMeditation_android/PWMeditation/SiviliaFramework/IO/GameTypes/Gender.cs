namespace SiviliaFramework.IO.GameTypes
{
    public class Gender
    {
        [FIELD] public byte GenderId;

        public Gender()
        {
        }
        public Gender(byte gender)
        {
            GenderId = gender;
        }

        public override string ToString()
        {
            return GenderId >= 1 ? "Женский" : "Мужской";
        }
        public string ToShortString()
        {
            return GenderId >= 1 ? "Ж" : "М";
        }
    }
}
