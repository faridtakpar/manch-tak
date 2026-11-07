
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

        public event Action<int> OnLevelUp;

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
            ProfileController.OnLevelUp += HandleLevelUp;

            SaveProfile(Profile);
        }

        private void SaveProfile(PlayerProfileData profile)
        {
            if (profile == null)
                return;

            PlayerProfileStorage.Save(profile);
        }

        private void HandleLevelUp(int newLevel)
        {
            OnLevelUp?.Invoke(newLevel);
        }

        // تغییر نام بازیکن
        public bool ChangeDisplayName(string newName)
        {
            return ProfileController.UpdateDisplayName(newName);
        }

        // تغییر تصویر پروفایل
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

        // تغییر دستی سطح
        public void UpdateLevel(int level)
        {
            ProfileController.UpdateLevel(level);
        }

        // افزودن XP
        public bool AddXP(int amount)
        {
            return ProfileController.AddXP(amount);
        }

        // XP برنده
        public bool AddWinXP()
        {
            return ProfileController.AddWinXP();
        }

        // XP بازنده
        public bool AddLoseXP()
        {
            return ProfileController.AddLoseXP();
        }

        // ثبت آمار برد
        public void RecordWin()
        {
            ProfileController.RecordWin();
        }

        // ثبت آمار باخت
        public void RecordLoss()
        {
            ProfileController.RecordLoss();
        }

        // XP باقی‌مانده تا سطح بعد
        public int GetRemainingXP()
        {
            return ProfileController.GetRemainingXP();
        }

        // درصد پیشرفت سطح
        public float GetLevelProgress()
        {
            return ProfileController.GetLevelProgress();
        }

        // ذخیره دستی
        public void Save()
        {
            SaveProfile(Profile);
        }

        // بستن مدیر و قطع رویدادها
        public void Close()
        {
            SaveProfile(Profile);

            ProfileController.OnProfileChanged -= SaveProfile;
            ProfileController.OnLevelUp -= HandleLevelUp;
        }
    }
}
