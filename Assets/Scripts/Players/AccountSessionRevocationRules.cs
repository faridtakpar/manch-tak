
using System;
using System.Collections.Generic;

namespace ManchTak
{
    public static class AccountSessionRevocationRules
    {
        // بررسی اعتبار رکورد لغو نشست
        public static bool IsValid(
            AccountSessionRevocation record)
        {
            return record != null && record.IsValid();
        }

        // بررسی تطابق نشست با رکورد لغو
        public static bool MatchesSession(
            AccountSessionRevocation record,
            string playerId,
            string sessionTokenHash)
        {
            return IsValid(record) &&
                   !string.IsNullOrWhiteSpace(playerId) &&
                   !string.IsNullOrWhiteSpace(sessionTokenHash) &&
                   string.Equals(
                       record.PlayerId,
                       playerId,
                       StringComparison.Ordinal
                   ) &&
                   string.Equals(
                       record.SessionTokenHash,
                       sessionTokenHash,
                       StringComparison.Ordinal
                   );
        }

        // بررسی اینکه نشست قبلاً لغو شده است یا خیر
        public static bool IsRevoked(
            IReadOnlyList<AccountSessionRevocation> records,
            string playerId,
            string sessionTokenHash)
        {
            if (records == null)
                return false;

            foreach (AccountSessionRevocation record in records)
            {
                if (MatchesSession(
                    record,
                    playerId,
                    sessionTokenHash))
                    return true;
            }

            return false;
        }

        // بررسی اجازه استفاده از نشست
        public static bool CanUseSession(
            PlayerAccountSession session,
            string playerId,
            string sessionTokenHash,
            IReadOnlyList<AccountSessionRevocation> records,
            long serverTime)
        {
            if (!AccountSessionRules.IsSessionActive(
                session,
                playerId,
                serverTime))
                return false;

            if (string.IsNullOrWhiteSpace(sessionTokenHash))
                return false;

            return !IsRevoked(
                records,
                playerId,
                sessionTokenHash
            );
        }

        // جلوگیری از ثبت شناسه لغو تکراری
        public static bool HasDuplicateRevocationIds(
            IReadOnlyList<AccountSessionRevocation> records)
        {
            if (records == null)
                return false;

            HashSet<string> ids = new HashSet<string>(
                StringComparer.Ordinal
            );

            foreach (AccountSessionRevocation record in records)
            {
                if (!IsValid(record) ||
                    !ids.Add(record.RevocationId))
                    return true;
            }

            return false;
        }

        // بررسی ترتیب زمانی سوابق لغو
        public static bool IsChronological(
            IReadOnlyList<AccountSessionRevocation> records)
        {
            if (records == null)
                return false;

            long previousTime = 0;

            foreach (AccountSessionRevocation record in records)
            {
                if (!IsValid(record) ||
                    record.RevokedAt < previousTime)
                    return false;

                previousTime = record.RevokedAt;
            }

            return true;
        }
    }
}
