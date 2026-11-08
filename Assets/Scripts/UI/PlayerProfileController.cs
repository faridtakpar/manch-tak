
using System;

namespace ManchTak
{
    public class PlayerProfileViewController
    {
        public PlayerProfileViewData Profile { get; private set; }

        public event Action<PlayerProfileViewData> OnProfileChanged;

        public PlayerProfileViewController(
            PlayerProfileViewData profile = null)
        {
            Profile = profile ?? new PlayerProfileViewData();
            Profile.RefreshLevelBadge();
        }

        public bool ChangeDisplayName(string newName)
        {
            if (!PlayerProfileRules.SetDisplayName(Profile, newName))
                return false;

            NotifyProfileChanged();
            return true;
        }

        public bool ChangeAvatar(string avatarId)
        {
            ProfileAvatarData avatar =
                ProfileAvatarCatalog.FindById(avatarId);

            if (avatar == null)
                return false;

            if (!PlayerProfileRules.SetAvatar(
                Profile,
                avatar.AvatarType,
                avatar.AvatarId))
                return false;

            NotifyProfileChanged();
            return true;
        }

        public bool SyncProgress(PlayerProfileData source)
        {
            if (source == null)
                return false;

            if (!PlayerLevelBadgeRules.IsValidLevel(
                source.PlayerLevel))
                return false;

            Profile.DisplayName =
                string.IsNullOrWhiteSpace(source.DisplayName)
                ? "بازیکن جدید"
                : source.DisplayName;

            Profile.Level = source.PlayerLevel;
            Profile.TotalXP = Math.Max(0, source.TotalXP);
            Profile.TotalWins = Math.Max(0, source.TotalWins);
            Profile.TotalLosses = Math.Max(0, source.TotalLosses);

            Profile.TotalMatches =
                Profile.TotalWins + Profile.TotalLosses;

            Profile.RefreshLevelBadge();

            NotifyProfileChanged();
            return true;
        }

        public bool RecordMatchResult(bool won)
        {
            if (!PlayerProfileRules.UpdateMatchStats(Profile, won))
                return false;

            NotifyProfileChanged();
            return true;
        }

        public float GetWinRate()
        {
            return PlayerProfileRules.GetWinRate(Profile);
        }

        private void NotifyProfileChanged()
        {
            Profile.RefreshLevelBadge();
            OnProfileChanged?.Invoke(Profile);
        }
    }
}
