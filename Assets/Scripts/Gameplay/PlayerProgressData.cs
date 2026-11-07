
using System;

namespace ManchTak
{
    [Serializable]
    public class PlayerProgressData
    {
        public string PlayerId;

        public int PlayerLevel;
        public int CurrentXP;
        public int TotalXP;
        public int XPToNextLevel;

        public PlayerProgressData()
        {
            PlayerId = string.Empty;

            PlayerLevel = LevelProgressionSystem.MinimumLevel;
            CurrentXP = 0;
            TotalXP = 0;

            XPToNextLevel =
                LevelProgressionSystem.GetXPRequiredForNextLevel(
                    PlayerLevel
                );
        }

        public PlayerProgressData(
            int playerLevel,
            int currentXP,
            int totalXP,
            int xpToNextLevel)
        {
            PlayerId = string.Empty;

            PlayerLevel = Math.Max(
                LevelProgressionSystem.MinimumLevel,
                Math.Min(
                    LevelProgressionSystem.MaximumLevel,
                    playerLevel
                )
            );

            CurrentXP = Math.Max(0, currentXP);
            TotalXP = Math.Max(0, totalXP);

            XPToNextLevel =
                LevelProgressionSystem.GetXPRequiredForNextLevel(
                    PlayerLevel
                );
        }

        public PlayerProgressData(
            string playerId,
            int playerLevel,
            int currentXP,
            int totalXP)
        {
            PlayerId = playerId ?? string.Empty;

            PlayerLevel = Math.Max(
                LevelProgressionSystem.MinimumLevel,
                Math.Min(
                    LevelProgressionSystem.MaximumLevel,
                    playerLevel
                )
            );

            CurrentXP = Math.Max(0, currentXP);
            TotalXP = Math.Max(0, totalXP);

            XPToNextLevel =
                LevelProgressionSystem.GetXPRequiredForNextLevel(
                    PlayerLevel
                );
        }
    }
}
