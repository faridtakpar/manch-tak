namespace ManchTak
{
    public static class PieceMovementRules
    {
        public const int PiecesPerPlayer = 4;
        public const int MainTrackLength = 52;
        public const int HomePathLength = 6;

        // بررسی خروج مهره از خانه
        public static bool CanLeaveBase(
            PieceData piece,
            int diceValue)
        {
            return piece != null
                && piece.State == PieceState.InBase
                && DiceRules.IsValidRoll(diceValue)
                && diceValue == 6;
        }

        // بررسی امکان حرکت مهره
        public static bool CanMove(
            PieceData piece,
            int diceValue)
        {
            if (piece == null || !DiceRules.IsValidRoll(diceValue))
                return false;

            switch (piece.State)
            {
                case PieceState.InBase:
                    return CanLeaveBase(piece, diceValue);

                case PieceState.OnBoard:
                case PieceState.InHomePath:
                    return true;

                case PieceState.Finished:
                default:
                    return false;
            }
        }

        // خروج مهره با خانه شروع پیش فرض
        public static bool LeaveBase(PieceData piece)
        {
            return LeaveBase(piece, 0);
        }

        // خروج مهره با خانه شروع اختصاصی رنگ
        public static bool LeaveBase(
            PieceData piece,
            int playerStartCell)
        {
            if (piece == null ||
                piece.State != PieceState.InBase ||
                playerStartCell < 0 ||
                playerStartCell >= MainTrackLength)
            {
                return false;
            }

            PieceMovementEngine.MoveResult result =
                PieceMovementEngine.TryApplyMove(
                    piece,
                    6,
                    playerStartCell,
                    1
                );

            return result == PieceMovementEngine.MoveResult.LeftBase;
        }

        // محاسبه موقعیت جدید روی مسیر اصلی
        public static int CalculateBoardPosition(
            int currentPosition,
            int diceValue)
        {
            if (currentPosition < 0 ||
                currentPosition >= MainTrackLength ||
                !DiceRules.IsValidRoll(diceValue))
            {
                return -1;
            }

            return (currentPosition + diceValue) % MainTrackLength;
        }

        // بررسی ورود به مسیر پایانی
        public static bool CanEnterHomePath(
            int distanceToHome,
            int diceValue)
        {
            if (distanceToHome < 1 ||
                distanceToHome > MainTrackLength ||
                !DiceRules.IsValidRoll(diceValue))
            {
                return false;
            }

            return diceValue >= distanceToHome;
        }

        // بررسی رسیدن دقیق به مقصد
        public static bool CanFinish(
            int currentHomePosition,
            int diceValue)
        {
            return currentHomePosition >= 0
                && currentHomePosition < HomePathLength
                && DiceRules.IsValidRoll(diceValue)
                && currentHomePosition + diceValue == HomePathLength;
        }

        // اجرای حرکت کامل با موتور جدید
        public static PieceMovementEngine.MoveResult TryApplyMove(
            PieceData piece,
            int diceValue,
            int playerStartCell,
            int distanceToHomeEntry)
        {
            return PieceMovementEngine.TryApplyMove(
                piece,
                diceValue,
                playerStartCell,
                distanceToHomeEntry
            );
        }

        // اعتبارسنجی شماره مهره
        public static bool IsValidPieceIndex(int pieceIndex)
        {
            return pieceIndex >= 0 &&
                   pieceIndex < PiecesPerPlayer;
        }
    }
}
