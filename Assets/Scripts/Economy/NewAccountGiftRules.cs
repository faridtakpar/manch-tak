using System;

namespace ManchTak
{
    public static class NewAccountGiftRules
    {
        public const long InitialCoins = 500;
        public const long InitialDiamonds = 500;

        // بررسی اعتبار شناسه بازیکن
        public static bool IsValidPlayer(string playerId)
        {
            return !string.IsNullOrWhiteSpace(playerId);
        }

        // ساخت تراکنش هدیه اولیه سکه
        public static WalletTransaction CreateCoinGift(string playerId)
        {
            if (!IsValidPlayer(playerId))
                return null;

            return WalletTransactionRules.CreateTransaction(
                playerId,
                WalletCurrency.Coin,
                WalletTransactionType.NewAccountGift,
                InitialCoins,
                "",
                "هدیه اولیه ثبت نام"
            );
        }

        // ساخت تراکنش هدیه اولیه الماس
        public static WalletTransaction CreateDiamondGift(string playerId)
        {
            if (!IsValidPlayer(playerId))
                return null;

            return WalletTransactionRules.CreateTransaction(
                playerId,
                WalletCurrency.Diamond,
                WalletTransactionType.NewAccountGift,
                InitialDiamonds,
                "",
                "هدیه اولیه ثبت نام"
            );
        }

        // بررسی اعتبار مبلغ هدیه
        public static bool IsValidGiftAmount(long amount)
        {
            return amount == InitialCoins ||
                   amount == InitialDiamonds;
        }
    }
}
