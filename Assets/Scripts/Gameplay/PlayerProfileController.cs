
using System;

namespace ManchTak
{
    public class PlayerProfileController
    {
        public PlayerProfileData Profile { get; private set; }

        public event Action<PlayerProfileData> OnProfileChanged;
        public event Action<int> OnLevelUp;

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

            Profile.TotalWins = Math.Max(0, Profile.TotalWins);
            Profile.TotalLosses = Math.Max(0, Profile.TotalLosses);

            Profile.TotalXP = Math.Max(0, Profile.TotalXP);
            Profile.CurrentXP = Math.Max(0, Profile.CurrentXP);

            Profile.PlayerLevel = Math.Max(
                LevelProgressionSystem.MinimumLevel,
                Math.Min(
                    LevelProgressionSystem.MaximumLevel,
                    Profile.PlayerLevel
                )
            );

            // حفظ اطلاعات قدیمی در صورت ناقص بودن مجموع XP
            long expectedTotalXP =
                LevelProgressionSystem.GetTotalXPForLevel(
                    Profile.PlayerLevel
                ) + Profile.CurrentXP;

            if (Profile.TotalXP < expectedTotalXP)
            {
                Profile.TotalXP = (int)Math.Min(
                    int.MaxValue,
                    expectedTotalXP
                );
            }

            // محاسبه سطح واقعی از روی مجموع XP
            Profile.PlayerLevel =
                LevelProgressionSystem.CalculateLevel(
                    Profile.TotalXP
                );

            // محاسبه XP داخل سطح فعلی
            long levelStartXP =
                LevelProgressionSystem.GetTotalXPForLevel(
                    Profile.PlayerLevel
                );

            long currentLevelXP =
                Profile.TotalXP - levelStartXP;

            Profile.CurrentXP = (int)Math.Max(
                0L,
                Math.Min(int.MaxValue, currentLevelXP)
            );

            if (Profile.PlayerLevel >=
                LevelProgressionSystem.MaximumLevel)
            {
                Profile.CurrentXP = 0;
            }
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

        // تغییر دستی سطح؛ برای مدیریت و اصلاح اطلاعات
        public void UpdateLevel(int level)
        {
            level = Math.Max(
                LevelProgressionSystem.MinimumLevel,
                Math.Min(
                    LevelProgressionSystem.MaximumLevel,
                    level
                )
            );

            Profile.PlayerLevel = level;

            long minimumTotalXP =
                LevelProgressionSystem.GetTotalXPForLevel(level);

            Profile.TotalXP = (int)Math.Min(
                int.MaxValue,
                Math.Max(Profile.TotalXP, minimumTotalXP)
            );

            NormalizeProfile();
            NotifyChanged();
        }

        // افزودن XP به پروفایل اصلی
        public bool AddXP(int amount)
        {
            if (amount <= 0)
                return false;

            if (Profile.PlayerLevel >=
                LevelProgressionSystem.MaximumLevel)
                return false;

            if (Profile.TotalXP > int.MaxValue - amount)
                return false;

            int previousLevel = Profile.PlayerLevel;

            Profile.TotalXP += amount;

            NormalizeProfile();

            // اعلام تمام سطح‌هایی که بازیکن ارتقا داده است
            for (
                int level = previousLevel + 1;
                level <= Profile.PlayerLevel;
                level++
            )
            {
                OnLevelUp?.Invoke(level);
            }

            NotifyChanged();
            return true;
        }

        // XP برنده مسابقه
        public bool AddWinXP()
        {
            return AddXP(PlayerProgressionRules.WinnerXP);
        }

        // XP بازنده مسابقه
        public bool AddLoseXP()
        {
            return AddXP(PlayerProgressionRules.LoserXP);
        }

        public void RecordWin()
        {
            if (Profile.TotalWins < int.MaxValue)
                Profile.TotalWins++;

            NotifyChanged();
        }

        public void RecordLoss()
        {
            if (Profile.TotalLosses < int.MaxValue)
                Profile.TotalLosses++;

            NotifyChanged();
        }

        public int GetTotalMatches()
        {
            long total =
                (long)Profile.TotalWins + Profile.TotalLosses;

            return (int)Math.Min(int.MaxValue, total);
        }

        public float GetWinRate()
        {
            int total = GetTotalMatches();

            if (total == 0)
                return 0f;

            return (float)Profile.TotalWins / total * 100f;
        }

        public int GetRemainingXP()
        {
            return (int)Math.Min(
                int.MaxValue,
                LevelProgressionSystem.GetRemainingXP(
                    new LevelProgressData(Profile.PlayerId)
                    {
                        Level = Profile.PlayerLevel,
                        TotalXP = Profile.TotalXP
                    }
                )
            );
        }

        public float GetLevelProgress()
        {
            return LevelProgressionSystem
                .GetLevelProgressPercent(
                    new LevelProgressData(Profile.PlayerId)
                    {
                        Level = Profile.PlayerLevel,
                        TotalXP = Profile.TotalXP
                    }
                ) / 100f;
        }

        private void NotifyChanged()
        {
            OnProfileChanged?.Invoke(Profile);
        }
    }
}
