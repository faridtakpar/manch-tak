
namespace ManchTak
{
    public enum MatchField
    {
        AzadiFourPlayer,
        TajrishTwoPlayer
    }

    public enum MatchResult
    {
        Winner,
        Loser,
        Resigned
    }

    public static class MatchEconomyRules
    {
        // هزینه ورود هر بازیکن
        public const int EntryFee = 50;

        // دریافت هزینه ورود فقط یک بار
        public static int GetEntryFee()
        {
            return EntryFee;
        }

        // جایزه برنده بر اساس میدان
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

        // بازنده و انصراف‌دهنده جریمه اضافی ندارند
        public static int GetAdditionalDeduction(MatchResult result)
        {
            return 0;
        }

        // فقط برنده جایزه مسابقه را دریافت می‌کند
        public static int GetPrize(
            MatchField field,
            MatchResult result)
        {
            if (result != MatchResult.Winner)
                return 0;

            return GetWinnerPrize(field);
        }
    }
}
