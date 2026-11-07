
using System;

namespace ManchTak
{
    public static class PlayerAccountStatusRules
    {
        // بررسی معتبر بودن وضعیت حساب
        public static bool IsValidStatus(PlayerAccountStatus status)
        {
            return Enum.IsDefined(
                typeof(PlayerAccountStatus),
                status
            );
        }

        // بررسی امکان ورود بازیکن
        public static bool CanAccessAccount(
            PlayerAccountIdentity identity,
            PlayerBanRecord activeBan,
            long serverTime)
        {
            if (identity == null ||
                !identity.IsValid() ||
                serverTime <= 0)
                return false;

            if (identity.Status == PlayerAccountStatus.PermanentlyBanned)
                return false;

            if (identity.Status == PlayerAccountStatus.TemporarilyBanned)
            {
                if (activeBan == null ||
                    !activeBan.IsValid())
                    return false;

                if (activeBan.IsPermanent)
                    return false;

                if (activeBan.ExpiresAt <= serverTime)
                    return false;

                return false;
            }

            return identity.Status == PlayerAccountStatus.Active;
        }

        // بررسی امکان تغییر وضعیت حساب
        public static bool CanChangeStatus(
            PlayerAccountIdentity identity,
            PlayerAccountStatus newStatus,
            bool serverVerifiedAdmin)
        {
            return
                serverVerifiedAdmin &&
                identity != null &&
                identity.IsValid() &&
                IsValidStatus(newStatus);
        }

        // تغییر وضعیت حساب توسط مدیر تأییدشده
        public static bool ChangeStatus(
            PlayerAccountIdentity identity,
            PlayerAccountStatus newStatus,
            bool serverVerifiedAdmin)
        {
            if (!CanChangeStatus(
                identity,
                newStatus,
                serverVerifiedAdmin))
                return false;

            identity.Status = newStatus;
            return true;
        }

        // بررسی فعال بودن حساب
        public static bool IsActive(PlayerAccountIdentity identity)
        {
            return identity != null &&
                   identity.IsValid() &&
                   identity.Status == PlayerAccountStatus.Active;
        }

        // بررسی محرومیت دائمی
        public static bool IsPermanentlyBanned(
            PlayerAccountIdentity identity)
        {
            return identity != null &&
                   identity.IsValid() &&
                   identity.Status == PlayerAccountStatus.PermanentlyBanned;
        }

        // بررسی محرومیت موقت
        public static bool IsTemporarilyBanned(
            PlayerAccountIdentity identity)
        {
            return identity != null &&
                   identity.IsValid() &&
                   identity.Status == PlayerAccountStatus.TemporarilyBanned;
        }
    }
}
