using System;

namespace ManchTak
{
    public class BoardThemeController
    {
        public MainMenuBoardTheme SelectedTheme
        {
            get;
            private set;
        }

        public int PlayerLevel
        {
            get;
            private set;
        }

        public event Action<MainMenuBoardTheme> OnThemeChanged;

        public BoardThemeController(int playerLevel)
        {
            PlayerLevel = Math.Max(1, playerLevel);

            SelectedTheme = MainMenuBoardTheme.Classic;
        }

        public bool SelectTheme(MainMenuBoardTheme theme)
        {
            if (!IsAvailable(theme))
                return false;

            if (SelectedTheme == theme)
                return true;

            SelectedTheme = theme;

            OnThemeChanged?.Invoke(SelectedTheme);

            return true;
        }

        public void UpdatePlayerLevel(int newLevel)
        {
            PlayerLevel = Math.Max(1, newLevel);
        }

        public bool IsSelected(MainMenuBoardTheme theme)
        {
            return SelectedTheme == theme;
        }

        public bool IsAvailable(MainMenuBoardTheme theme)
        {
            return BoardThemeCatalog.IsUnlocked(
                theme,
                PlayerLevel
            );
        }

        public BoardThemeData GetSelectedThemeData()
        {
            return BoardThemeCatalog.Find(SelectedTheme);
        }
    }
}
