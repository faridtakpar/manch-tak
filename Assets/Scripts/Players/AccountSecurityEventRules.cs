
using System;
using System.Collections.Generic;

namespace ManchTak
{
    public static class AccountSecurityEventRules
    {
        // اعتبارسنجی رویداد
        public static bool IsValid(AccountSecurityEvent securityEvent)
        {
            return securityEvent != null &&
                   securityEvent.IsValid();
        }

        // بررسی امکان ثبت رویداد با زمان سرور
        public static bool CanAppend(
            AccountSecurityEvent securityEvent,
            long serverTime)
        {
            return IsValid(securityEvent) &&
                   serverTime > 0 &&
                   securityEvent.CreatedAt <= serverTime;
        }

        // جلوگیری از شناسه رویداد تکراری
        public static bool HasDuplicateIds(
            IReadOnlyList<AccountSecurityEvent> events)
        {
            if (events == null)
                return false;

            HashSet<string> ids = new HashSet<string>(
                StringComparer.Ordinal
            );

            foreach (AccountSecurityEvent item in events)
            {
                if (!IsValid(item) ||
                    !ids.Add(item.EventId))
                    return true;
            }

            return false;
        }

        // بررسی ترتیب زمانی رویدادها
        public static bool IsChronological(
            IReadOnlyList<AccountSecurityEvent> events)
        {
            if (events == null)
                return false;

            long previousTime = 0;

            foreach (AccountSecurityEvent item in events)
            {
                if (!IsValid(item) ||
                    item.CreatedAt < previousTime)
                    return false;

                previousTime = item.CreatedAt;
            }

            return true;
        }

        // دریافت رویدادهای یک بازیکن
        public static List<AccountSecurityEvent> GetPlayerEvents(
            IReadOnlyList<AccountSecurityEvent> events,
            string playerId)
        {
            List<AccountSecurityEvent> result =
                new List<AccountSecurityEvent>();

            if (events == null ||
                string.IsNullOrWhiteSpace(playerId))
                return result;

            foreach (AccountSecurityEvent item in events)
            {
                if (IsValid(item) &&
                    string.Equals(
                        item.PlayerId,
                        playerId,
                        StringComparison.Ordinal))
                {
                    result.Add(item);
                }
            }

            return result;
        }

        // شمارش تلاش‌های ناموفق ورود
        public static int CountFailedLogins(
            IReadOnlyList<AccountSecurityEvent> events,
            string playerId)
        {
            if (events == null ||
                string.IsNullOrWhiteSpace(playerId))
                return 0;

            int count = 0;

            foreach (AccountSecurityEvent item in events)
            {
                if (IsValid(item) &&
                    item.EventType ==
                    AccountSecurityEventType.LoginFailed &&
                    string.Equals(
                        item.PlayerId,
                        playerId,
                        StringComparison.Ordinal))
                {
                    count++;
                }
            }

            return count;
        }

        // بررسی وجود رویداد امنیتی مشکوک
        public static bool HasSuspiciousActivity(
            IReadOnlyList<AccountSecurityEvent> events,
            string playerId)
        {
            if (events == null ||
                string.IsNullOrWhiteSpace(playerId))
                return false;

            foreach (AccountSecurityEvent item in events)
            {
                if (IsValid(item) &&
                    item.EventType ==
                    AccountSecurityEventType.SuspiciousActivity &&
                    string.Equals(
                        item.PlayerId,
                        playerId,
                        StringComparison.Ordinal))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
