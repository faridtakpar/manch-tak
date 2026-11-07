
using System;

namespace ManchTak
{
    [Serializable]
    public class PlayerProfileData
    {
        public string PlayerId;
        public string DisplayName;

        public int AvatarId;

        public int PlayerLevel;
        public int CurrentXP;
        public int TotalXP;

        public int TotalWins;
        public int TotalLosses;

        // شناسه مسابقاتی که پاداش آن‌ها ثبت شده است
        public string[] ProcessedMatchIds;

        public string CreatedAt;

        public PlayerProfileData()
        {
            PlayerId = string.Empty;
            DisplayName = "بازیکن جدید";

            AvatarId = 1;

            PlayerLevel = 1;
            CurrentXP = 0;
            TotalXP = 0;

            TotalWins = 0;
            TotalLosses = 0;

            ProcessedMatchIds = new string[0];

            CreatedAt = string.Empty;
        }

        public PlayerProfileData(
            string playerId,
            string displayName,
            int avatarId)
        {
            PlayerId = playerId ?? string.Empty;

            DisplayName = string.IsNullOrWhiteSpace(displayName)
                ? "بازیکن جدید"
                : displayName;

            AvatarId = avatarId > 0 ? avatarId : 1;

            PlayerLevel = 1;
            CurrentXP = 0;
            TotalXP = 0;

            TotalWins = 0;
            TotalLosses = 0;

            ProcessedMatchIds = new string[0];

            CreatedAt = DateTime.UtcNow.ToString("O");
        }
    }
}
