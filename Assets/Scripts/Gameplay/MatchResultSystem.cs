
using System;

namespace ManchTak
{
    public enum MatchPlayMode
    {
        Speed,
        Classic
    }

    public enum MatchResultStatus
    {
        InProgress,
        Completed,
        Invalid
    }

    [Serializable]
    public class PlayerMatchProgress
    {
        public string PlayerId;
        public int FinishedPieces;
        public bool HasResigned;

        public PlayerMatchProgress(string playerId)
        {
            PlayerId = playerId;
            FinishedPieces = 0;
            HasResigned = false;
        }
    }

    [Serializable]
    public class MatchResult
    {
        public string MatchId;
        public string WinnerId;
        public string[] LoserIds;

        public MatchPlayMode PlayMode;
        public MatchResultStatus Status;

        public int WinnerXP;
        public int LoserXP;

        public long CompletedAt;

        public MatchResult()
        {
            Status = MatchResultStatus.InProgress;
            WinnerXP = 138;
            LoserXP = 50;
        }
    }

    public static class MatchResultSystem
    {
        public const int SpeedModeRequiredPieces = 2;
        public const int ClassicModeRequiredPieces = 4;

        public const int WinnerXPReward = 138;
        public const int LoserXPReward = 50;

        // تعداد مهره لازم برای برنده شدن
        public static int GetRequiredFinishedPieces(
            MatchPlayMode mode)
        {
            switch (mode)
            {
                case MatchPlayMode.Speed:
                    return SpeedModeRequiredPieces;

                case MatchPlayMode.Classic:
                    return ClassicModeRequiredPieces;

                default:
                    return -1;
            }
        }

        // بررسی رسیدن بازیکن به شرط پیروزی
        public static bool HasPlayerWon(
            PlayerMatchProgress player,
            MatchPlayMode mode)
        {
            if (player == null || player.HasResigned)
                return false;

            int requiredPieces =
                GetRequiredFinishedPieces(mode);

            if (requiredPieces < 0)
                return false;

            return player.FinishedPieces >= requiredPieces;
        }

        // بررسی پایان مسابقه
        public static MatchResult CheckMatchResult(
            string matchId,
            PlayerMatchProgress[] players,
            MatchPlayMode mode)
        {
            MatchResult result = new MatchResult();

            result.MatchId = matchId;
            result.PlayMode = mode;

            if (string.IsNullOrWhiteSpace(matchId) ||
                players == null ||
                players.Length < 2 ||
                players.Length > 4 ||
                GetRequiredFinishedPieces(mode) < 0)
            {
                result.Status = MatchResultStatus.Invalid;
                return result;
            }

            string winnerId = null;

            foreach (PlayerMatchProgress player in players)
            {
                if (player == null ||
                    string.IsNullOrWhiteSpace(player.PlayerId))
                {
                    result.Status = MatchResultStatus.Invalid;
                    return result;
                }

                if (player.FinishedPieces < 0 ||
                    player.FinishedPieces > 4)
                {
                    result.Status = MatchResultStatus.Invalid;
                    return result;
                }

                foreach (PlayerMatchProgress other in players)
                {
                    if (other == null ||
                        other == player)
                        continue;

                    if (other.PlayerId == player.PlayerId)
                    {
                        result.Status = MatchResultStatus.Invalid;
                        return result;
                    }
                }

                if (HasPlayerWon(player, mode))
                {
                    if (winnerId != null)
                    {
                        result.Status = MatchResultStatus.Invalid;
                        return result;
                    }

                    winnerId = player.PlayerId;
                }
            }

            if (winnerId == null)
            {
                result.Status = MatchResultStatus.InProgress;
                return result;
            }

            result.WinnerId = winnerId;

            int loserCount = players.Length - 1;
            result.LoserIds = new string[loserCount];

            int index = 0;

            foreach (PlayerMatchProgress player in players)
            {
                if (player.PlayerId != winnerId)
                {
                    result.LoserIds[index] = player.PlayerId;
                    index++;
                }
            }

            result.Status = MatchResultStatus.Completed;
            result.CompletedAt =
                DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            result.WinnerXP = WinnerXPReward;
            result.LoserXP = LoserXPReward;

            return result;
        }

        // دریافت XP بازیکن
        public static int GetPlayerXP(
            MatchResult result,
            string playerId)
        {
            if (result == null ||
                result.Status != MatchResultStatus.Completed ||
                string.IsNullOrWhiteSpace(playerId))
                return 0;

            if (result.WinnerId == playerId)
                return WinnerXPReward;

            if (result.LoserIds == null)
                return 0;

            foreach (string loserId in result.LoserIds)
            {
                if (loserId == playerId)
                    return LoserXPReward;
            }

            return 0;
        }
    }
}
