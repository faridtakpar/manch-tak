using System;

namespace ManchTak
{
    public static class AdminWalletRules
    {
        public const long InitialAdminCoins = 200000000;
        public const long InitialAdminDiamonds = 200000000;

        // بررسی اعتبار حساب مدیر
        public static bool IsValidAdmin(
            string playerId,
            bool serverVerifiedAdmin)
        {
            return serverVerifiedAdmin &&
                   !string.IsNullOrWhiteSpace(playerId);
        }

        // بررسی موجودی اولیه مدیر
        public static bool HasCorrectInitialBalance(
            WalletData wallet)
        {
            return wallet != null &&
                   wallet.IsValid() &&
                   wallet.Coins == InitialAdminCoins &&
                   wallet.Diamonds == InitialAdminDiamonds;
        }

        // ایجاد کیف پول اولیه مدیر
        // فقط در محیط سرور مجاز است.
        public static WalletData CreateInitialAdminWallet(
            string playerId,
            bool serverVerifiedAdmin)
        {
            if (!IsValidAdmin(
                playerId,
                serverVerifiedAdmin))
                return null;

            WalletData wallet =
                new WalletData(playerId);

            wallet.Coins = InitialAdminCoins;
            wallet.Diamonds = InitialAdminDiamonds;

            wallet.LastUpdatedAt =
                DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            return wallet;
        }

        // جلوگیری از تخصیص مجدد موجودی اولیه
        public static bool CanInitializeAdminWallet(
            WalletData existingWallet,
            bool alreadyInitialized)
        {
            return existingWallet == null &&
                   !alreadyInitialized;
        }
    }
}
