using System;
using Unity.Netcode;
using Unity.Collections;

namespace ManchTak
{
    public class ManchTakNetworkMatchState : NetworkBehaviour
    {
        private const int MaxPlayers = 4;
        private const int MinPlayers = 2;
        private const int PiecesPerPlayer = 4;

        public NetworkVariable<int> ActivePlayerIndex =
            new NetworkVariable<int>(0);

        public NetworkVariable<int> LastDiceValue =
            new NetworkVariable<int>(0);

        public NetworkVariable<int> ConsecutiveSixes =
            new NetworkVariable<int>(0);

        public NetworkVariable<bool> MatchStarted =
            new NetworkVariable<bool>(false);

        public NetworkVariable<bool> MatchFinished =
            new NetworkVariable<bool>(false);

        public NetworkVariable<int> WinnerSeat =
            new NetworkVariable<int>(-1);

        private readonly NetworkList<NetworkPieceState> Pieces =
            new NetworkList<NetworkPieceState>();

        private readonly NetworkList<NetworkPlayerState> Players =
            new NetworkList<NetworkPlayerState>();

        private bool IsReady =>
            IsServer && MatchStarted.Value && !MatchFinished.Value;

        public override void OnNetworkSpawn()
        {
            if (!IsServer)
                return;

            InitializePlayers();
            InitializePieces();
        }

        private void InitializePlayers()
        {
            Players.Clear();

            int seat = 0;

            foreach (ulong clientId in NetworkManager.ConnectedClientsIds)
            {
                if (seat >= MaxPlayers)
                    break;

                Players.Add(new NetworkPlayerState
                {
                    ClientId = clientId,
                    SeatIndex = seat,
                    IsConnected = true
                });

                seat++;
            }

            MatchStarted.Value =
                Players.Count >= MinPlayers;
        }

        private void InitializePieces()
        {
            Pieces.Clear();

            for (int player = 0; player < Players.Count; player++)
            {
                for (int piece = 0; piece < PiecesPerPlayer; piece++)
                {
                    Pieces.Add(new NetworkPieceState
                    {
                        OwnerSeat = player,
                        PieceIndex = piece,
                        State = 0,
                        BoardPosition = -1,
                        HomePathPosition = -1
                    });
                }
            }
        }

        [Rpc(SendTo.Server)]
        public void RequestDiceRollRpc(RpcParams rpcParams = default)
        {
            if (!IsReady)
                return;

            ulong sender = rpcParams.Receive.SenderClientId;

            int seat = GetSeatByClient(sender);

            if (seat != ActivePlayerIndex.Value)
                return;

            if (LastDiceValue.Value != 0)
                return;

            int dice = UnityEngine.Random.Range(1, 7);

            int sixes = dice == 6
                ? ConsecutiveSixes.Value + 1
                : 0;

            LastDiceValue.Value = dice;
            ConsecutiveSixes.Value = sixes;

            if (sixes >= 3)
            {
                LastDiceValue.Value = 0;
                ConsecutiveSixes.Value = 0;
                AdvanceTurn();
            }
        }

        [Rpc(SendTo.Server)]
        public void RequestMovePieceRpc(
            int pieceIndex,
            RpcParams rpcParams = default)
        {
            if (!IsReady)
                return;

            ulong sender = rpcParams.Receive.SenderClientId;

            int seat = GetSeatByClient(sender);

            if (seat != ActivePlayerIndex.Value)
                return;

            if (LastDiceValue.Value < 1 ||
                LastDiceValue.Value > 6)
                return;

            if (pieceIndex < 0 || pieceIndex >= PiecesPerPlayer)
                return;

            int globalIndex =
                seat * PiecesPerPlayer + pieceIndex;

            if (globalIndex < 0 || globalIndex >= Pieces.Count)
                return;

            NetworkPieceState piece = Pieces[globalIndex];

            if (piece.State == 3)
                return;

            if (piece.State == 0)
            {
                if (LastDiceValue.Value != 6)
                    return;

                piece.State = 1;
                piece.BoardPosition = seat * 13;
                piece.HomePathPosition = -1;
            }
            else if (piece.State == 1)
            {
                piece.BoardPosition =
                    (piece.BoardPosition + LastDiceValue.Value) % 52;
            }
            else if (piece.State == 2)
            {
                int next =
                    piece.HomePathPosition + LastDiceValue.Value;

                if (next > 6)
                    return;

                if (next == 6)
                {
                    piece.State = 3;
                    piece.HomePathPosition = -1;
                    piece.BoardPosition = -1;
                }
                else
                {
                    piece.HomePathPosition = next;
                }
            }

            Pieces[globalIndex] = piece;

            bool extraTurn = LastDiceValue.Value == 6 &&
                             ConsecutiveSixes.Value < 3;

            LastDiceValue.Value = 0;

            if (!extraTurn)
                AdvanceTurn();
        }

        private void AdvanceTurn()
        {
            if (Players.Count < MinPlayers)
                return;

            ActivePlayerIndex.Value =
                (ActivePlayerIndex.Value + 1) % Players.Count;

            LastDiceValue.Value = 0;
        }

        private int GetSeatByClient(ulong clientId)
        {
            for (int i = 0; i < Players.Count; i++)
            {
                if (Players[i].ClientId == clientId &&
                    Players[i].IsConnected)
                    return Players[i].SeatIndex;
            }

            return -1;
        }

        public void SetMatchWinner(int seat)
        {
            if (!IsServer || MatchFinished.Value)
                return;

            if (seat < 0 || seat >= Players.Count)
                return;

            WinnerSeat.Value = seat;
            MatchFinished.Value = true;
        }
    }

    public struct NetworkPieceState : INetworkSerializable,
        IEquatable<NetworkPieceState>
    {
        public int OwnerSeat;
        public int PieceIndex;
        public int State;
        public int BoardPosition;
        public int HomePathPosition;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer)
            where T : IReaderWriter
        {
            serializer.SerializeValue(ref OwnerSeat);
            serializer.SerializeValue(ref PieceIndex);
            serializer.SerializeValue(ref State);
            serializer.SerializeValue(ref BoardPosition);
            serializer.SerializeValue(ref HomePathPosition);
        }

        public bool Equals(NetworkPieceState other)
        {
            return OwnerSeat == other.OwnerSeat &&
                   PieceIndex == other.PieceIndex &&
                   State == other.State &&
                   BoardPosition == other.BoardPosition &&
                   HomePathPosition == other.HomePathPosition;
        }
    }

    public struct NetworkPlayerState : INetworkSerializable,
        IEquatable<NetworkPlayerState>
    {
        public ulong ClientId;
        public int SeatIndex;
        public bool IsConnected;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer)
            where T : IReaderWriter
        {
            serializer.SerializeValue(ref ClientId);
            serializer.SerializeValue(ref SeatIndex);
            serializer.SerializeValue(ref IsConnected);
        }

        public bool Equals(NetworkPlayerState other)
        {
            return ClientId == other.ClientId &&
                   SeatIndex == other.SeatIndex &&
                   IsConnected == other.IsConnected;
        }
    }
}
