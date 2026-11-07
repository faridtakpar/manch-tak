
using System;

namespace ManchTak
{
    public enum MatchArena
    {
        Azadi,
        Tajrish
    }

    public enum MatchWalletAction
    {
        EntryFee,
        WinnerReward
    }

    public static class MatchWalletRules
    {
        public const long EntryFee = 50;

        public const long AzadiWinnerReward = 500;
        public const long TajrishWinnerReward = 200;

        public static long GetEntryFee(MatchArena arena)
        {
            return IsValidArena(arena) ? EntryFee : 0;
        }

        public static long GetWinnerReward(MatchArena arena)
        {
            switch (arena)
            {
                case MatchArena.Azadi:
                    return AzadiWinnerReward;

                case MatchArena.Tajrish:
                    return TajrishWinnerReward;

                default:
                    return 0;
            }
        }

        public static int GetPlayerCapacity(MatchArena arena)
        {
            switch (arena)
            {
                case MatchArena.Azadi:
                    return 4;

                case MatchArena.Tajrish:
                    return 2;

                default:
                    return 0;
            }
        }

        public static bool IsValidArena(MatchArena arena)
        {
            return arena == MatchArena.Azadi ||
                   arena == MatchArena.Tajrish;
        }

        public static bool CanEnterMatch(WalletData wallet)
        {
            return WalletModels.HasEnoughCoins(
                wallet,
                EntryFee
            );
        }

        public static long CalculateCoinsAfterEntry(
            long currentCoins)
        {
            if (currentCoins < EntryFee)
                return -1;

            return currentCoins - EntryFee;
        }

        public static long CalculateWinnerCoins(
            long currentCoins,
            MatchArena arena)
        {
            long reward = GetWinnerReward(arena);

            if (reward <= 0 || currentCoins < 0)
                return -1;

            if (currentCoins > long.MaxValue - reward)
                return -1;

            return currentCoins + reward;
        }

        public static long CalculateLoserCoins(
            long currentCoins)
        {
            return currentCoins >= 0
                ? currentCoins
                : -1;
        }

        // بازپرداخت هزینه ورود هنگام انصراف
        // این نسخه فرض می‌کند ۵۰ سکه قبلاً کسر شده است.
        public static long CalculateResignedCoins(
            long currentCoins)
        {
            return CalculateResignedCoins(
                currentCoins,
                true
            );
        }

        // مشخص می‌کند هزینه ورود قبلاً کسر شده یا خیر.
        public static long CalculateResignedCoins(
            long currentCoins,
            bool entryFeeWasDeducted)
        {
            if (currentCoins < 0)
                return -1;

            if (!entryFeeWasDeducted)
                return currentCoins;

            if (currentCoins > long.MaxValue - EntryFee)
                return -1;

            return currentCoins + EntryFee;
        }

        public static bool IsValidPlayerCount(
            MatchArena arena,
            int playerCount)
        {
            return playerCount == GetPlayerCapacity(arena);
        }
    }
}
