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

        public static bool IsValidPlayerCount(int count)
        {
            return count == 2 || count == 4;
        }

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

        public static bool StartTurn(
            PlayerTurnData[] turns,
            int activeIndex)
        {
            if (!IsValidIndex(turns, activeIndex))
                return false;

            for (int i = 0; i < turns.Length; i++)
            {
                turns[i].State = TurnState.Waiting;
                turns[i].HasPlayed = false;
                turns[i].RemainingSeconds = TurnDurationSeconds;
            }

            turns[activeIndex].State = TurnState.Active;

            return true;
        }

        public static bool IsPlayerTurn(
            PlayerTurnData[] turns,
            string playerId)
        {
            if (turns == null ||
                string.IsNullOrWhiteSpace(playerId))
                return false;

            foreach (PlayerTurnData turn in turns)
            {
                if (turn != null &&
                    turn.PlayerId == playerId)
                {
                    return turn.State == TurnState.Active;
                }
            }

            return false;
        }

        public static bool CompleteTurn(
            PlayerTurnData[] turns,
            string playerId)
        {
            if (!IsPlayerTurn(turns, playerId))
                return false;

            foreach (PlayerTurnData turn in turns)
            {
                if (turn != null && turn.PlayerId == playerId)
                {
                    turn.HasPlayed = true;
                    turn.State = TurnState.Completed;
                    return true;
                }
            }

            return false;
        }

        public static bool StartNextTurn(
            PlayerTurnData[] turns)
        {
            int currentIndex = GetActivePlayerIndex(turns);

            if (currentIndex < 0)
                return false;

            int nextIndex = GetNextPlayerIndex(turns, currentIndex);

            return StartTurn(turns, nextIndex);
        }

        public static bool AdvanceAfterTimeout(
            PlayerTurnData[] turns)
        {
            int activeIndex = GetActivePlayerIndex(turns);

            if (activeIndex < 0)
                return false;

            if (turns[activeIndex].State != TurnState.TimedOut)
                return false;

            int nextIndex = GetNextPlayerIndex(turns, activeIndex);

            return StartTurn(turns, nextIndex);
        }

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

        public static int GetActivePlayerIndex(
            PlayerTurnData[] turns)
        {
            if (turns == null)
                return -1;

            for (int i = 0; i < turns.Length; i++)
            {
                if (turns[i] != null &&
                    turns[i].State == TurnState.Active)
                    return i;
            }

            return -1;
        }

        public static int GetNextPlayerIndex(
            PlayerTurnData[] turns,
            int currentIndex)
        {
            if (turns == null ||
                turns.Length == 0 ||
                currentIndex < 0 ||
                currentIndex >= turns.Length)
                return -1;

            return (currentIndex + 1) % turns.Length;
        }

        private static bool IsValidIndex(
            PlayerTurnData[] turns,
            int index)
        {
            return turns != null &&
                   index >= 0 &&
                   index < turns.Length &&
                   turns[index] != null;
        }
    }
}
