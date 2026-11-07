using System.Collections.Generic;

namespace ManchTak
{
    public static class BoardThemeCatalog
    {
        private static readonly List<BoardThemeData> BoardList =
            new List<BoardThemeData>
            {
                new BoardThemeData(
                    MainMenuBoardTheme.Classic,
                    "کلاسیک",
                    "تخته اصلی منچ تک",
                    "Boards/Classic/board",
                    "Boards/Classic/preview",
                    true,
                    1
                ),

                new BoardThemeData(
                    MainMenuBoardTheme.Wooden,
                    "چوبی",
                    "تخته با طراحی چوب طبیعی",
                    "Boards/Wooden/board",
                    "Boards/Wooden/preview",
                    false,
                    5
                ),

                new BoardThemeData(
                    MainMenuBoardTheme.Ice,
                    "یخی",
                    "تخته با طراحی کریستالی و یخی",
                    "Boards/Ice/board",
                    "Boards/Ice/preview",
                    false,
                    10
                ),

                new BoardThemeData(
                    MainMenuBoardTheme.Football,
                    "فوتبالی",
                    "تخته ویژه علاقه‌مندان فوتبال",
                    "Boards/Football/board",
                    "Boards/Football/preview",
                    false,
                    15
                ),

                new BoardThemeData(
                    MainMenuBoardTheme.Professional,
                    "حرفه‌ای",
                    "تخته ویژه بازیکنان حرفه‌ای",
                    "Boards/Professional/board",
                    "Boards/Professional/preview",
                    false,
                    20
                )
            };

        public static IReadOnlyList<BoardThemeData> GetAll()
        {
            return BoardList.AsReadOnly();
        }

        public static BoardThemeData Find(
            MainMenuBoardTheme theme)
        {
            return BoardList.Find(
                item => item.ThemeId == theme
            );
        }

        public static bool IsUnlocked(
            MainMenuBoardTheme theme,
            int playerLevel)
        {
            BoardThemeData data = Find(theme);

            if (data == null || playerLevel < 1)
                return false;

            return data.IsUnlocked ||
                   playerLevel >= data.UnlockLevel;
        }

        public static List<BoardThemeData> GetUnlocked(
            int playerLevel)
        {
            return BoardList.FindAll(
                item => IsUnlocked(
                    item.ThemeId,
                    playerLevel
                )
            );
        }
    }
}
