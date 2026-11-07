
namespace ManchTak
{
    public enum MatchField
    {
        AzadiFourPlayer,
        TajrishTwoPlayer
    }

    public enum MatchEconomyResult
    {
        Winner,
        Loser,
        Resigned
    }

    public static class MatchEconomyRules
    {
        public static int GetEntryFee()
        {
            return (int)MatchWalletRules.EntryFee;
        }

        private static MatchArena ConvertArena(
            MatchField field)
        {
            switch (field)
            {
                case MatchField.AzadiFourPlayer:
                    return MatchArena.Azadi;

                case MatchField.TajrishTwoPlayer:
                    return MatchArena.Tajrish;

                default:
                    return (MatchArena)(-1);
            }
        }

        public static int GetWinnerPrize(
            MatchField field)
        {
            return (int)MatchWalletRules.GetWinnerReward(
                ConvertArena(field)
            );
        }

        public static int GetAdditionalDeduction(
            MatchEconomyResult result)
        {
            return 0;
        }

        public static int GetPrize(
            MatchField field,
            MatchEconomyResult result)
        {
            if (result != MatchEconomyResult.Winner)
                return 0;

            return GetWinnerPrize(field);
        }
    }
}
