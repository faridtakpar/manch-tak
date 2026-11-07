
using System;

namespace ManchTak
{
    public enum BanDuration
    {
        FiveMinutes = 5,
        TenMinutes = 10,
        TwentyMinutes = 20,
        OneHour = 60,
        TwoHours = 120,
        ThreeHours = 180,
        FourHours = 240
    }

    [Serializable]
    public class PlayerBanRecord
    {
        public string BanId;
        public string PlayerId;
        public string AdminId;
        public string Reason;

        public bool IsPermanent;

        public long StartedAt;
        public long ExpiresAt;

        public PlayerBanRecord()
        {
            BanId = Guid.NewGuid().ToString("N");
            PlayerId = string.Empty;
            AdminId = string.Empty;
            Reason = string.Empty;
            IsPermanent = false;
            StartedAt = 0;
            ExpiresAt = 0;
        }

        public bool IsValid()
        {
            return
                !string.IsNullOrWhiteSpace(BanId) &&
                !string.IsNullOrWhiteSpace(PlayerId) &&
                !string.IsNullOrWhiteSpace(AdminId) &&
                StartedAt > 0 &&
                (IsPermanent || ExpiresAt > StartedAt);
        }
    }

    public static class AccountModerationRules
    {
        // ایجاد محرومیت موقت
        public static PlayerBanRecord CreateTemporaryBan(
            string playerId,
            string adminId,
            string reason,
            BanDuration duration,
            long serverTime)
        {
            if (!IsValidAdmin(adminId) ||
                string.IsNullOrWhiteSpace(playerId) ||
                string.IsNullOrWhiteSpace(reason) ||
                serverTime <= 0 ||
                !Enum.IsDefined(typeof(BanDuration), duration))
                return null;

            long durationSeconds = (long)duration * 60L;

            if (serverTime > long.MaxValue - durationSeconds)
                return null;

            PlayerBanRecord record = new PlayerBanRecord();

            record.PlayerId = playerId;
            record.AdminId = adminId;
            record.Reason = reason;
            record.StartedAt = serverTime;
            record.ExpiresAt = serverTime + durationSeconds;
            record.IsPermanent = false;

            return record.IsValid() ? record : null;
        }

        // ایجاد محرومیت دائمی
        public static PlayerBanRecord CreatePermanentBan(
            string playerId,
            string adminId,
            string reason,
            long serverTime)
        {
            if (!IsValidAdmin(adminId) ||
                string.IsNullOrWhiteSpace(playerId) ||
                string.IsNullOrWhiteSpace(reason) ||
                serverTime <= 0)
                return null;

            PlayerBanRecord record = new PlayerBanRecord();

            record.PlayerId = playerId;
            record.AdminId = adminId;
            record.Reason = reason;
            record.StartedAt = serverTime;
            record.ExpiresAt = 0;
            record.IsPermanent = true;

            return record.IsValid() ? record : null;
        }

        // بررسی اعتبار شناسه مدیر
        private static bool IsValidAdmin(string adminId)
        {
            return string.Equals(
                adminId,
                "faridtak",
                StringComparison.OrdinalIgnoreCase
            );
        }

        // بررسی فعال بودن محرومیت
        public static bool IsBanActive(
            PlayerBanRecord record,
            long serverTime)
        {
            if (record == null ||
                !record.IsValid() ||
                serverTime < record.StartedAt)
                return false;

            if (record.IsPermanent)
                return true;

            return serverTime < record.ExpiresAt;
        }

        // بررسی امکان ورود بازیکن
        public static bool CanPlayerLogin(
            PlayerAccountIdentity identity,
            PlayerBanRecord activeBan,
            long serverTime)
        {
            if (identity == null || !identity.IsValid())
                return false;

            if (identity.Status ==
                PlayerAccountStatus.PermanentlyBanned)
                return false;

            if (activeBan != null &&
                IsBanActive(activeBan, serverTime))
                return false;

            return true;
        }

        // پایان دادن به محرومیت منقضی شده
        public static bool RestoreExpiredBan(
            PlayerAccountIdentity identity,
            PlayerBanRecord record,
            long serverTime)
        {
            if (identity == null ||
                record == null ||
                !identity.IsValid() ||
                !record.IsValid() ||
                record.IsPermanent ||
                serverTime < record.ExpiresAt)
                return false;

            identity.Status = PlayerAccountStatus.Active;

            return true;
        }

        // رفع محرومیت با تأیید مدیر
        public static bool LiftBan(
            PlayerAccountIdentity identity,
            bool serverVerifiedAdmin)
        {
            if (identity == null ||
                !identity.IsValid() ||
                !serverVerifiedAdmin)
                return false;

            if (identity.Status ==
                PlayerAccountStatus.PermanentlyBanned ||
                identity.Status ==
                PlayerAccountStatus.TemporarilyBanned)
            {
                identity.Status = PlayerAccountStatus.Active;
                return true;
            }

            return false;
        }
    }
}
