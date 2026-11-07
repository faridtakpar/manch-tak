
namespace ManchTak
{
    public static class GameConfig
    {
        // اطلاعات اصلی بازی
        public const string GameName = "منچ تک";
        public const string GameVersion = "1.0.0";

        // تنظیمات مسابقه
        public const int EntryFee = 50;

        // میدان آزادی: چهار نفره
        public const int AzadiPlayerCount = 4;
        public const int AzadiWinnerPrize = 500;

        // میدان تجریش: دونفره
        public const int TajrishPlayerCount = 2;
        public const int TajrishWinnerPrize = 200;

        // پاداش شروع حساب جدید
        public const int NewAccountCoins = 500;
        public const int NewAccountDiamonds = 500;

        // سطح و امتیاز تجربه
        public const int MaxLevel = 200;
        public const int WinnerXP = 138;
        public const int LoserXP = 50;

        // قانون سه تاس شش
        public const int ConsecutiveSixLimit = 3;
        public const int SixRetryDiamondCost = 2;

        // کیف پول اولیه مدیر
        public const long ManagerInitialCoins = 200000000;
        public const long ManagerInitialDiamonds = 200000000;
    }
}
