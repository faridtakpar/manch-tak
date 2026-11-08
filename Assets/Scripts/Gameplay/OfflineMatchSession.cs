
using System;

namespace ManchTak
{
    [Serializable]
    public class OfflineMatchSession
    {
        public string MatchId;

        public OfflineMatchSetup Setup;
        public PlayerData[] Players;
        public PlayerTurnData[] Turns;
        public int[] ConsecutiveSixes;
        public PlayerMatchProgress[] Progress;
        public MatchPlayMode PlayMode;
        public MatchResult LastMatchResult;

        public DiceRollData CurrentDice;
        public bool HasRolled;

        private int[] startCellByColor;
        private int[] homeEntryCellByColor;

        public bool IsReady
        {
            get
            {
                if (Setup == null ||
                    !Setup.IsValid() ||
                    Players == null ||
                    Turns == null ||
                    Progress == null ||
                    ConsecutiveSixes == null ||
                    startCellByColor == null ||
                    homeEntryCellByColor == null)
                {
                    return false;
                }

                int count = Setup.PlayerCount;

                if (Players.Length != count ||
                    Turns.Length != count ||
                    Progress.Length != count ||
                    ConsecutiveSixes.Length != count ||
                    startCellByColor.Length != 4 ||
                    homeEntryCellByColor.Length != 4)
                {
                    return false;
                }

                for (int i = 0; i < count; i++)
                {
                    if (Players[i] == null ||
                        Players[i].Pieces == null ||
                        Players[i].Pieces.Length !=
                        PieceMovementEngine.PiecesPerPlayer ||
                        Progress[i] == null)
                    {
                        return false;
                    }
                }

                return true;
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
            {
                return null;
            }

            for (int i = 0; i < 4; i++)
            {
                if (startCellsByColor[i] < 0 ||
                    startCellsByColor[i] >=
                    PieceMovementEngine.TrackLength ||
                    homeEntryCellsByColor[i] < 0 ||
                    homeEntryCellsByColor[i] >=
                    PieceMovementEngine.TrackLength)
                {
                    return null;
                }
            }

            string safeDisplayName =
                string.IsNullOrWhiteSpace(humanDisplayName)
                    ? "بازیکن"
                    : humanDisplayName.Trim();

            OfflineMatchSetup setup =
                OfflineMatchFactory.Create(
                    playerCount,
                    humanPlayerId
                );

            if (setup == null || !setup.IsValid())
                return null;

            string[] ids = new string[playerCount];

            for (int i = 0; i < playerCount; i++)
            {
                if (setup.Players[i] == null)
                    return null;

                ids[i] = setup.Players[i].PlayerId;
            }

            PlayerTurnData[] turns =
                TurnSystem.CreateTurns(ids);

            if (turns == null ||
                turns.Length != playerCount ||
                !TurnSystem.StartTurn(turns, 0))
            {
                return null;
            }

            PlayerData[] players =
                new PlayerData[playerCount];

            PlayerMatchProgress[] progress =
                new PlayerMatchProgress[playerCount];

            for (int i = 0; i < playerCount; i++)
            {
                OfflinePlayerSlot slot = setup.Players[i];

                string displayName =
                    slot.Type == OfflinePlayerType.Human
                        ? safeDisplayName
                        : slot.DisplayName;

                players[i] = new PlayerData(
                    slot.PlayerId,
                    displayName,
                    slot.Color
                );

                if (players[i] == null ||
                    players[i].Pieces == null ||
                    players[i].Pieces.Length !=
                    PieceMovementEngine.PiecesPerPlayer)
                {
                    return null;
                }

                players[i].IsOnline = false;

                progress[i] =
                    new PlayerMatchProgress(slot.PlayerId);
            }

            OfflineMatchSession session =
                new OfflineMatchSession();

            session.MatchId =
                "OFFLINE_" + Guid.NewGuid().ToString("N");

            session.Setup = setup;
            session.Players = players;
            session.Turns = turns;
            session.Progress = progress;
            session.PlayMode = playMode;
            session.ConsecutiveSixes =
                new int[playerCount];

            session.CurrentDice = null;
            session.HasRolled = false;

            session.startCellByColor =
                (int[])startCellsByColor.Clone();

            session.homeEntryCellByColor =
                (int[])homeEntryCellsByColor.Clone();

            if (!session.IsReady)
                return null;

            return session;
        }

        public DiceRollData RollForHuman()
        {
            if (!IsReady ||
                HasRolled ||
                IsMatchCompleted())
            {
                return null;
            }

            int seat =
                TurnSystem.GetActivePlayerIndex(Turns);

            if (!IsValidSeat(seat) ||
                Setup.Players[seat].Type !=
                OfflinePlayerType.Human)
            {
                return null;
            }

            DiceRollData dice = DiceRollSystem.Roll(
                Players[seat].PlayerId,
                ConsecutiveSixes[seat]
            );

            if (dice == null)
                return null;

            ConsecutiveSixes[seat] =
                dice.ConsecutiveSixes;

            if (DiceRollSystem.ShouldSkipTurn(dice))
            {
                ConsecutiveSixes[seat] = 0;
                CurrentDice = null;
                HasRolled = false;

                AdvanceTurn(seat);

                return dice;
            }

            CurrentDice = dice;
            HasRolled = true;

            return dice;
        }

        public PieceMovementEngine.MoveResult ApplyHumanMove(
            int pieceIndex)
        {
            if (!IsReady ||
                !HasRolled ||
                CurrentDice == null ||
                IsMatchCompleted() ||
                !PieceMovementEngine.IsValidPieceIndex(pieceIndex))
            {
                return PieceMovementEngine.MoveResult.InvalidInput;
            }

            int seat =
                TurnSystem.GetActivePlayerIndex(Turns);

            if (!IsValidSeat(seat) ||
                Setup.Players[seat].Type !=
                OfflinePlayerType.Human)
            {
                return PieceMovementEngine.MoveResult.InvalidInput;
            }

            if (!DiceRollSystem.CanMovePiece(CurrentDice))
                return PieceMovementEngine.MoveResult.InvalidInput;

            if (Players[seat].Pieces == null ||
                Players[seat].Pieces.Length !=
                PieceMovementEngine.PiecesPerPlayer ||
                pieceIndex >= Players[seat].Pieces.Length)
            {
                return PieceMovementEngine.MoveResult.InvalidInput;
            }

            PieceData piece =
                Players[seat].Pieces[pieceIndex];

            if (piece == null ||
                !PieceMovementRules.CanMove(
                    piece,
                    CurrentDice.Value))
            {
                return PieceMovementEngine.MoveResult.NoLegalMove;
            }

            int colorIndex =
                (int)Players[seat].Color;

            if (colorIndex < 0 || colorIndex >= 4)
                return PieceMovementEngine.MoveResult.InvalidInput;

            int distance = GetDistanceToHomeEntry(
                piece,
                homeEntryCellByColor[colorIndex]
            );

            PieceMovementEngine.MoveResult result =
                PieceMovementEngine.TryApplyMove(
                    piece,
                    CurrentDice.Value,
                    startCellByColor[colorIndex],
                    distance
                );

            if (result ==
                    PieceMovementEngine.MoveResult.InvalidInput ||
                result ==
                    PieceMovementEngine.MoveResult.NoLegalMove)
            {
                return result;
            }

            bool extraTurn =
                DiceRollSystem.ShouldKeepTurn(CurrentDice);

            CurrentDice = null;
            HasRolled = false;

            RefreshProgressAndResult();

            if (IsMatchCompleted())
                return result;

            if (!extraTurn)
                AdvanceTurn(seat);

            return result;
        }

        public bool FinishHumanTurnWithoutMove()
        {
            if (!IsReady ||
                !HasRolled ||
                CurrentDice == null ||
                IsMatchCompleted())
            {
                return false;
            }

            int seat =
                TurnSystem.GetActivePlayerIndex(Turns);

            if (!IsValidSeat(seat) ||
                Setup.Players[seat].Type !=
                OfflinePlayerType.Human)
            {
                return false;
            }

            if (DiceRollSystem.ShouldKeepTurn(CurrentDice))
            {
                CurrentDice = null;
                HasRolled = false;

                return true;
            }

            CurrentDice = null;
            HasRolled = false;

            AdvanceTurn(seat);

            return true;
        }

        public OfflineBotTurnResult PlayActiveBotTurn()
        {
            if (!IsReady || IsMatchCompleted())
                return null;

            int activeSeat =
                TurnSystem.GetActivePlayerIndex(Turns);

            if (!IsValidSeat(activeSeat) ||
                Setup.Players[activeSeat].Type !=
                OfflinePlayerType.Bot)
            {
                return null;
            }

            int[] playerStartCells =
                new int[Setup.PlayerCount];

            int[][] distances =
                new int[Setup.PlayerCount][];

            for (int seat = 0;
                 seat < Setup.PlayerCount;
                 seat++)
            {
                if (Players[seat] == null ||
                    Players[seat].Pieces == null ||
                    Players[seat].Pieces.Length !=
                    PieceMovementEngine.PiecesPerPlayer)
                {
                    return null;
                }

                int colorIndex =
                    (int)Players[seat].Color;

                if (colorIndex < 0 || colorIndex >= 4)
                    return null;

                playerStartCells[seat] =
                    startCellByColor[colorIndex];

                distances[seat] =
                    new int[PieceMovementEngine.PiecesPerPlayer];

                for (int pieceIndex = 0;
                     pieceIndex <
                     PieceMovementEngine.PiecesPerPlayer;
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

            OfflineBotTurnResult result =
                OfflineBotTurnController.PlayTurn(
                    Setup,
                    Players,
                    Turns,
                    activeSeat,
                    playerStartCells,
                    distances,
                    ConsecutiveSixes
                );

            if (result == null)
                return null;

            CurrentDice = null;
            HasRolled = false;

            RefreshProgressAndResult();

            return result;
        }

        public void RefreshProgressAndResult()
        {
            if (!IsReady)
                return;

            for (int i = 0; i < Players.Length; i++)
            {
                if (Players[i] == null ||
                    Players[i].Pieces == null ||
                    Players[i].Pieces.Length !=
                    PieceMovementEngine.PiecesPerPlayer ||
                    Progress[i] == null)
                {
                    return;
                }

                int finished = 0;

                for (int p = 0;
                     p < Players[i].Pieces.Length;
                     p++)
                {
                    PieceData piece =
                        Players[i].Pieces[p];

                    if (piece != null &&
                        piece.State == PieceState.Finished)
                    {
                        finished++;
                    }
                }

                Progress[i].FinishedPieces = finished;
            }

            if (string.IsNullOrWhiteSpace(MatchId))
            {
                MatchId =
                    "OFFLINE_" + Guid.NewGuid().ToString("N");
            }

            LastMatchResult =
                MatchResultSystem.CheckMatchResult(
                    MatchId,
                    Progress,
                    PlayMode
                );
        }

        private void AdvanceTurn(int currentSeat)
        {
            if (!IsValidSeat(currentSeat))
                return;

            int nextSeat =
                TurnSystem.GetNextPlayerIndex(
                    Turns,
                    currentSeat
                );

            if (nextSeat >= 0)
                TurnSystem.StartTurn(Turns, nextSeat);
        }

        private bool IsMatchCompleted()
        {
            return LastMatchResult != null &&
                   LastMatchResult.Status ==
                   MatchResultStatus.Completed;
        }

        private bool IsValidSeat(int seat)
        {
            return seat >= 0 &&
                   seat < Setup.PlayerCount &&
                   seat < Players.Length;
        }

        private static int GetDistanceToHomeEntry(
            PieceData piece,
            int homeEntryCell)
        {
            if (piece == null ||
                piece.State != PieceState.OnBoard)
            {
                return 1;
            }

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
