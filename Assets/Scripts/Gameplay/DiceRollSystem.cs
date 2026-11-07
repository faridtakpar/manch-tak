
using System;

namespace ManchTak
{
    public enum DiceRollStatus
    {
        Success,
        InvalidPlayer,
        InvalidDiceValue,
        ThirdSix,
        RetryRequired
    }

    [Serializable]
    public class DiceRollResult
    {
        public string PlayerId;
        public int DiceValue;
        public int ConsecutiveSixes;
        public DiceRollStatus Status;
        public long CreatedAt;

        public DiceRollResult(
            string playerId,
            int diceValue,
            int consecutiveSixes,
            DiceRollStatus status)
        {
            PlayerId = playerId;
            DiceValue = diceValue;
            ConsecutiveSixes = consecutiveSixes;
            Status = status;
            CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }
    }

    public static class DiceRollSystem
    {
        public const int MinimumDiceValue = 1;
        public const int MaximumDiceValue = 6;
        public const int ThirdSixRetryCost = 2;

        // بررسی معتبر بودن نتیجه تاس
        public static bool IsValidDiceValue(int value)
        {
            return value >= MinimumDiceValue &&
                   value <= MaximumDiceValue;
        }

        // ثبت نتیجه پرتاب تاس
        public static DiceRollResult ProcessRoll(
            string playerId,
            int diceValue,
            int previousConsecutiveSixes)
        {
            if (string.IsNullOrWhiteSpace(playerId))
            {
                return new DiceRollResult(
                    playerId,
                    diceValue,
                    previousConsecutiveSixes,
                    DiceRollStatus.InvalidPlayer
                );
            }

            if (!IsValidDiceValue(diceValue))
            {
                return new DiceRollResult(
                    playerId,
                    diceValue,
                    previousConsecutiveSixes,
                    DiceRollStatus.InvalidDiceValue
                );
            }

            int consecutiveSixes = diceValue == 6
                ? previousConsecutiveSixes + 1
                : 0;

            if (consecutiveSixes >= 3)
            {
                return new DiceRollResult(
                    playerId,
                    diceValue,
                    0,
                    DiceRollStatus.ThirdSix
                );
            }

            return new DiceRollResult(
                playerId,
                diceValue,
                consecutiveSixes,
                DiceRollStatus.Success
            );
        }

        // بررسی نیاز به تلاش مجدد
        public static bool CanRetryAfterThirdSix(
            int diamondBalance)
        {
            return diamondBalance >= ThirdSixRetryCost;
        }

        // هزینه الماس تلاش مجدد
        public static int GetRetryCost()
        {
            return ThirdSixRetryCost;
        }
    }
}
