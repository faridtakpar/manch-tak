
using System;

namespace ManchTak
{
    [Serializable]
    public class AccountRegistrationRequest
    {
        public string PublicId;
        public string RecoveryCode;
    }

    [Serializable]
    public class AccountRegistrationResult
    {
        public bool Success;
        public string PlayerId;
        public string PublicId;
        public string ErrorMessage;

        public AccountRegistrationResult()
        {
            Success = false;
            PlayerId = string.Empty;
            PublicId = string.Empty;
            ErrorMessage = string.Empty;
        }
    }

    public static class AccountRegistrationRules
    {
        // اعتبارسنجی درخواست ثبت نام
        public static bool IsValidRequest(
            AccountRegistrationRequest request)
        {
            return request != null &&
                   PlayerIdGenerator.IsValidId(request.PublicId) &&
                   !PlayerIdGenerator.IsReservedId(request.PublicId) &&
                   !string.IsNullOrWhiteSpace(request.RecoveryCode);
        }

        // ساخت هویت اولیه حساب
        public static PlayerAccountIdentity CreateIdentity(
            string internalPlayerId,
            string publicId)
        {
            if (string.IsNullOrWhiteSpace(internalPlayerId) ||
                !PlayerIdGenerator.IsValidId(publicId) ||
                PlayerIdGenerator.IsReservedId(publicId))
                return null;

            PlayerAccountIdentity identity =
                new PlayerAccountIdentity();

            identity.PlayerId = internalPlayerId;
            identity.PublicId = publicId;

            identity.CreatedAt =
                DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            identity.LastLoginAt = identity.CreatedAt;

            identity.Status = PlayerAccountStatus.Active;

            return identity.IsValid() ? identity : null;
        }

        // آماده سازی کیف پول حساب جدید
        public static WalletData CreateInitialWallet(
            string internalPlayerId)
        {
            if (string.IsNullOrWhiteSpace(internalPlayerId))
                return null;

            WalletData wallet =
                WalletModels.CreateEmptyWallet(internalPlayerId);

            if (wallet == null)
                return null;

            wallet.Coins = NewAccountGiftRules.InitialCoins;
            wallet.Diamonds = NewAccountGiftRules.InitialDiamonds;

            wallet.LastUpdatedAt =
                DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            return wallet;
        }

        // آماده سازی پیشرفت اولیه بازیکن
        public static LevelProgressData CreateInitialProgress(
            string internalPlayerId)
        {
            if (string.IsNullOrWhiteSpace(internalPlayerId))
                return null;

            return new LevelProgressData(internalPlayerId);
        }
    }
}
