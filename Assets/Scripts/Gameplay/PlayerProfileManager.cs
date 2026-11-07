
using System;

namespace ManchTak
{
    public class PlayerProfileManager
    {
        public PlayerProfileController ProfileController
        {
            get;
            private set;
        }

        public PlayerProfileData Profile
        {
            get
            {
                return ProfileController.Profile;
            }
        }

        public PlayerProfileManager(
            string playerId,
            string defaultDisplayName = "بازیکن جدید")
        {
            if (string.IsNullOrWhiteSpace(playerId))
            {
                throw new ArgumentException(
                    "شناسه بازیکن معتبر نیست.",
                    nameof(playerId)
                );
            }

            PlayerProfileData savedProfile =
                PlayerProfileStorage.Load(playerId);

            if (savedProfile == null)
            {
                savedProfile = new PlayerProfileData(
                    playerId,
                    defaultDisplayName,
                    1
                );
            }

            ProfileController =
                new PlayerProfileController(savedProfile);

            ProfileController.OnProfileChanged += SaveProfile;

            SaveProfile(Profile);
        }

        private void SaveProfile(PlayerProfileData profile)
        {
            PlayerProfileStorage.Save(profile);
        }

        public bool ChangeDisplayName(string newName)
        {
            return ProfileController.UpdateDisplayName(newName);
        }

        public bool ChangeAvatar(int avatarId)
        {
            if (!PlayerAvatarCatalog.IsUnlocked(
                avatarId,
                Profile.PlayerLevel))
            {
                return false;
            }

            return ProfileController.UpdateAvatar(avatarId);
        }

        public void UpdateLevel(int level)
        {
            ProfileController.UpdateLevel(level);
        }

        public void RecordWin()
        {
            ProfileController.RecordWin();
        }

        public void RecordLoss()
        {
            ProfileController.RecordLoss();
        }

        public void Save()
        {
            SaveProfile(Profile);
        }

        public void Close()
        {
            SaveProfile(Profile);
            ProfileController.OnProfileChanged -= SaveProfile;
        }
    }
}
