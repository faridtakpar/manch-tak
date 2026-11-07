
using System;

namespace ManchTak
{
    public class PlayerProgressController
    {
        public PlayerProgressData Progress { get; private set; }

        public event Action<int> OnLevelUp;
        public event Action<PlayerProgressData> OnProgressChanged;

        public PlayerProgressController(PlayerProgressData data)
        {
            Progress = data ?? new PlayerProgressData();
            NormalizeProgress();
        }

        // هماهنگ‌سازی اطلاعات XP و سطح
        private void NormalizeProgress()
        {
            Progress.PlayerLevel = Math.Max(
                LevelProgressionSystem.MinimumLevel,
                Progress.PlayerLevel
            );

            Progress.PlayerLevel = Math.Min(
                LevelProgressionSystem.MaximumLevel,
                Progress.PlayerLevel
            );

            Progress.CurrentXP = Math.Max(0, Progress.CurrentXP);
            Progress.TotalXP = Math.Max(0, Progress.TotalXP);

            // بازیابی XP کل در صورت ناسازگاری اطلاعات قدیمی
            long expectedTotalXP =
                LevelProgressionSystem.GetTotalXPForLevel(
                    Progress.PlayerLevel
                ) + Progress.CurrentXP;

            if (Progress.TotalXP < expectedTotalXP)
            {
                Progress.TotalXP = expectedTotalXP > int.MaxValue
                    ? int.MaxValue
                    : (int)expectedTotalXP;
            }

            int calculatedLevel =
                LevelProgressionSystem.CalculateLevel(
                    Progress.TotalXP
                );

            Progress.PlayerLevel = calculatedLevel;

            long levelStartXP =
                LevelProgressionSystem.GetTotalXPForLevel(
                    calculatedLevel
                );

            long currentLevelXP =
                Progress.TotalXP - levelStartXP;

            Progress.CurrentXP = (int)Math.Max(
                0L,
                Math.Min(int.MaxValue, currentLevelXP)
            );

            Progress.XPToNextLevel =
                LevelProgressionSystem.GetXPRequiredForNextLevel(
                    Progress.PlayerLevel
                );
        }

        // دریافت XP پس از برد
        public void AddWinXP()
        {
            AddXP(PlayerProgressionRules.WinnerXP);
        }

        // دریافت XP پس از باخت
        public void AddLoseXP()
        {
            AddXP(PlayerProgressionRules.LoserXP);
        }

        // اضافه کردن XP
        public void AddXP(int amount)
        {
            if (amount <= 0)
                return;

            NormalizeProgress();

            if (Progress.PlayerLevel >=
                LevelProgressionSystem.MaximumLevel)
                return;

            if (Progress.TotalXP > int.MaxValue - amount)
                return;

            int previousLevel = Progress.PlayerLevel;

            Progress.TotalXP += amount;

            NormalizeProgress();

            // اعلام ارتقای سطح، حتی اگر چند سطح یک‌باره رد شده باشد
            for (
                int level = previousLevel + 1;
                level <= Progress.PlayerLevel;
                level++
            )
            {
                OnLevelUp?.Invoke(level);
            }

            OnProgressChanged?.Invoke(Progress);
        }

        // XP باقی‌مانده تا سطح بعدی
        public int GetRemainingXP()
        {
            return (int)Math.Min(
                int.MaxValue,
                LevelProgressionSystem.GetRemainingXP(
                    new LevelProgressData(Progress.PlayerId)
                    {
                        Level = Progress.PlayerLevel,
                        TotalXP = Progress.TotalXP
                    }
                )
            );
        }

        // درصد پیشرفت نوار XP
        public float GetLevelProgress()
        {
            if (Progress.PlayerLevel >=
                LevelProgressionSystem.MaximumLevel)
                return 1f;

            return LevelProgressionSystem
                .GetLevelProgressPercent(
                    new LevelProgressData(Progress.PlayerId)
                    {
                        Level = Progress.PlayerLevel,
                        TotalXP = Progress.TotalXP
                    }
                ) / 100f;
        }
    }
}
