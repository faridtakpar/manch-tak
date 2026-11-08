using System;

namespace ManchTak
{
    public enum OfflinePlayerType
    {
        Human,
        Bot
    }

    [Serializable]
    public class OfflinePlayerSlot
    {
        public string PlayerId;
        public string DisplayName;
        public PlayerColor Color;
        public OfflinePlayerType Type;
    }

    [Serializable]
    public class OfflineMatchSetup
    {
        public int PlayerCount;
        public OfflinePlayerSlot[] Players;

        public bool IsValid()
        {
            if (PlayerCount != 2 && PlayerCount != 4)
                return false;

            if (Players == null || Players.Length != PlayerCount)
                return false;

            int humans = 0;
            int bots = 0;

            foreach (OfflinePlayerSlot player in Players)
            {
                if (player == null ||
                    string.IsNullOrWhiteSpace(player.PlayerId) ||
                    string.IsNullOrWhiteSpace(player.DisplayName))
                    return false;

                if (player.Type == OfflinePlayerType.Human)
                    humans++;
                else if (player.Type == OfflinePlayerType.Bot)
                    bots++;
                else
                    return false;

                foreach (OfflinePlayerSlot other in Players)
                {
                    if (other == null || other == player)
                        continue;

                    if (string.Equals(
                        player.PlayerId,
                        other.PlayerId,
                        StringComparison.Ordinal))
                        return false;
                }
            }

            return humans == 1 && bots == PlayerCount - 1;
        }
    }

    public static class OfflineMatchFactory
    {
        public static OfflineMatchSetup Create(
            int playerCount,
            string humanPlayerId)
        {
            if ((playerCount != 2 && playerCount != 4) ||
                string.IsNullOrWhiteSpace(humanPlayerId))
                return null;

            PlayerColor[] colors =
            {
                PlayerColor.Red,
                PlayerColor.Blue,
                PlayerColor.Green,
                PlayerColor.Yellow
            };

            OfflineMatchSetup setup = new OfflineMatchSetup();

            setup.PlayerCount = playerCount;
            setup.Players = new OfflinePlayerSlot[playerCount];

            for (int i = 0; i < playerCount; i++)
            {
                bool isHuman = i == 0;

                setup.Players[i] = new OfflinePlayerSlot
                {
                    PlayerId = isHuman
                        ? humanPlayerId
                        : "BOT_" + i,

                    DisplayName = isHuman
                        ? "بازیکن"
                        : "ربات " + i,

                    Color = colors[i],

                    Type = isHuman
                        ? OfflinePlayerType.Human
                        : OfflinePlayerType.Bot
                };
            }

            return setup.IsValid() ? setup : null;
        }
    }
}
