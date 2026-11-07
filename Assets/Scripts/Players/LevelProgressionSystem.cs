
using System;

namespace ManchTak
{
    [Serializable]
    public class LevelProgressData
    {
        public string PlayerId;
        public int Level;
        public long TotalXP;

        public LevelProgressData(string playerId)
        {
            PlayerId = playerId;
            Level = 1;
            TotalXP = 0;
        }
    }

    public static class LevelProgressionSystem
    {
        public const int MinimumLevel = 1;
        public const int MaximumLevel = 200;

        public const int FirstLevelXP = 100;
        public const int XPIncreasePerLevel = 25;

        // XP مورد نیاز برای عبور از یک سطح
        public static int GetXPRequiredForNextLevel(int level)
        {
            if (level < MinimumLevel ||
                level >= MaximumLevel)
                return 0;

            return FirstLevelXP +
                   ((level - 1) * XPIncreasePerLevel);
        }

        // مجموع XP مورد نیاز برای رسیدن به یک سطح
        public static long GetTotalXPForLevel(int level)
        {
            if (level <= MinimumLevel)
                return 0;

            if (level > MaximumLevel)
                level = MaximumLevel;

            long completedLevels = level - 1;

            return completedLevels * FirstLevelXP +
                   ((long)XPIncreasePerLevel *
                   completedLevels *
                   (completedLevels - 1) / 2);
        }

        // تعیین سطح براساس مجموع XP
        public static int CalculateLevel(long totalXP)
        {
            if (totalXP <= 0)
                return MinimumLevel;

            int level = MinimumLevel;

            while (level < MaximumLevel)
            {
                long nextLevelXP =
                    GetTotalXPForLevel(level + 1);

                if (totalXP < nextLevelXP)
                    break;

                level++;
            }

            return level;
        }

        // اضافه کردن XP مسابقه
        public static bool AddXP(
            LevelProgressData progress,
            int amount)
        {
            if (progress == null ||
                amount <= 0 ||
                string.IsNullOrWhiteSpace(progress.PlayerId))
                return false;

            if (progress.Level >= MaximumLevel)
                return false;

            if (progress.TotalXP > long.MaxValue - amount)
                return false;

            progress.TotalXP += amount;

            progress.Level =
                CalculateLevel(progress.TotalXP);

            return true;
        }

        // XP فعلی داخل سطح
        public static long GetCurrentLevelXP(
            LevelProgressData progress)
        {
            if (progress == null)
                return 0;

            long levelStartXP =
                GetTotalXPForLevel(progress.Level);

            return Math.Max(
                0,
                progress.TotalXP - levelStartXP
            );
        }

        // درصد پیشرفت نوار XP
        public static float GetLevelProgressPercent(
            LevelProgressData progress)
        {
            if (progress == null ||
                progress.Level >= MaximumLevel)
                return 100f;

            int requiredXP =
                GetXPRequiredForNextLevel(progress.Level);

            if (requiredXP <= 0)
                return 100f;

            long currentXP =
                GetCurrentLevelXP(progress);

            float percentage =
                (float)currentXP / requiredXP * 100f;

            return Math.Max(
                0f,
                Math.Min(100f, percentage)
            );
        }

        // XP باقی مانده تا سطح بعدی
        public static long GetRemainingXP(
            LevelProgressData progress)
        {
            if (progress == null ||
                progress.Level >= MaximumLevel)
                return 0;

            long currentXP =
                GetCurrentLevelXP(progress);

            return Math.Max(
                0,
                GetXPRequiredForNextLevel(progress.Level) -
                currentXP
            );
        }
    }
}
