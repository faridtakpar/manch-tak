
namespace ManchTak
{
    public static class PlayerProgressionRules
    {
        public const int MaxLevel = 200;
        public const int WinnerXP = 138;
        public const int LoserXP = 50;

        // XP مورد نیاز برای رسیدن به سطح بعدی
        public static int GetRequiredXP(int currentLevel)
        {
            if (currentLevel >= MaxLevel)
                return 0;

            if (currentLevel < 1)
                currentLevel = 1;

            return 100 + (currentLevel * 50);
        }

        // XP دریافتی پس از مسابقه
        public static int GetMatchXP(bool isWinner)
        {
            return isWinner ? WinnerXP : LoserXP;
        }

        // محاسبه سطح جدید بازیکن
        public static int CalculateLevel(
            int currentLevel,
            int currentXP,
            out int remainingXP)
        {
            currentLevel = System.Math.Max(1, currentLevel);
            currentXP = System.Math.Max(0, currentXP);

            while (currentLevel < MaxLevel)
            {
                int requiredXP = GetRequiredXP(currentLevel);

                if (currentXP < requiredXP)
                    break;

                currentXP -= requiredXP;
                currentLevel++;
            }

            if (currentLevel >= MaxLevel)
                currentXP = 0;

            remainingXP = currentXP;
            return currentLevel;
        }
    }
}
