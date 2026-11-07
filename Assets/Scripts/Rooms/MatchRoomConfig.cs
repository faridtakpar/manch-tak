
using System;

namespace ManchTak
{
    [Serializable]
    public class MatchRoomData
    {
        public MatchField Field;
        public string RoomName;
        public int PlayerCount;
        public int EntryFee;
        public int WinnerPrize;

        public MatchRoomData(
            MatchField field,
            string roomName,
            int playerCount)
        {
            Field = field;
            RoomName = roomName;
            PlayerCount = playerCount;

            // استفاده از قوانین اقتصادی فایل قبلی
            EntryFee = MatchEconomyRules.GetEntryFee();
            WinnerPrize = MatchEconomyRules.GetWinnerPrize(field);
        }
    }

    public static class MatchRoomConfig
    {
        // ساخت فهرست اتاق های بازی
        public static MatchRoomData[] GetRooms()
        {
            return new MatchRoomData[]
            {
                new MatchRoomData(
                    MatchField.AzadiFourPlayer,
                    "میدان آزادی",
                    4
                ),

                new MatchRoomData(
                    MatchField.TajrishTwoPlayer,
                    "میدان تجریش",
                    2
                )
            };
        }

        // دریافت اطلاعات یک اتاق
        public static MatchRoomData GetRoom(MatchField field)
        {
            MatchRoomData[] rooms = GetRooms();

            foreach (MatchRoomData room in rooms)
            {
                if (room.Field == field)
                    return room;
            }

            return null;
        }

        // بررسی تعداد بازیکنان اتاق
        public static bool IsValidPlayerCount(
            MatchField field,
            int playerCount)
        {
            MatchRoomData room = GetRoom(field);

            return room != null &&
                   room.PlayerCount == playerCount;
        }

        // بررسی آماده بودن اتاق برای شروع
        public static bool CanStartMatch(
            MatchField field,
            int connectedPlayers)
        {
            MatchRoomData room = GetRoom(field);

            if (room == null)
                return false;

            return connectedPlayers == room.PlayerCount;
        }
    }
}
