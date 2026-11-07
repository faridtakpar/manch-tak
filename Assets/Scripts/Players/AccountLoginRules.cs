
using System;

namespace ManchTak
{
    public enum AccountLoginStatus
    {
        Success,
        InvalidId,
        AccountNotFound,
        AccountBanned,
        AdminAuthenticationRequired
    }

    [Serializable]
    public class AccountLoginResponse
    {
        public AccountLoginStatus Status;
        public string PlayerId;
        public string PublicId;
        public string Message;

        public bool Success
        {
            get
            {
                return Status == AccountLoginStatus.Success;
            }
        }
    }

    public static class AccountLoginRules
    {
        private const string AdminId = "faridtak";

        // بررسی ساختار شناسه
        public static bool IsValidLoginId(string publicId)
        {
            if (string.IsNullOrWhiteSpace(publicId))
                return false;

            if (string.Equals(
                publicId,
                AdminId,
                StringComparison.OrdinalIgnoreCase))
                return true;

            return PlayerIdGenerator.IsValidId(publicId);
        }

        // بررسی وضعیت حساب
        public static bool CanLogin(
            PlayerAccountIdentity identity)
        {
            return identity != null &&
                   identity.IsValid() &&
                   identity.Status == PlayerAccountStatus.Active;
        }

        // ساخت پاسخ ورود موفق
        public static AccountLoginResponse CreateSuccess(
            PlayerAccountIdentity identity)
        {
            if (identity == null || !identity.IsValid())
                return CreateFailure(
                    AccountLoginStatus.AccountNotFound,
                    "حساب کاربری پیدا نشد."
                );

            if (identity.Status != PlayerAccountStatus.Active)
                return CreateFailure(
                    AccountLoginStatus.AccountBanned,
                    "این حساب کاربری مسدود شده است."
                );

            return new AccountLoginResponse
            {
                Status = AccountLoginStatus.Success,
                PlayerId = identity.PlayerId,
                PublicId = identity.PublicId,
                Message = "ورود موفقیت‌آمیز بود."
            };
        }

        // ساخت پاسخ ورود ناموفق
        public static AccountLoginResponse CreateFailure(
            AccountLoginStatus status,
            string message)
        {
            return new AccountLoginResponse
            {
                Status = status,
                PlayerId = string.Empty,
                PublicId = string.Empty,
                Message = message ?? string.Empty
            };
        }

        // بررسی شناسه مخصوص مدیر
        public static bool IsAdminId(string publicId)
        {
            return string.Equals(
                publicId,
                AdminId,
                StringComparison.OrdinalIgnoreCase
            );
        }

        // بررسی ورود مدیر
        // تأیید واقعی مدیر باید از سرور دریافت شود.
        public static bool CanLoginAsAdmin(
            string publicId,
            bool serverVerifiedAdmin)
        {
            return IsAdminId(publicId) &&
                   serverVerifiedAdmin;
        }

        // بررسی شناسه برای ورود
        public static AccountLoginStatus ValidateLoginId(
            string publicId)
        {
            if (!IsValidLoginId(publicId))
                return AccountLoginStatus.InvalidId;

            if (IsAdminId(publicId))
                return AccountLoginStatus.AdminAuthenticationRequired;

            return AccountLoginStatus.Success;
        }
    }
}
