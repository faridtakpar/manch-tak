using System;

namespace ManchTak
{
    public static class BotMoveSelector
    {
        public static int ChoosePiece(
            PlayerData player,
            int diceValue,
            int playerStartCell,
            int[] distancesToHomeEntry)
        {
            if (player == null ||
                player.Pieces == null ||
                player.Pieces.Length != 4 ||
                distancesToHomeEntry == null ||
                distancesToHomeEntry.Length != 4 ||
                !DiceRules.IsValidRoll(diceValue))
            {
                return -1;
            }

            int bestPiece = -1;
            int bestScore = int.MinValue;

            for (int i = 0; i < 4; i++)
            {
                PieceData original = player.Pieces[i];

                if (!PieceMovementRules.CanMove(
                    original, diceValue))
                {
                    continue;
                }

                PieceData test = new PieceData(
                    original.PieceId,
                    original.OwnerColor
                );

                test.State = original.State;
                test.BoardPosition = original.BoardPosition;
                test.HomePathPosition = original.HomePathPosition;

                PieceMovementEngine.MoveResult result =
                    PieceMovementEngine.TryApplyMove(
                        test,
                        diceValue,
                        playerStartCell,
                        distancesToHomeEntry[i]
                    );

                int score = GetScore(result, test);

                if (score > bestScore)
                {
                    bestScore = score;
                    bestPiece = i;
                }
            }

            return bestPiece;
        }

        private static int GetScore(
            PieceMovementEngine.MoveResult result,
            PieceData piece)
        {
            switch (result)
            {
                case PieceMovementEngine.MoveResult.Finished:
                    return 10000;

                case PieceMovementEngine.MoveResult.EnteredHomeLane:
                    return 7000;

                case PieceMovementEngine.MoveResult.MovedInHomeLane:
                    return 6000 + piece.HomePathPosition;

                case PieceMovementEngine.MoveResult.LeftBase:
                    return 5000;

                case PieceMovementEngine.MoveResult.MovedOnTrack:
                    return 1000;

                default:
                    return -1;
            }
        }
    }
}
