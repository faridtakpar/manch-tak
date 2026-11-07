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
        }
    }
}
