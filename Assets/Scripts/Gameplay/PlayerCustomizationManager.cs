
namespace ManchTak
{
    public class PlayerCustomizationManager
    {
        public BoardThemeController BoardController { get; private set; }

        public DiceVisualController DiceController { get; private set; }

        public PlayerCustomizationManager(int playerLevel)
        {
            int level = playerLevel < 1 ? 1 : playerLevel;

            BoardController = new BoardThemeController(level);
            DiceController = new DiceVisualController(level);

            LoadSelections();
        }

        private void LoadSelections()
        {
            MainMenuBoardTheme savedBoard =
                BoardThemeStorage.Load();

            DiceVisualType savedDice =
                DiceVisualStorage.Load();

            if (BoardController.IsAvailable(savedBoard))
            {
                BoardController.SelectTheme(savedBoard);
            }

            if (DiceController.IsAvailable(savedDice))
            {
                DiceController.SelectDice(savedDice);
            }
        }

        public bool SelectBoard(MainMenuBoardTheme theme)
        {
            if (!BoardController.SelectTheme(theme))
                return false;

            BoardThemeStorage.Save(theme);
            return true;
        }

        public bool SelectDice(DiceVisualType dice)
        {
            if (!DiceController.SelectDice(dice))
                return false;

            DiceVisualStorage.Save(dice);
            return true;
        }

        public void UpdatePlayerLevel(int newLevel)
        {
            BoardController.UpdatePlayerLevel(newLevel);
            DiceController.UpdatePlayerLevel(newLevel);
        }

        public MainMenuBoardTheme GetSelectedBoard()
        {
            return BoardController.SelectedTheme;
        }

        public DiceVisualType GetSelectedDice()
        {
            return DiceController.SelectedDice;
        }
    }
}
