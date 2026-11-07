
namespace ManchTak
{
    // صفحات اصلی بازی
    public enum MainMenuPage
    {
        Home,
        Profile,
        Friends,
        Shop,
        Wallet,
        Missions,
        Settings,
        Messages,
        GameRooms
    }

    // حالت‌های بازی
    public enum MainMenuGameMode
    {
        Classic,
        Speed,
        Football
    }

    // نوع تخته بازی
    public enum MainMenuBoardTheme
    {
        Classic,
        Wooden,
        Ice,
        Football,
        Professional
    }

    // تنظیمات منوی اصلی
    public static class MainMenuConfig
    {
        public const string GameTitle = "منچ تک";
        public const int MaximumLevel = 200;

        public static MainMenuPage DefaultPage
        {
            get { return MainMenuPage.Home; }
        }

        public static MainMenuGameMode DefaultGameMode
        {
            get { return MainMenuGameMode.Classic; }
        }

        public static MainMenuBoardTheme DefaultBoard
        {
            get { return MainMenuBoardTheme.Classic; }
        }

        public static bool IsValidPage(MainMenuPage page)
        {
            return System.Enum.IsDefined(
                typeof(MainMenuPage), page);
        }

        public static bool IsValidGameMode(
            MainMenuGameMode mode)
        {
            return System.Enum.IsDefined(
                typeof(MainMenuGameMode), mode);
        }

        public static bool IsValidBoard(
            MainMenuBoardTheme board)
        {
            return System.Enum.IsDefined(
                typeof(MainMenuBoardTheme), board);
        }
    }
}
