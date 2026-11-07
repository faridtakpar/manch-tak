using System;

namespace ManchTak
{
    public static class PlayerProfileRules
    {
        public static bool SetDisplayName(
            PlayerProfileViewData profile,
            string displayName)
        {
            if (profile == null || string.IsNullOrWhiteSpace(displayName))
                return false;

            displayName = displayName.Trim();

            if (displayName.Length < 3 || displayName.Length > 20)
                return false;

            profile.DisplayName = displayName;
            return true;
        }

        public static bool SetAvatar(
            PlayerProfileViewData profile,
            PlayerAvatarType avatarType,
            string avatarId)
        {
            if (profile == null ||
                string.IsNullOrWhiteSpace(avatarId))
                return false;

            if (!Enum.IsDefined(typeof(PlayerAvatarType), avatarType))
                return false;

            ProfileAvatarData avatar =
                ProfileAvatarCatalog.FindById(avatarId.Trim());

            if (avatar == null || avatar.AvatarType != avatarType)
                return false;

            profile.AvatarType = avatarType;
            profile.AvatarId = avatar.AvatarId;

            return true;
        }

        public static bool UpdateMatchStats(
            PlayerProfileViewData profile,
            bool won)
        {
            if (profile == null)
                return false;

            if (profile.TotalWins < 0 ||
                profile.TotalLosses < 0)
                return false;

            if (profile.TotalWins + profile.TotalLosses !=
                profile.TotalMatches)
                return false;

            profile.TotalMatches++;

            if (won)
                profile.TotalWins++;
            else
                profile.TotalLosses++;

            return true;
        }

        public static bool SetFriendsCount(
            PlayerProfileViewData profile,
            int count)
        {
            if (profile == null || count < 0)
                return false;

            profile.FriendsCount = count;
            return true;
        }

        public static float GetWinRate(
            PlayerProfileViewData profile)
        {
            if (profile == null || profile.TotalMatches <= 0)
                return 0f;

            return (float)profile.TotalWins /
                   profile.TotalMatches * 100f;
        }
    }
}
