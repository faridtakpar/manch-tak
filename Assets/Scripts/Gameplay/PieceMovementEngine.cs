
namespace ManchTak
{
    public static class PieceMovementEngine
    {
        public const int TrackLength = 52;
        public const int HomeLaneLength = 6;
        public const int PiecesPerPlayer = 4;

        public enum MoveResult
        {
            InvalidInput,
            NoLegalMove,
            LeftBase,
            MovedOnTrack,
            EnteredHomeLane,
            MovedInHomeLane,
            Finished
        }

        public static MoveResult TryApplyMove(
            PieceData piece,
            int diceValue,
            int playerStartCell,
            int distanceToHomeEntry)
        {
            if (piece == null ||
                diceValue < 1 ||
                diceValue > 6 ||
                playerStartCell < 0 ||
                playerStartCell >= TrackLength)
            {
                return MoveResult.InvalidInput;
            }

            switch (piece.State)
            {
                case PieceState.InBase:

                    if (diceValue != 6)
                        return MoveResult.NoLegalMove;

                    piece.State = PieceState.OnBoard;
                    piece.BoardPosition = playerStartCell;
                    piece.HomePathPosition = -1;

                    return MoveResult.LeftBase;

                case PieceState.OnBoard:

                    if (piece.BoardPosition < 0 ||
                        piece.BoardPosition >= TrackLength ||
                        distanceToHomeEntry < 1 ||
                        distanceToHomeEntry > TrackLength)
                    {
                        return MoveResult.InvalidInput;
                    }

                    if (diceValue < distanceToHomeEntry)
                    {
                        piece.BoardPosition =
                            (piece.BoardPosition + diceValue)
                            % TrackLength;

                        return MoveResult.MovedOnTrack;
                    }

                    int homeSteps =
                        diceValue - distanceToHomeEntry;

                    if (homeSteps > HomeLaneLength)
                        return MoveResult.NoLegalMove;

                    piece.BoardPosition = -1;

                    if (homeSteps == HomeLaneLength)
                    {
                        piece.State = PieceState.Finished;
                        piece.HomePathPosition = -1;

                        return MoveResult.Finished;
                    }

                    piece.State = PieceState.InHomePath;
                    piece.HomePathPosition = homeSteps;

                    return MoveResult.EnteredHomeLane;

                case PieceState.InHomePath:

                    if (piece.HomePathPosition < 0 ||
                        piece.HomePathPosition >= HomeLaneLength)
                    {
                        return MoveResult.InvalidInput;
                    }

                    int nextPosition =
                        piece.HomePathPosition + diceValue;

                    if (nextPosition > HomeLaneLength)
                        return MoveResult.NoLegalMove;

                    if (nextPosition == HomeLaneLength)
                    {
                        piece.State = PieceState.Finished;
                        piece.BoardPosition = -1;
                        piece.HomePathPosition = -1;

                        return MoveResult.Finished;
                    }

                    piece.HomePathPosition = nextPosition;

                    return MoveResult.MovedInHomeLane;

                case PieceState.Finished:

                    return MoveResult.NoLegalMove;

                default:

                    return MoveResult.InvalidInput;
            }
        }

        public static bool IsValidPieceIndex(int pieceIndex)
        {
            return pieceIndex >= 0 &&
                   pieceIndex < PiecesPerPlayer;
        }
    }
}
