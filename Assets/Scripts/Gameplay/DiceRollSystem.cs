using System;

namespace ManchTak
{
    public enum DiceRollResult
    {
        Normal,
        ExtraTurn,
        ThirdSix
    }

    [Serializable]
    public class DiceRollData
    {
        public int Value;
        public int ConsecutiveSixes;
        public DiceRollResult Result;
        public string PlayerId;
    }

    public static class DiceRollSystem
    {
        private const int MinimumDiceValue = 1;
        private const int MaximumDiceValue = 6;
        private const int MaximumConsecutiveSixes = 3;

        private static readonly Random RandomGenerator =
            new Random();

        public static DiceRollData Roll(string playerId)
        {
            if (string.IsNullOrWhiteSpace(playerId))
                return null;

            int value;

            lock (RandomGenerator)
            {
                value = RandomGenerator.Next(
                    MinimumDiceValue,
                    MaximumDiceValue + 1);
            }

            return new DiceRollData
            {
                Value = value,
                PlayerId = playerId,
                ConsecutiveSixes = value == 6 ? 1 : 0,
                Result = value == 6
                    ? DiceRollResult.ExtraTurn
                    : DiceRollResult.Normal
            };
        }

        public static DiceRollData Roll(
            string playerId,
            int previousConsecutiveSixes)
        {
            if (string.IsNullOrWhiteSpace(playerId) ||
                previousConsecutiveSixes < 0 ||
                previousConsecutiveSixes >= MaximumConsecutiveSixes)
                return null;

            DiceRollData result = Roll(playerId);

            if (result == null)
                return null;

            result.ConsecutiveSixes =
                result.Value == 6
                    ? previousConsecutiveSixes + 1
                    : 0;

            if (result.ConsecutiveSixes >= MaximumConsecutiveSixes)
            {
                result.Result = DiceRollResult.ThirdSix;
            }
            else if (result.Value == 6)
            {
                result.Result = DiceRollResult.ExtraTurn;
            }
            else
            {
                result.Result = DiceRollResult.Normal;
            }

            return result;
        }

        public static bool IsValidDiceValue(int value)
        {
            return value >= MinimumDiceValue &&
                   value <= MaximumDiceValue;
        }

        public static bool ShouldKeepTurn(DiceRollData result)
        {
            return result != null &&
                   result.Result == DiceRollResult.ExtraTurn;
        }

        public static bool ShouldSkipTurn(DiceRollData result)
        {
            return result != null &&
                   result.Result == DiceRollResult.ThirdSix;
        }

        public static bool CanMovePiece(DiceRollData result)
        {
            return result != null &&
                   IsValidDiceValue(result.Value) &&
                   result.Result != DiceRollResult.ThirdSix;
        }
    }
}
