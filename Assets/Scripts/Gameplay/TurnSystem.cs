
using System;

namespace ManchTak
{
    public enum TurnState
    {
        Waiting,
        Active,
        Completed,
        TimedOut
    }

    [Serializable]
    public class PlayerTurnData
    {
        public string PlayerId;
        public int SeatIndex;
        public TurnState State;
        public int RemainingSeconds;
        public bool HasPlayed;

        public PlayerTurnData(string playerId, int seatIndex)
        {
            PlayerId = playerId;
            SeatIndex = seatIndex;
            State = TurnState.Waiting;
            RemainingSeconds = TurnSystem.TurnDurationSeconds;
            HasPlayed = false;
        }
    }

    public static class TurnSystem
    {
        public const int TurnDurationSeconds = 30;

        // بررسی معتبر بودن تعداد بازیکنان
        public static bool IsValidPlayerCount(int count)
        {
            return count == 2 || count == 4;
        }

        // ساخت نوبت های بازیکنان
        public static PlayerTurnData[] CreateTurns(string[] playerIds)
        {
            if (playerIds == null ||
                !IsValidPlayerCount(playerIds.Length))
                return Array.Empty<PlayerTurnData>();

            PlayerTurnData[] turns =
                new PlayerTurnData[playerIds.Length];

            for (int i = 0; i < playerIds.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(playerIds[i]))
                    return Array.Empty<PlayerTurnData>();

                for (int j = 0; j < i; j++)
                {
                    if (playerIds[i] == playerIds[j])
                        return Array.Empty<PlayerTurnData>();
                }

                turns[i] = new PlayerTurnData(playerIds[i], i);
            }

            return turns;
        }

        // شروع نوبت بازیکن
        public static bool StartTurn(
            PlayerTurnData[] turns,
            int activeIndex)
        {
            if (!IsValidIndex(turns, activeIndex))
                return false;

            foreach (PlayerTurnData turn in turns)
            {
                turn.State = TurnState.Waiting;
                turn.HasPlayed = false;
                turn.RemainingSeconds = TurnDurationSeconds;
            }

            turns[activeIndex].State = TurnState.Active;

            return true;
        }

        // بررسی نوبت بازیکن
        public static bool IsPlayerTurn(
            PlayerTurnData[] turns,
            string playerId)
        {
            if (turns == null ||
                string.IsNullOrWhiteSpace(playerId))
                return false;

            foreach (PlayerTurnData turn in turns)
            {
                if (turn.PlayerId == playerId)
                    return turn.State == TurnState.Active;
            }

            return false;
        }

        // ثبت حرکت بازیکن
        public static bool CompleteTurn(
            PlayerTurnData[] turns,
            string playerId)
        {
            if (!IsPlayerTurn(turns, playerId))
                return false;

            foreach (PlayerTurnData turn in turns)
            {
                if (turn.PlayerId == playerId)
                {
                    turn.HasPlayed = true;
                    turn.State = TurnState.Completed;
                    return true;
                }
            }

            return false;
        }

        // کاهش زمان نوبت
        public static bool UpdateTimer(
            PlayerTurnData[] turns,
            int activeIndex,
            int elapsedSeconds)
        {
            if (!IsValidIndex(turns, activeIndex) ||
                elapsedSeconds < 0)
                return false;

            PlayerTurnData turn = turns[activeIndex];

            if (turn.State != TurnState.Active)
                return false;

            turn.RemainingSeconds -= elapsedSeconds;

            if (turn.RemainingSeconds <= 0)
            {
                turn.RemainingSeconds = 0;
                turn.State = TurnState.TimedOut;
                return true;
            }

            return false;
        }

        // انتقال به بازیکن بعدی
        public static int GetNextPlayerIndex(
            PlayerTurnData[] turns,
            int currentIndex)
        {
            if (turns == null || turns.Length == 0)
                return -1;

            if (currentIndex < 0 || currentIndex >= turns.Length)
                return -1;

            return (currentIndex + 1) % turns.Length;
        }

        // بررسی معتبر بودن اندیس
        private static bool IsValidIndex(
            PlayerTurnData[] turns,
            int index)
        {
            return turns != null &&
                   index >= 0 &&
                   index < turns.Length;
        }
    }
}
