
using System;

namespace ManchTak
{
    public enum WalletCurrency
    {
        Coin,
        Diamond
    }

    public enum WalletTransactionType
    {
        NewAccountGift,
        MatchEntryFee,
        MatchWinnerReward,
        DiamondRetry,
        AdminGrant,
        Purchase,
        Refund
    }

    public enum WalletTransactionStatus
    {
        Pending,
        Confirmed,
        Rejected
    }

    [Serializable]
    public class WalletData
    {
        public string PlayerId;

        public long Coins;
        public long Diamonds;

        public long LastUpdatedAt;

        public WalletData(string playerId)
        {
            PlayerId = playerId;
            Coins = 0;
            Diamonds = 0;
            LastUpdatedAt = 0;
        }

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(PlayerId) &&
                   Coins >= 0 &&
                   Diamonds >= 0;
        }
    }

    [Serializable]
    public class WalletTransaction
    {
        public string TransactionId;
        public string PlayerId;

        public WalletCurrency Currency;
        public WalletTransactionType Type;
        public WalletTransactionStatus Status;

        // مقدار تراکنش همیشه مثبت است.
        public long Amount;

        public string MatchId;
        public string Description;

        public long CreatedAt;

        public WalletTransaction()
        {
            TransactionId = Guid.NewGuid().ToString("N");

            Status = WalletTransactionStatus.Pending;

            CreatedAt =
                DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            Amount = 0;
            MatchId = string.Empty;
            Description = string.Empty;
        }

        public bool IsValid()
        {
            return
                !string.IsNullOrWhiteSpace(TransactionId) &&
                !string.IsNullOrWhiteSpace(PlayerId) &&
                Amount > 0 &&
                Enum.IsDefined(typeof(WalletCurrency), Currency) &&
                Enum.IsDefined(typeof(WalletTransactionType), Type);
        }
    }

    public static class WalletModels
    {
        public const long NewAccountCoins = 500;
        public const long NewAccountDiamonds = 500;

        public const long AdminInitialCoins = 200000000;
        public const long AdminInitialDiamonds = 200000000;

        public static WalletData CreateEmptyWallet(string playerId)
        {
            if (string.IsNullOrWhiteSpace(playerId))
                return null;

            return new WalletData(playerId);
        }

        public static bool HasEnoughCoins(
            WalletData wallet,
            long amount)
        {
            return wallet != null &&
                   wallet.IsValid() &&
                   amount > 0 &&
                   wallet.Coins >= amount;
        }

        public static bool HasEnoughDiamonds(
            WalletData wallet,
            long amount)
        {
            return wallet != null &&
                   wallet.IsValid() &&
                   amount > 0 &&
                   wallet.Diamonds >= amount;
        }
    }
}
