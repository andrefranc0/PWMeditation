namespace SiviliaFramework.IO.GameTypes
{
    public class Occupation
    {
        [FIELD] public byte OccupationId;

        public Occupation()
        {
        }
        public Occupation(byte occupation)
        {
            OccupationId = occupation;
        }

        public override string ToString()
        {
            switch (OccupationId)
            {
                case 0: return "Воин";
                case 1: return "Маг";
                case 2: return "Шаман";
                case 3: return "Друид";
                case 4: return "Оборотень";
                case 5: return "Убийца";
                case 6: return "Лучник";
                case 7: return "Жрец";
                case 8: return "Страж";
                case 9: return "Мистик";

                default: return "Unknown";
            }
        }
        public string ToShortString()
        {
            switch (OccupationId)
            {
                case 0: return "Вар";
                case 1: return "Маг";
                case 2: return "Шам";
                case 3: return "Дру";
                case 4: return "Танк";
                case 5: return "Син";
                case 6: return "Лук";
                case 7: return "Жрец";
                case 8: return "Сик";
                case 9: return "Мист";

                default: return "unk";
            }
        }
    }
}
