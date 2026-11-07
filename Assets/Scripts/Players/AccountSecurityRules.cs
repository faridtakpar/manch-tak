
using System;

namespace ManchTak
{
    public static class AccountSecurityRules
    {
        // اعتبارسنجی شناسه بازیکن
        public static bool IsValidPublicId(string publicId)
        {
            return PlayerIdGenerator.IsValidId(publicId);
        }

        // بررسی شناسه مدیر
        public static bool IsAdminId(string publicId)
        {
            return string.Equals(
                publicId,
                PlayerIdGenerator.AdminId,
                StringComparison.OrdinalIgnoreCase
            );
        }

        // بررسی مجوز ورود
        public static bool CanLogin(
            PlayerAccountIdentity identity,
            PlayerBanRecord activeBan,
            bool serverVerifiedAdmin,
            long serverTime)
        {
            if (identity == null ||
                !identity.IsValid() ||
                serverTime <= 0)
                return false;

            if (IsAdminId(identity.PublicId) &&
                !serverVerifiedAdmin)
                return false;

            if (identity.Status != PlayerAccountStatus.Active)
                return false;

            if (activeBan != null &&
                activeBan.IsValid() &&
                (activeBan.IsPermanent ||
                 activeBan.ExpiresAt > serverTime))
                return false;

            return true;
        }

        // بررسی اعتبار نشست
        public static bool IsValidSession(
            PlayerAccountSession session,
            string playerId,
            long serverTime)
        {
            return session != null &&
                   session.IsValid() &&
                   !string.IsNullOrWhiteSpace(playerId) &&
                   string.Equals(
                       session.PlayerId,
                       playerId,
                       StringComparison.Ordinal) &&
                   serverTime >= session.CreatedAt &&
                   serverTime < session.ExpiresAt;
        }

        // بررسی نشست مدیر
        public static bool CanUseAdminSession(
            PlayerAccountSession session,
            string adminId,
            bool serverVerifiedAdmin,
            long serverTime)
        {
            return serverVerifiedAdmin &&
                   string.Equals(
                       adminId,
                       PlayerIdGenerator.AdminId,
                       StringComparison.OrdinalIgnoreCase
                   ) &&
                   IsValidSession(
                       session,
                       session != null ? session.PlayerId : null,
                       serverTime
                   );
        }
    }
}
