using UnityEngine;

namespace ManchTak
{
    public static class PlayerLevelBadgeRules
    {
        public const int MinLevel = 1;
        public const int MaxLevel = 200;

        public const int GoldMaxLevel = 30;
        public const int GreenMinLevel = 31;
        public const int GreenMaxLevel = 49;
        public const int RedMinLevel = 50;

        private const string GoldColor = "#FFC928";
        private const string GreenColor = "#28C76F";
        private const string RedColor = "#F04452";

        public static bool IsValidLevel(int level)
        {
            return level >= MinLevel &&
                   level <= MaxLevel;
        }

        public static string GetColorHex(int level)
        {
            if (level <= GoldMaxLevel)
                return GoldColor;

            if (level <= GreenMaxLevel)
                return GreenColor;

            if (level <= MaxLevel)
                return RedColor;

            return GoldColor;
        }

        public static Color GetColor(int level)
        {
            Color color;

            if (ColorUtility.TryParseHtmlString(
                GetColorHex(level), out color))
            {
                return color;
            }

            return Color.white;
        }

        public static string GetLevelText(int level)
        {
            if (!IsValidLevel(level))
                return "LV 1";

            return "LV " + level;
        }

        public static string GetLevelCategory(int level)
        {
            if (level >= 1 && level <= 30)
                return "Gold";

            if (level >= 31 && level <= 49)
                return "Green";

            if (level >= 50 && level <= 200)
                return "Red";

            return "Invalid";
        }
    }
}
