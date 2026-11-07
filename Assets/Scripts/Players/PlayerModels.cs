
namespace ManchTak
{
    // رنگ بازیکنان
    public enum PlayerColor
    {
        Red,
        Blue,
        Green,
        Yellow
    }

    // وضعیت مهره
    public enum PieceState
    {
        InBase,
        OnBoard,
        InHomePath,
        Finished
    }

    // مدل مهره
    [System.Serializable]
    public class PieceData
    {
        public int PieceId;
        public PlayerColor OwnerColor;
        public PieceState State;
        public int BoardPosition;
        public int HomePathPosition;

        public PieceData(int id, PlayerColor color)
        {
            PieceId = id;
            OwnerColor = color;
            State = PieceState.InBase;
            BoardPosition = -1;
            HomePathPosition = -1;
        }
    }

    // مدل بازیکن
    [System.Serializable]
    public class PlayerData
    {
        public string PlayerId;
        public string DisplayName;
        public PlayerColor Color;
        public int Level;
        public int XP;
        public bool IsOnline;

        public PieceData[] Pieces;

        public PlayerData(
            string playerId,
            string displayName,
            PlayerColor color)
        {
            PlayerId = playerId;
            DisplayName = displayName;
            Color = color;

            Level = 1;
            XP = 0;
            IsOnline = true;

            Pieces = new PieceData[4];

            for (int i = 0; i < Pieces.Length; i++)
            {
                Pieces[i] = new PieceData(i, color);
            }
        }
    }
}
