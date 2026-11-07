
using System;

namespace ManchTak
{
    public class DiceVisualController
    {
        public DiceVisualType SelectedDice { get; private set; }

        public int PlayerLevel { get; private set; }

        public event Action<DiceVisualType> OnDiceChanged;

        public DiceVisualController(int playerLevel)
        {
            PlayerLevel = Math.Max(1, playerLevel);
            SelectedDice = DiceVisualType.Classic;
        }

        public bool SelectDice(DiceVisualType dice)
        {
            if (!DiceVisualCatalog.IsUnlocked(dice, PlayerLevel))
                return false;

            SelectedDice = dice;

            OnDiceChanged?.Invoke(SelectedDice);

            return true;
        }

        public void UpdatePlayerLevel(int newLevel)
        {
            PlayerLevel = Math.Max(1, newLevel);
        }

        public bool IsSelected(DiceVisualType dice)
        {
            return SelectedDice == dice;
        }

        public bool IsAvailable(DiceVisualType dice)
        {
            return DiceVisualCatalog.IsUnlocked(
                dice,
                PlayerLevel
            );
        }

        public DiceVisualData GetSelectedDiceData()
        {
            return DiceVisualCatalog.Find(SelectedDice);
        }
    }
}
