
namespace ManchTak
{
    public static class PieceMovementRules
    {
        public const int PiecesPerPlayer = 4;
        public const int MainTrackLength = 52;
        public const int HomePathLength = 6;

        // بررسی امکان خروج مهره از خانه
        public static bool CanLeaveBase(
            PieceData piece,
            int diceValue)
        {
            if (piece == null)
                return false;

            return piece.State == PieceState.InBase
                   && diceValue == 6;
        }

        // بررسی امکان حرکت مهره
        public static bool CanMove(
            PieceData piece,
            int diceValue)
        {
            if (piece == null || !DiceRules.IsValidRoll(diceValue))
                return false;

            if (piece.State == PieceState.InBase)
                return CanLeaveBase(piece, diceValue);

            if (piece.State == PieceState.Finished)
                return false;

            return true;
        }

        // خارج کردن مهره از خانه
        public static bool LeaveBase(PieceData piece)
        {
            if (piece == null || piece.State != PieceState.InBase)
                return false;

            piece.State = PieceState.OnBoard;
            piece.BoardPosition = 0;
            piece.HomePathPosition = -1;

            return true;
        }

        // محاسبه موقعیت جدید مهره در مسیر
        public static int CalculateBoardPosition(
            int currentPosition,
            int diceValue)
        {
            if (currentPosition < 0 || currentPosition >= MainTrackLength)
                return -1;

            if (!DiceRules.IsValidRoll(diceValue))
                return -1;

            return (currentPosition + diceValue) % MainTrackLength;
        }

        // بررسی ورود به مسیر پایانی
        public static bool CanEnterHomePath(
            int distanceToHome,
            int diceValue)
        {
            if (distanceToHome < 1)
                return false;

            if (!DiceRules.IsValidRoll(diceValue))
                return false;

            return diceValue >= distanceToHome;
        }

        // بررسی رسیدن به مقصد
        public static bool CanFinish(
            int currentHomePosition,
            int diceValue)
        {
            return currentHomePosition >= 0
                   && currentHomePosition < HomePathLength
                   && currentHomePosition + diceValue == HomePathLength;
        }
    }
}
