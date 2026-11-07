
using System;

namespace ManchTak
{
    public class MainMenuController
    {
        public MainMenuPage CurrentPage { get; private set; }

        public MainMenuGameMode SelectedGameMode { get; private set; }

        public MainMenuBoardTheme SelectedBoard { get; private set; }

        public event Action<MainMenuPage> OnPageChanged;

        public event Action<MainMenuGameMode> OnGameModeChanged;

        public event Action<MainMenuBoardTheme> OnBoardChanged;

        public MainMenuController()
        {
            CurrentPage = MainMenuConfig.DefaultPage;
            SelectedGameMode = MainMenuConfig.DefaultGameMode;
            SelectedBoard = MainMenuConfig.DefaultBoard;
        }

        // رفتن به صفحه موردنظر
        public bool OpenPage(MainMenuPage page)
        {
            if (!MainMenuConfig.IsValidPage(page))
                return false;

            CurrentPage = page;

            OnPageChanged?.Invoke(CurrentPage);

            return true;
        }

        // بازگشت به صفحه اصلی
        public void GoHome()
        {
            OpenPage(MainMenuPage.Home);
        }

        // انتخاب حالت بازی
        public bool SelectGameMode(MainMenuGameMode mode)
        {
            if (!MainMenuConfig.IsValidGameMode(mode))
                return false;

            SelectedGameMode = mode;

            OnGameModeChanged?.Invoke(mode);

            return true;
        }

        // انتخاب تخته بازی
        public bool SelectBoard(MainMenuBoardTheme board)
        {
            if (!MainMenuConfig.IsValidBoard(board))
                return false;

            SelectedBoard = board;

            OnBoardChanged?.Invoke(board);

            return true;
        }

        // بررسی صفحه فعلی
        public bool IsCurrentPage(MainMenuPage page)
        {
            return CurrentPage == page;
        }
    }
}
