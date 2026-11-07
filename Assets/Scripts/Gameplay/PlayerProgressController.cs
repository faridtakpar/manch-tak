
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

        private void NormalizeProgress()
        {
            Progress.PlayerLevel = Math.Max(1, Progress.PlayerLevel);
            Progress.CurrentXP = Math.Max(0, Progress.CurrentXP);
            Progress.TotalXP = Math.Max(0, Progress.TotalXP);
            Progress.XPToNextLevel = Math.Max(1, Progress.XPToNextLevel);

            while (Progress.CurrentXP >= Progress.XPToNextLevel)
            {
                Progress.CurrentXP -= Progress.XPToNextLevel;
                Progress.PlayerLevel++;

                Progress.XPToNextLevel =
                    GetXPRequiredForLevel(Progress.PlayerLevel);
            }
        }

        public void AddWinXP()
        {
            AddXP(138);
        }

        public void AddLoseXP()
        {
            AddXP(50);
        }

        public void AddXP(int amount)
        {
            if (amount <= 0)
                return;

            Progress.TotalXP += amount;
            Progress.CurrentXP += amount;

            while (Progress.CurrentXP >= Progress.XPToNextLevel)
            {
                Progress.CurrentXP -= Progress.XPToNextLevel;
                Progress.PlayerLevel++;

                Progress.XPToNextLevel =
                    GetXPRequiredForLevel(Progress.PlayerLevel);

                OnLevelUp?.Invoke(Progress.PlayerLevel);
            }

            OnProgressChanged?.Invoke(Progress);
        }

        private int GetXPRequiredForLevel(int level)
        {
            return 500 + ((level - 1) * 100);
        }

        public int GetRemainingXP()
        {
            return Math.Max(
                0,
                Progress.XPToNextLevel - Progress.CurrentXP
            );
        }

        public float GetLevelProgress()
        {
            if (Progress.XPToNextLevel <= 0)
                return 1f;

            return (float)Progress.CurrentXP /
                   Progress.XPToNextLevel;
        }
    }
}
