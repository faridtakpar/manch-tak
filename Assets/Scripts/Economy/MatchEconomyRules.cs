
namespace ManchTak
{
    public enum MatchField
    {
        AzadiFourPlayer,
        TajrishTwoPlayer
    }

    // نام مستقل برای نتیجه اقتصادی مسابقه
    public enum MatchEconomyResult
    {
        Winner,
        Loser,
        Resigned
    }

    public static class MatchEconomyRules
    {
        public const int EntryFee = 50;

        public static int GetEntryFee()
        {
            return EntryFee;
        }

        public static int GetWinnerPrize(MatchField field)
        {
            switch (field)
            {
                case MatchField.AzadiFourPlayer:
                    return 500;

                case MatchField.TajrishTwoPlayer:
                    return 200;

                default:
                    return 0;
            }
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
