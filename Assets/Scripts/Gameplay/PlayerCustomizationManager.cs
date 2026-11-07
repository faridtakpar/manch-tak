
using System;

namespace ManchTak
{
    public class PlayerCustomizationManager
    {
        public BoardThemeController BoardController { get; private set; }

        public DiceVisualController DiceController { get; private set; }

        private readonly string playerId;

        public PlayerCustomizationManager(
            string playerId,
            int playerLevel)
        {
            if (string.IsNullOrWhiteSpace(playerId))
            {
                throw new ArgumentException(
                    "شناسه بازیکن معتبر نیست.",
                    nameof(playerId)
                );
            }

            this.playerId = playerId;

            int level = Math.Max(1, playerLevel);

            BoardController = new BoardThemeController(level);
            DiceController = new DiceVisualController(level);

            LoadSelections();
        }

        private void LoadSelections()
        {
            MainMenuBoardTheme savedBoard =
                BoardThemeStorage.Load(playerId);

            DiceVisualType savedDice =
                DiceVisualStorage.Load(playerId);

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

            BoardThemeStorage.Save(playerId, theme);

            return true;
        }

        public bool SelectDice(DiceVisualType dice)
        {
            if (!DiceController.SelectDice(dice))
                return false;

            DiceVisualStorage.Save(playerId, dice);

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
