
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

        // هزینه ورود هر میدان
        public static long GetEntryFee(MatchArena arena)
        {
            return IsValidArena(arena) ? EntryFee : 0;
        }

        // جایزه برنده هر میدان
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

        // بررسی ظرفیت میدان
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

        // بررسی اعتبار میدان
        public static bool IsValidArena(MatchArena arena)
        {
            return arena == MatchArena.Azadi ||
                   arena == MatchArena.Tajrish;
        }

        // بررسی امکان ورود بازیکن
        public static bool CanEnterMatch(WalletData wallet)
        {
            return WalletModels.HasEnoughCoins(
                wallet,
                EntryFee
            );
        }

        // محاسبه موجودی پس از پرداخت ورودی
        public static long CalculateCoinsAfterEntry(
            long currentCoins)
        {
            if (currentCoins < EntryFee)
                return -1;

            return currentCoins - EntryFee;
        }

        // محاسبه جایزه برنده
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

        // محاسبه وضعیت بازنده
        // هزینه ورود قبلاً پرداخت شده است.
        public static long CalculateLoserCoins(
            long currentCoins)
        {
            return currentCoins >= 0
                ? currentCoins
                : -1;
        }

        // محاسبه وضعیت بازیکن پس از انصراف وسط مسابقه
        public static long CalculateResignedCoins(
            long currentCoins)
        {
            return currentCoins >= 0
                ? currentCoins
                : -1;
        }

        // اعتبارسنجی تعداد بازیکنان میدان
        public static bool IsValidPlayerCount(
            MatchArena arena,
            int playerCount)
        {
            return playerCount == GetPlayerCapacity(arena);
        }
    }
}
