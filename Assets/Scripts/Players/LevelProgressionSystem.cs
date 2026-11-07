
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
            Level = LevelProgressionSystem.MinimumLevel;
            TotalXP = 0;
        }
    }

    public static class LevelProgressionSystem
    {
        public const int MinimumLevel = 1;
        public const int MaximumLevel = 200;

        public const int FirstLevelXP = 100;
        public const int XPIncreasePerLevel = 25;

        // XP مورد نیاز برای عبور از سطح فعلی
        public static int GetXPRequiredForNextLevel(int level)
        {
            if (level < MinimumLevel || level >= MaximumLevel)
                return 0;

            return FirstLevelXP +
                   ((level - 1) * XPIncreasePerLevel);
        }

        // مجموع XP لازم برای رسیدن به یک سطح
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

        // محاسبه سطح براساس مجموع XP
        public static int CalculateLevel(long totalXP)
        {
            if (totalXP <= 0)
                return MinimumLevel;

            int level = MinimumLevel;

            while (level < MaximumLevel)
            {
                long requiredTotalXP =
                    GetTotalXPForLevel(level + 1);

                if (totalXP < requiredTotalXP)
                    break;

                level++;
            }

            return level;
        }

        // هماهنگ‌سازی اطلاعات سطح و XP
        public static bool NormalizeProgress(
            LevelProgressData progress)
        {
            if (progress == null)
                return false;

            if (string.IsNullOrWhiteSpace(progress.PlayerId))
                return false;

            if (progress.TotalXP < 0)
                progress.TotalXP = 0;

            progress.Level = CalculateLevel(progress.TotalXP);

            return true;
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

            NormalizeProgress(progress);

            if (progress.Level >= MaximumLevel)
                return false;

            if (progress.TotalXP > long.MaxValue - amount)
                return false;

            progress.TotalXP += amount;

            NormalizeProgress(progress);

            return true;
        }

        // XP فعلی داخل سطح
        public static long GetCurrentLevelXP(
            LevelProgressData progress)
        {
            if (progress == null)
                return 0;

            int actualLevel = CalculateLevel(progress.TotalXP);

            long levelStartXP =
                GetTotalXPForLevel(actualLevel);

            return Math.Max(
                0L,
                progress.TotalXP - levelStartXP
            );
        }

        // درصد پیشرفت نوار XP
        public static float GetLevelProgressPercent(
            LevelProgressData progress)
        {
            if (progress == null)
                return 0f;

            int actualLevel = CalculateLevel(progress.TotalXP);

            if (actualLevel >= MaximumLevel)
                return 100f;

            int requiredXP =
                GetXPRequiredForNextLevel(actualLevel);

            if (requiredXP <= 0)
                return 100f;

            long currentXP = GetCurrentLevelXP(progress);

            float percentage =
                (float)currentXP / requiredXP * 100f;

            return Math.Max(
                0f,
                Math.Min(100f, percentage)
            );
        }

        // XP باقی‌مانده تا سطح بعدی
        public static long GetRemainingXP(
            LevelProgressData progress)
        {
            if (progress == null)
                return 0;

            int actualLevel = CalculateLevel(progress.TotalXP);

            if (actualLevel >= MaximumLevel)
                return 0;

            long currentXP = GetCurrentLevelXP(progress);

            return Math.Max(
                0L,
                GetXPRequiredForNextLevel(actualLevel) - currentXP
            );
        }
    }
}
