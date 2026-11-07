using System;

namespace ManchTak
{
    public class PlayerProfileController
    {
        public PlayerProfileData Profile { get; private set; }

        public event Action<PlayerProfileData> OnProfileChanged;

        public PlayerProfileController(PlayerProfileData profile)
        {
            Profile = profile ?? new PlayerProfileData();
            NormalizeProfile();
        }

        private void NormalizeProfile()
        {
            Profile.PlayerId ??= string.Empty;

            if (string.IsNullOrWhiteSpace(Profile.DisplayName))
                Profile.DisplayName = "بازیکن جدید";

            Profile.AvatarId = Math.Max(1, Profile.AvatarId);
            Profile.PlayerLevel = Math.Max(1, Profile.PlayerLevel);
            Profile.CurrentXP = Math.Max(0, Profile.CurrentXP);
            Profile.TotalXP = Math.Max(0, Profile.TotalXP);
            Profile.TotalWins = Math.Max(0, Profile.TotalWins);
            Profile.TotalLosses = Math.Max(0, Profile.TotalLosses);
        }

        public bool UpdateDisplayName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                return false;

            newName = newName.Trim();

            if (newName.Length < 3 || newName.Length > 20)
                return false;

            Profile.DisplayName = newName;

            NotifyChanged();
            return true;
        }

        public bool UpdateAvatar(int avatarId)
        {
            if (PlayerAvatarCatalog.Find(avatarId) == null)
                return false;

            Profile.AvatarId = avatarId;

            NotifyChanged();
            return true;
        }

        public void UpdateLevel(int level)
        {
            Profile.PlayerLevel = Math.Max(1, level);
            NotifyChanged();
        }

        public void RecordWin()
        {
            Profile.TotalWins++;
            NotifyChanged();
        }

        public void RecordLoss()
        {
            Profile.TotalLosses++;
            NotifyChanged();
        }

        public int GetTotalMatches()
        {
            return Profile.TotalWins + Profile.TotalLosses;
        }

        public float GetWinRate()
        {
            int total = GetTotalMatches();

            if (total == 0)
                return 0f;

            return (float)Profile.TotalWins / total * 100f;
        }

        private void NotifyChanged()
        {
            OnProfileChanged?.Invoke(Profile);
        }
    }
}
