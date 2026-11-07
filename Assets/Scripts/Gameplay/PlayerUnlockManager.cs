
using System.Collections.Generic;

namespace ManchTak
{
    public static class PlayerUnlockManager
    {
        public static List<BoardThemeData> GetAvailableBoards(int level)
        {
            return BoardThemeCatalog.GetUnlocked(level);
        }

        public static List<DiceVisualData> GetAvailableDice(int level)
        {
            return DiceVisualCatalog.GetUnlocked(level);
        }

        public static bool IsBoardUnlocked(
            MainMenuBoardTheme theme,
            int level)
        {
            return BoardThemeCatalog.IsUnlocked(theme, level);
        }

        public static bool IsDiceUnlocked(
            DiceVisualType dice,
            int level)
        {
            return DiceVisualCatalog.IsUnlocked(dice, level);
        }

        public static int GetUnlockedBoardCount(int level)
        {
            return GetAvailableBoards(level).Count;
        }

        public static int GetUnlockedDiceCount(int level)
        {
            return GetAvailableDice(level).Count;
        }
    }
}
