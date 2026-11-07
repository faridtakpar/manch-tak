
using System;
using System.Collections.Generic;

namespace ManchTak
{
    public static class PlayerBanHistoryQueryRules
    {
        // دریافت سوابق معتبر یک بازیکن
        public static List<PlayerBanHistoryRecord> GetPlayerHistory(
            IReadOnlyList<PlayerBanHistoryRecord> records,
            string playerId)
        {
            List<PlayerBanHistoryRecord> result =
                new List<PlayerBanHistoryRecord>();

            if (records == null ||
                string.IsNullOrWhiteSpace(playerId))
                return result;

            foreach (PlayerBanHistoryRecord record in records)
            {
                if (record == null ||
                    !record.IsValid())
                    continue;

                if (string.Equals(
                    record.PlayerId,
                    playerId,
                    StringComparison.Ordinal))
                {
                    result.Add(record);
                }
            }

            return result;
        }

        // دریافت سوابق بر اساس نوع اقدام
        public static List<PlayerBanHistoryRecord> GetByAction(
            IReadOnlyList<PlayerBanHistoryRecord> records,
            BanHistoryAction action)
        {
            List<PlayerBanHistoryRecord> result =
                new List<PlayerBanHistoryRecord>();

            if (records == null ||
                !Enum.IsDefined(typeof(BanHistoryAction), action))
                return result;

            foreach (PlayerBanHistoryRecord record in records)
            {
                if (record == null ||
                    !record.IsValid())
                    continue;

                if (record.Action == action)
                    result.Add(record);
            }

            return result;
        }

        // جست‌وجو با شناسه محرومیت
        public static PlayerBanHistoryRecord FindByBanId(
            IReadOnlyList<PlayerBanHistoryRecord> records,
            string banId)
        {
            if (records == null ||
                string.IsNullOrWhiteSpace(banId))
                return null;

            foreach (PlayerBanHistoryRecord record in records)
            {
                if (record == null ||
                    !record.IsValid())
                    continue;

                if (string.Equals(
                    record.RelatedBanId,
                    banId,
                    StringComparison.Ordinal))
                    return record;
            }

            return null;
        }

        // شمارش سوابق دائمی یک بازیکن
        public static int CountPermanentBans(
            IReadOnlyList<PlayerBanHistoryRecord> records,
            string playerId)
        {
            if (records == null ||
                string.IsNullOrWhiteSpace(playerId))
                return 0;

            int count = 0;

            foreach (PlayerBanHistoryRecord record in records)
            {
                if (record == null ||
                    !record.IsValid())
                    continue;

                if (record.Action == BanHistoryAction.PermanentBan &&
                    string.Equals(
                        record.PlayerId,
                        playerId,
                        StringComparison.Ordinal))
                {
                    count++;
                }
            }

            return count;
        }
    }
}
