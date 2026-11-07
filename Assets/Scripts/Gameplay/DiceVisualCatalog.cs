
using System.Collections.Generic;

namespace ManchTak
{
    public static class DiceVisualCatalog
    {
        private static readonly List<DiceVisualData> DiceList =
            new List<DiceVisualData>
            {
                new DiceVisualData(
                    DiceVisualType.Classic,
                    "کلاسیک",
                    "تاس اصلی منچ تک",
                    "Dice/Classic/dice",
                    "Dice/Classic/preview",
                    true,
                    1
                ),

                new DiceVisualData(
                    DiceVisualType.Golden,
                    "طلایی",
                    "تاس طلایی ویژه",
                    "Dice/Golden/dice",
                    "Dice/Golden/preview",
                    false,
                    5
                ),

                new DiceVisualData(
                    DiceVisualType.Diamond,
                    "الماسی",
                    "تاس درخشان الماسی",
                    "Dice/Diamond/dice",
                    "Dice/Diamond/preview",
                    false,
                    10
                ),

                new DiceVisualData(
                    DiceVisualType.Fire,
                    "آتشین",
                    "تاس با جلوه شعله‌های آتش",
                    "Dice/Fire/dice",
                    "Dice/Fire/preview",
                    false,
                    15
                ),

                new DiceVisualData(
                    DiceVisualType.Ice,
                    "یخی",
                    "تاس کریستالی یخی",
                    "Dice/Ice/dice",
                    "Dice/Ice/preview",
                    false,
                    20
                ),

                new DiceVisualData(
                    DiceVisualType.Lightning,
                    "صاعقه‌ای",
                    "تاس با جلوه الکتریکی",
                    "Dice/Lightning/dice",
                    "Dice/Lightning/preview",
                    false,
                    25
                ),

                new DiceVisualData(
                    DiceVisualType.Football,
                    "فوتبالی",
                    "تاس مخصوص علاقه‌مندان فوتبال",
                    "Dice/Football/dice",
                    "Dice/Football/preview",
                    false,
                    30
                ),

                new DiceVisualData(
                    DiceVisualType.Galaxy,
                    "کهکشانی",
                    "تاس با طراحی کهکشانی",
                    "Dice/Galaxy/dice",
                    "Dice/Galaxy/preview",
                    false,
                    35
                ),

                new DiceVisualData(
                    DiceVisualType.Royal,
                    "سلطنتی",
                    "تاس ویژه سلطنتی",
                    "Dice/Royal/dice",
                    "Dice/Royal/preview",
                    false,
                    40
                ),

                new DiceVisualData(
                    DiceVisualType.Neon,
                    "نئونی",
                    "تاس با نورپردازی نئونی",
                    "Dice/Neon/dice",
                    "Dice/Neon/preview",
                    false,
                    45
                ),

                new DiceVisualData(
                    DiceVisualType.Crystal,
                    "کریستالی",
                    "تاس شفاف کریستالی",
                    "Dice/Crystal/dice",
                    "Dice/Crystal/preview",
                    false,
                    50
                ),

                new DiceVisualData(
                    DiceVisualType.Shadow,
                    "سایه‌ای",
                    "تاس تاریک و مرموز",
                    "Dice/Shadow/dice",
                    "Dice/Shadow/preview",
                    false,
                    60
                )
            };

        public static IReadOnlyList<DiceVisualData> GetAll()
        {
            return DiceList.AsReadOnly();
        }

        public static DiceVisualData Find(DiceVisualType dice)
        {
            return DiceList.Find(item => item.DiceId == dice);
        }

        public static bool IsUnlocked(
            DiceVisualType dice,
            int playerLevel)
        {
            DiceVisualData data = Find(dice);

            if (data == null || playerLevel < 1)
                return false;

            return data.IsUnlocked ||
                   playerLevel >= data.UnlockLevel;
        }

        public static List<DiceVisualData> GetUnlocked(
            int playerLevel)
        {
            return DiceList.FindAll(
                item => IsUnlocked(item.DiceId, playerLevel)
            );
        }
    }
}
