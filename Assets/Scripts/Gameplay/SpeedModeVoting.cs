using System;

namespace ManchTak
{
    public enum SpeedVoteState
    {
        WaitingForVotes,
        Accepted,
        Rejected,
        Invalid
    }

    [Serializable]
    public class SpeedVote
    {
        public string MatchId;
        public string[] PlayerIds;
        public bool[] Votes;
        public bool[] HasVoted;
        public SpeedVoteState State;

        public SpeedVote(string matchId, string[] playerIds)
        {
            MatchId = matchId;
            PlayerIds = playerIds;

            if (string.IsNullOrWhiteSpace(matchId) ||
                playerIds == null ||
                (playerIds.Length != 2 && playerIds.Length != 4))
            {
                State = SpeedVoteState.Invalid;
                return;
            }

            Votes = new bool[playerIds.Length];
            HasVoted = new bool[playerIds.Length];

            for (int i = 0; i < playerIds.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(playerIds[i]))
                {
                    State = SpeedVoteState.Invalid;
                    return;
                }

                for (int j = 0; j < i; j++)
                {
                    if (playerIds[i] == playerIds[j])
                    {
                        State = SpeedVoteState.Invalid;
                        return;
                    }
                }
            }

            State = SpeedVoteState.WaitingForVotes;
        }
    }

    public static class SpeedModeVoting
    {
        public static bool CastVote(
            SpeedVote vote,
            string playerId,
            bool accept)
        {
            if (vote == null ||
                vote.State != SpeedVoteState.WaitingForVotes ||
                string.IsNullOrWhiteSpace(playerId))
                return false;

            int index = Array.IndexOf(vote.PlayerIds, playerId);

            if (index < 0 || vote.HasVoted[index])
                return false;

            vote.HasVoted[index] = true;
            vote.Votes[index] = accept;

            if (!accept)
            {
                vote.State = SpeedVoteState.Rejected;
                return true;
            }

            for (int i = 0; i < vote.HasVoted.Length; i++)
            {
                if (!vote.HasVoted[i])
                    return true;
            }

            vote.State = SpeedVoteState.Accepted;
            return true;
        }

        public static MatchPlayMode GetSelectedMode(SpeedVote vote)
        {
            return vote != null &&
                   vote.State == SpeedVoteState.Accepted
                ? MatchPlayMode.Speed
                : MatchPlayMode.Classic;
        }

        public static bool IsVoteComplete(SpeedVote vote)
        {
            return vote != null &&
                (vote.State == SpeedVoteState.Accepted ||
                 vote.State == SpeedVoteState.Rejected ||
                 vote.State == SpeedVoteState.Invalid);
        }
    }
}
