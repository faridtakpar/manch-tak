using System;

namespace ManchTak
{
    public enum PlayerAvatarType
    {
        Male = 0,
        Female = 1
    }

    [Serializable]
    public class PlayerProfileViewData
    {
        public string DisplayName;
        public string AvatarId;
        public PlayerAvatarType AvatarType;

        public int Level;
        public int TotalXP;

        public int TotalWins;
        public int TotalLosses;
        public int TotalMatches;

        public int FriendsCount;

        // اطلاعات نشان لول
        public string LevelBadgeText;
        public string LevelBadgeColorHex;
        public string LevelBadgeCategory;

        public PlayerProfileViewData()
        {
            DisplayName = "بازیکن جدید";
            AvatarId = "default_01";
            AvatarType = PlayerAvatarType.Male;

            Level = 1;
            TotalXP = 0;

            TotalWins = 0;
            TotalLosses = 0;
            TotalMatches = 0;

            FriendsCount = 0;

            RefreshLevelBadge();
        }

        // هماهنگ سازی نشان با سطح بازیکن
        public void RefreshLevelBadge()
        {
            Level = Math.Max(
                1,
                Math.Min(200, Level)
            );

            LevelBadgeText =
                PlayerLevelBadgeRules.GetLevelText(Level);

            LevelBadgeColorHex =
                PlayerLevelBadgeRules.GetColorHex(Level);

            LevelBadgeCategory =
                PlayerLevelBadgeRules.GetLevelCategory(Level);
        }

        // تغییر سطح بازیکن و بروزرسانی نشان
        public bool SetLevel(int newLevel)
        {
            if (!PlayerLevelBadgeRules.IsValidLevel(newLevel))
                return false;

            Level = newLevel;

            RefreshLevelBadge();

            return true;
        }
    }
}
