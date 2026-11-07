
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

            if (savedProfile.ProcessedMatchIds == null)
            {
                savedProfile.ProcessedMatchIds =
                    new string[0];
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

        // بررسی ثبت قبلی مسابقه
        private bool HasProcessedMatch(string matchId)
        {
            if (string.IsNullOrWhiteSpace(matchId) ||
                Profile.ProcessedMatchIds == null)
            {
                return false;
            }

            foreach (string savedMatchId in Profile.ProcessedMatchIds)
            {
                if (string.Equals(
                    savedMatchId,
                    matchId,
                    StringComparison.Ordinal))
                {
                    return true;
                }
            }

            return false;
        }

        // ثبت شناسه مسابقه
        private void MarkMatchAsProcessed(string matchId)
        {
            string[] oldIds = Profile.ProcessedMatchIds
                ?? new string[0];

            string[] newIds = new string[oldIds.Length + 1];

            Array.Copy(
                oldIds,
                newIds,
                oldIds.Length
            );

            newIds[newIds.Length - 1] = matchId;

            Profile.ProcessedMatchIds = newIds;
        }

        // ثبت نتیجه مسابقه و پاداش بازیکن
        public bool ApplyMatchResult(MatchResult result)
        {
            if (result == null ||
                result.Status != MatchResultStatus.Completed ||
                string.IsNullOrWhiteSpace(result.MatchId))
            {
                return false;
            }

            if (HasProcessedMatch(result.MatchId))
                return false;

            string playerId = Profile.PlayerId;

            if (string.IsNullOrWhiteSpace(playerId))
                return false;

            bool isWinner = string.Equals(
                result.WinnerId,
                playerId,
                StringComparison.Ordinal
            );

            bool isLoser = false;

            if (result.LoserIds != null)
            {
                foreach (string loserId in result.LoserIds)
                {
                    if (string.Equals(
                        loserId,
                        playerId,
                        StringComparison.Ordinal))
                    {
                        isLoser = true;
                        break;
                    }
                }
            }

            if (!isWinner && !isLoser)
                return false;

            int rewardXP = isWinner
                ? PlayerProgressionRules.WinnerXP
                : PlayerProgressionRules.LoserXP;

            // ابتدا شناسه را ثبت می‌کنیم تا ذخیره‌های بعدی
            // نتوانند پاداش همین مسابقه را دوباره اعمال کنند.
            MarkMatchAsProcessed(result.MatchId);

            ProfileController.AddXP(rewardXP);

            if (isWinner)
            {
                ProfileController.RecordWin();
            }
            else
            {
                ProfileController.RecordLoss();
            }

            SaveProfile(Profile);

            return true;
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
