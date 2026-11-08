using System;

namespace ManchTak
{
    [Serializable]
    public class OfflineMatchSession
    {
        public OfflineMatchSetup Setup;
        public PlayerData[] Players;
        public PlayerTurnData[] Turns;
        public int[] ConsecutiveSixes;
        public PlayerMatchProgress[] Progress;
        public MatchPlayMode PlayMode;
        public MatchResult LastMatchResult;

        private int[] startCellByColor;
        private int[] homeEntryCellByColor;

        public bool IsReady
        {
            get
            {
                return Setup != null &&
                       Setup.IsValid() &&
                       Players != null &&
                       Turns != null &&
                       Players.Length == Setup.PlayerCount &&
                       Turns.Length == Setup.PlayerCount;
            }
        }

        public static OfflineMatchSession Create(
            int playerCount,
            string humanPlayerId,
            string humanDisplayName,
            MatchPlayMode playMode,
            int[] startCellsByColor,
            int[] homeEntryCellsByColor)
        {
            if ((playerCount != 2 && playerCount != 4) ||
                string.IsNullOrWhiteSpace(humanPlayerId) ||
                startCellsByColor == null ||
                homeEntryCellsByColor == null ||
                startCellsByColor.Length != 4 ||
                homeEntryCellsByColor.Length != 4 ||
                MatchResultSystem.GetRequiredFinishedPieces(playMode) < 0)
                return null;

            for (int i = 0; i < 4; i++)
            {
                if (startCellsByColor[i] < 0 ||
                    startCellsByColor[i] >= PieceMovementEngine.TrackLength ||
                    homeEntryCellsByColor[i] < 0 ||
                    homeEntryCellsByColor[i] >= PieceMovementEngine.TrackLength)
                    return null;
            }

            OfflineMatchSetup setup =
                OfflineMatchFactory.Create(playerCount, humanPlayerId);

            if (setup == null || !setup.IsValid())
                return null;

            string[] ids = new string[playerCount];

            for (int i = 0; i < playerCount; i++)
                ids[i] = setup.Players[i].PlayerId;

            PlayerTurnData[] turns = TurnSystem.CreateTurns(ids);

            if (turns == null || turns.Length != playerCount ||
                !TurnSystem.StartTurn(turns, 0))
                return null;

            PlayerData[] players = new PlayerData[playerCount];
            PlayerMatchProgress[] progress =
                new PlayerMatchProgress[playerCount];

            for (int i = 0; i < playerCount; i++)
            {
                OfflinePlayerSlot slot = setup.Players[i];

                players[i] = new PlayerData(
                    slot.PlayerId,
                    slot.DisplayName,
                    slot.Color
                );

                players[i].IsOnline = false;
                progress[i] = new PlayerMatchProgress(slot.PlayerId);
            }

            OfflineMatchSession session = new OfflineMatchSession();

            session.Setup = setup;
            session.Players = players;
            session.Turns = turns;
            session.Progress = progress;
            session.PlayMode = playMode;
            session.ConsecutiveSixes = new int[playerCount];

            session.startCellByColor =
                (int[])startCellsByColor.Clone();

            session.homeEntryCellByColor =
                (int[])homeEntryCellsByColor.Clone();

            return session;
        }

        public OfflineBotTurnResult PlayActiveBotTurn()
        {
            if (!IsReady ||
                (LastMatchResult != null &&
                 LastMatchResult.Status == MatchResultStatus.Completed))
                return null;

            int activeSeat =
                TurnSystem.GetActivePlayerIndex(Turns);

            if (activeSeat < 0 ||
                activeSeat >= Setup.PlayerCount ||
                Setup.Players[activeSeat].Type != OfflinePlayerType.Bot)
                return null;

            int[] playerStartCells =
                new int[Setup.PlayerCount];

            int[][] distances =
                new int[Setup.PlayerCount][];

            for (int seat = 0; seat < Setup.PlayerCount; seat++)
            {
                int colorIndex = (int)Players[seat].Color;

                playerStartCells[seat] =
                    startCellByColor[colorIndex];

                distances[seat] =
                    new int[PieceMovementEngine.PiecesPerPlayer];

                for (int pieceIndex = 0;
                     pieceIndex < PieceMovementEngine.PiecesPerPlayer;
                     pieceIndex++)
                {
                    PieceData piece =
                        Players[seat].Pieces[pieceIndex];

                    distances[seat][pieceIndex] =
                        GetDistanceToHomeEntry(
                            piece,
                            homeEntryCellByColor[colorIndex]
                        );
                }
            }

            OfflineBotTurnResult turnResult =
                OfflineBotTurnController.PlayTurn(
                    Setup,
                    Players,
                    Turns,
                    activeSeat,
                    playerStartCells,
                    distances,
                    ConsecutiveSixes
                );

            if (turnResult == null)
                return null;

            RefreshProgressAndResult();
            return turnResult;
        }

        public void RefreshProgressAndResult()
        {
            if (!IsReady || Progress == null)
                return;

            for (int i = 0; i < Players.Length; i++)
            {
                int finished = 0;

                for (int p = 0; p < Players[i].Pieces.Length; p++)
                {
                    if (Players[i].Pieces[p].State == PieceState.Finished)
                        finished++;
                }

                Progress[i].FinishedPieces = finished;
            }

            LastMatchResult = MatchResultSystem.CheckMatchResult(
                "OFFLINE_" + Setup.Players[0].PlayerId,
                Progress,
                PlayMode
            );
        }

        private static int GetDistanceToHomeEntry(
            PieceData piece,
            int homeEntryCell)
        {
            if (piece == null || piece.State != PieceState.OnBoard)
                return 1;

            int distance =
                (homeEntryCell - piece.BoardPosition +
                 PieceMovementEngine.TrackLength) %
                PieceMovementEngine.TrackLength;

            return distance == 0
                ? PieceMovementEngine.TrackLength
                : distance;
        }
    }
}
