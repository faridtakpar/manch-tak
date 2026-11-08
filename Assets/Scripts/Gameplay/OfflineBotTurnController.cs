using System;

namespace ManchTak
{
    public class OfflineBotTurnResult
    {
        public int DiceValue;
        public int PieceIndex;
        public PieceMovementEngine.MoveResult MoveResult;
        public bool ExtraTurn;
        public bool TurnAdvanced;
        public bool ThirdSix;
    }

    public static class OfflineBotTurnController
    {
        public static OfflineBotTurnResult PlayTurn(
            OfflineMatchSetup setup,
            PlayerData[] players,
            PlayerTurnData[] turns,
            int activeSeat,
            int[] playerStartCells,
            int[][] distancesToHomeEntry,
            int[] consecutiveSixes)
        {
            if (setup == null ||
                !setup.IsValid() ||
                players == null ||
                turns == null ||
                setup.Players == null ||
                playerStartCells == null ||
                distancesToHomeEntry == null ||
                consecutiveSixes == null)
                return null;

            if (activeSeat < 0 ||
                activeSeat >= setup.PlayerCount ||
                activeSeat >= players.Length ||
                activeSeat >= turns.Length ||
                activeSeat >= playerStartCells.Length ||
                activeSeat >= distancesToHomeEntry.Length ||
                activeSeat >= consecutiveSixes.Length ||
                distancesToHomeEntry[activeSeat] == null ||
                distancesToHomeEntry[activeSeat].Length != 4)
                return null;

            if (setup.Players[activeSeat].Type !=
                OfflinePlayerType.Bot)
                return null;

            PlayerData player = players[activeSeat];

            if (player == null ||
                player.Pieces == null ||
                player.Pieces.Length != 4 ||
                player.PlayerId != setup.Players[activeSeat].PlayerId ||
                TurnSystem.GetActivePlayerIndex(turns) != activeSeat)
                return null;

            DiceRollData roll = DiceRollSystem.Roll(
                player.PlayerId,
                consecutiveSixes[activeSeat]
            );

            if (roll == null)
                return null;

            consecutiveSixes[activeSeat] =
                roll.ConsecutiveSixes;

            OfflineBotTurnResult result =
                new OfflineBotTurnResult();

            result.DiceValue = roll.Value;
            result.PieceIndex = -1;
            result.ExtraTurn =
                DiceRollSystem.ShouldKeepTurn(roll);

            if (DiceRollSystem.ShouldSkipTurn(roll))
            {
                result.ThirdSix = true;
                consecutiveSixes[activeSeat] = 0;

                result.TurnAdvanced =
                    AdvanceTurn(turns, activeSeat);

                return result;
            }

            int pieceIndex = BotMoveSelector.ChoosePiece(
                player,
                roll.Value,
                playerStartCells[activeSeat],
                distancesToHomeEntry[activeSeat]
            );

            if (pieceIndex < 0)
            {
                if (!result.ExtraTurn)
                {
                    result.TurnAdvanced =
                        AdvanceTurn(turns, activeSeat);
                }

                return result;
            }

            PieceData piece = player.Pieces[pieceIndex];

            PieceMovementEngine.MoveResult move =
                PieceMovementEngine.TryApplyMove(
                    piece,
                    roll.Value,
                    playerStartCells[activeSeat],
                    distancesToHomeEntry[activeSeat][pieceIndex]
                );

            result.PieceIndex = pieceIndex;
            result.MoveResult = move;

            if (move ==
                    PieceMovementEngine.MoveResult.InvalidInput ||
                move ==
                    PieceMovementEngine.MoveResult.NoLegalMove)
            {
                result.PieceIndex = -1;

                if (!result.ExtraTurn)
                {
                    result.TurnAdvanced =
                        AdvanceTurn(turns, activeSeat);
                }

                return result;
            }

            if (!result.ExtraTurn)
            {
                result.TurnAdvanced =
                    AdvanceTurn(turns, activeSeat);
            }

            return result;
        }

        private static bool AdvanceTurn(
            PlayerTurnData[] turns,
            int currentSeat)
        {
            int nextSeat =
                TurnSystem.GetNextPlayerIndex(turns, currentSeat);

            if (nextSeat < 0)
                return false;

            return TurnSystem.StartTurn(turns, nextSeat);
        }
    }
}
