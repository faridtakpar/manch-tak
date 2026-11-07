
using System;
using System.Collections.Generic;

namespace ManchTak
{
    public static class PlayerBanHistoryRules
    {
        // بررسی اعتبار سابقه محرومیت
        public static bool IsValidRecord(
            PlayerBanHistoryRecord record)
        {
            return record != null && record.IsValid();
        }

        // بررسی مجوز مدیر برای ثبت سابقه
        public static bool CanCreateHistory(
            string adminId,
            bool serverVerifiedAdmin)
        {
            return serverVerifiedAdmin &&
                   string.Equals(
                       adminId,
                       "faridtak",
                       StringComparison.OrdinalIgnoreCase
                   );
        }

        // بررسی امکان ثبت سابقه جدید
        public static bool CanAppend(
            PlayerBanHistoryRecord record,
            string adminId,
            bool serverVerifiedAdmin,
            long serverTime)
        {
            if (!CanCreateHistory(adminId, serverVerifiedAdmin))
                return false;

            if (!IsValidRecord(record))
                return false;

            if (!string.Equals(
                record.AdminId,
                adminId,
                StringComparison.OrdinalIgnoreCase))
                return false;

            if (serverTime <= 0 || record.CreatedAt > serverTime)
                return false;

            if (record.LiftedAt > serverTime ||
                record.ExpiresAt > 0 &&
                record.ExpiresAt < record.CreatedAt)
                return false;

            return true;
        }

        // جلوگیری از شناسه تاریخچه تکراری
        public static bool HasDuplicateIds(
            IReadOnlyList<PlayerBanHistoryRecord> records)
        {
            if (records == null)
                return false;

            HashSet<string> ids = new HashSet<string>(
                StringComparer.Ordinal
            );

            foreach (PlayerBanHistoryRecord record in records)
            {
                if (!IsValidRecord(record))
                    return true;

                if (!ids.Add(record.HistoryId))
                    return true;
            }

            return false;
        }

        // بررسی ترتیب زمانی سوابق
        public static bool IsChronological(
            IReadOnlyList<PlayerBanHistoryRecord> records)
        {
            if (records == null)
                return false;

            long previousTime = 0;

            foreach (PlayerBanHistoryRecord record in records)
            {
                if (!IsValidRecord(record))
                    return false;

                if (record.CreatedAt < previousTime)
                    return false;

                previousTime = record.CreatedAt;
            }

            return true;
        }

        // شمارش سوابق یک بازیکن
        public static int CountPlayerRecords(
            IReadOnlyList<PlayerBanHistoryRecord> records,
            string playerId)
        {
            if (records == null ||
                string.IsNullOrWhiteSpace(playerId))
                return 0;

            int count = 0;

            foreach (PlayerBanHistoryRecord record in records)
            {
                if (IsValidRecord(record) &&
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
