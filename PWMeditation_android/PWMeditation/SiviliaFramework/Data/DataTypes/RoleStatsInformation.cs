using SiviliaFramework.IO;

namespace SiviliaFramework.Data.DataTypes
{
    public class RoleStatsInformation
    {
        [FIELD] public int FreeStats;  // Свободные статы(сила, кон и тд)
        [FIELD] public int AttackLevel;  // Уровень атаки
        [FIELD] public int DefLevel;  // Уровень защиты
        [FIELD] public int CriticalChance;  // Крит. шанс
        [FIELD] public int CriticalBonus;  // (Атака * (200+this) / 100)(?)
        [FIELD] public int Invisibility;  // Уровень скрытности
        [FIELD] public int ViewInvisibility;  // Уровень обнаружения
        [FIELD] public int MobAttack;  // Урон монстрам
        [FIELD] public int MobDef;  // Защита от монстров
        [FIELD] public int Con;  // Выносливость
        [FIELD] public int Int;  // Итнеллект
        [FIELD] public int Str;  // Сила
        [FIELD] public int Dex;  // Ловкость
        public int HP;
        [FIELD] public int HPMax;
        public int MP;
        [FIELD] public int MPMax; 
        [FIELD] public int HPRegen; 
        [FIELD] public int MPRegen; 
        [FIELD] public float WalkSpeed;  // Ходьба
        [FIELD] public float RunSpeed;  // Бег
        [FIELD] public float SwimSpeed;  // Плавание
        [FIELD] public float FlySpeed;  // Полет
        [FIELD] public int Accuracy;  // Меткость
        [FIELD] public int PhysAttackLow; 
        [FIELD] public int PhysAtkHigh; 
        [FIELD] public uint AttackSpeed;  // 20/this
        [FIELD] public float AttackDist; 
        [FIELD] public int MetalAttackMin;  // Всегда 0
        [FIELD] public int WoodAttackMin;  // Всегда 0
        [FIELD] public int WaterAttackMin;  // Всегда 0
        [FIELD] public int FireAttackMin;  // Всегда 0
        [FIELD] public int EathAttackMin;  // Всегда 0
        [FIELD] public int MetalAttackMax;  // Всегда 0
        [FIELD] public int WoodAttackMax;  // Всегда 0
        [FIELD] public int WaterAttackMax;  // Всегда 0
        [FIELD] public int FireAttackMax;  // Всегда 0
        [FIELD] public int EathAttackMax;  // Всегда 0
        [FIELD] public int MagAttackMin; 
        [FIELD] public int MagAttackMax; 
        [FIELD] public int MetalDef; 
        [FIELD] public int WoodDef; 
        [FIELD] public int WaterDef; 
        [FIELD] public int FireDef; 
        [FIELD] public int EarthDef; 
        [FIELD] public int PhysDef; 
        [FIELD] public int Evasion;  // Уклон
        [FIELD] public int Vigor;  // Макс. чи

    }
}
