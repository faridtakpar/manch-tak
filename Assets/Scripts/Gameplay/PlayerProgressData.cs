
using System;

namespace ManchTak
{
    [Serializable]
    public class PlayerProgressData
    {
        public int PlayerLevel;
        public int CurrentXP;
        public int TotalXP;
        public int XPToNextLevel;

        public PlayerProgressData()
        {
            PlayerLevel = 1;
            CurrentXP = 0;
            TotalXP = 0;
            XPToNextLevel = 500;
        }

        public PlayerProgressData(
            int playerLevel,
            int currentXP,
            int totalXP,
            int xpToNextLevel)
        {
            PlayerLevel = Math.Max(1, playerLevel);
            CurrentXP = Math.Max(0, currentXP);
            TotalXP = Math.Max(0, totalXP);
            XPToNextLevel = Math.Max(1, xpToNextLevel);
        }
    }
}
