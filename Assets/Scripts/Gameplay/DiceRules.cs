
using System;

namespace ManchTak
{
    public enum DiceType
    {
        Classic,
        Diamond,
        Ruby,
        Emerald,
        Gold,
        Silver,
        Ice,
        Fire,
        Galaxy,
        Neon,
        Crystal,
        Glass,
        BlackLuxury,
        Football,
        Electric,
        RoyalPurple,
        OceanBlue,
        RedFire,
        Space,
        Tournament,
        Legendary,
        SpecialEmerald,
        AnimatedGold,
        SparklingDiamond
    }

    public static class DiceRules
    {
        public const int MinimumDiceValue = 1;
        public const int MaximumDiceValue = 6;

        public const int ConsecutiveSixLimit = 3;
        public const int RetryDiamondCost = 2;

        // اعتبارسنجی عدد تاس
        public static bool IsValidRoll(int value)
        {
            return value >= MinimumDiceValue &&
                   value <= MaximumDiceValue;
        }

        // بررسی سه عدد شش متوالی
        public static bool HasThreeConsecutiveSixes(
            int previousSixCount,
            int currentRoll)
        {
            return previousSixCount >= 2 &&
                   currentRoll == 6;
        }

        // تعداد شش‌های متوالی
        public static int UpdateConsecutiveSixCount(
            int previousSixCount,
            int currentRoll)
        {
            if (currentRoll == 6)
                return previousSixCount + 1;

            return 0;
        }

        // هزینه تلاش مجدد با الماس
        public static int GetRetryDiamondCost()
        {
            return RetryDiamondCost;
        }
    }
}
