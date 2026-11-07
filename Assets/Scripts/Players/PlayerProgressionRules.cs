
namespace ManchTak
{
    public static class PlayerProgressionRules
    {
        public const int MaxLevel =
            LevelProgressionSystem.MaximumLevel;

        public const int WinnerXP = 138;
        public const int LoserXP = 50;

        // XP مورد نیاز برای رسیدن به سطح بعدی
        public static int GetRequiredXP(int currentLevel)
        {
            return LevelProgressionSystem
                .GetXPRequiredForNextLevel(currentLevel);
        }

        // XP دریافتی پس از مسابقه
        public static int GetMatchXP(bool isWinner)
        {
            return isWinner ? WinnerXP : LoserXP;
        }

        // محاسبه سطح جدید بازیکن
        // currentXP مقدار XP ذخیره‌شده در سطح فعلی است.
        public static int CalculateLevel(
            int currentLevel,
            int currentXP,
            out int remainingXP)
        {
            currentLevel = System.Math.Max(
                LevelProgressionSystem.MinimumLevel,
                currentLevel
            );

            currentLevel = System.Math.Min(
                LevelProgressionSystem.MaximumLevel,
                currentLevel
            );

            currentXP = System.Math.Max(0, currentXP);

            if (currentLevel >= MaxLevel)
            {
                remainingXP = 0;
                return MaxLevel;
            }

            long totalXP =
                LevelProgressionSystem.GetTotalXPForLevel(currentLevel)
                + currentXP;

            int newLevel =
                LevelProgressionSystem.CalculateLevel(totalXP);

            long levelStartXP =
                LevelProgressionSystem.GetTotalXPForLevel(newLevel);

            long xpInsideLevel = totalXP - levelStartXP;

            if (newLevel >= MaxLevel)
            {
                remainingXP = 0;
                return MaxLevel;
            }

            remainingXP = (int)System.Math.Max(
                0L,
                System.Math.Min(int.MaxValue, xpInsideLevel)
            );

            return newLevel;
        }
    }
}
