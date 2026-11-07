
using System;

namespace ManchTak
{
    public static class AccountSessionRules
    {
        // حداقل اعتبار نشست: 15 دقیقه
        public const long MinimumSessionDuration = 15 * 60;

        // حداکثر اعتبار نشست: 30 روز
        public const long MaximumSessionDuration = 30 * 24 * 60 * 60;

        // بررسی اعتبار اطلاعات نشست
        public static bool IsValidSession(
            PlayerAccountSession session)
        {
            if (session == null ||
                !session.IsValid())
                return false;

            long duration = session.ExpiresAt - session.CreatedAt;

            return duration >= MinimumSessionDuration &&
                   duration <= MaximumSessionDuration;
        }

        // بررسی تطابق نشست با حساب بازیکن
        public static bool BelongsToPlayer(
            PlayerAccountSession session,
            string expectedPlayerId)
        {
            return IsValidSession(session) &&
                   !string.IsNullOrWhiteSpace(expectedPlayerId) &&
                   string.Equals(
                       session.PlayerId,
                       expectedPlayerId,
                       StringComparison.Ordinal
                   );
        }

        // بررسی فعال بودن نشست با زمان سرور
        public static bool IsSessionActive(
            PlayerAccountSession session,
            string expectedPlayerId,
            long serverTime)
        {
            return serverTime > 0 &&
                   BelongsToPlayer(session, expectedPlayerId) &&
                   serverTime >= session.CreatedAt &&
                   serverTime < session.ExpiresAt;
        }

        // بررسی اعتبار نشست مدیر
        public static bool IsAdminSessionActive(
            PlayerAccountSession session,
            string expectedAdminPlayerId,
            bool serverVerifiedAdmin,
            long serverTime)
        {
            return serverVerifiedAdmin &&
                   IsSessionActive(
                       session,
                       expectedAdminPlayerId,
                       serverTime
                   );
        }

        // بررسی نیاز به تمدید نشست
        public static bool ShouldRefreshSession(
            PlayerAccountSession session,
            string expectedPlayerId,
            long serverTime,
            long refreshWindowSeconds)
        {
            if (!IsSessionActive(
                session,
                expectedPlayerId,
                serverTime))
                return false;

            if (refreshWindowSeconds <= 0)
                return false;

            return session.ExpiresAt - serverTime
                   <= refreshWindowSeconds;
        }

        // بررسی اعتبار زمان‌های نشست جدید
        public static bool CanIssueSession(
            long createdAt,
            long expiresAt)
        {
            if (createdAt <= 0 || expiresAt <= createdAt)
                return false;

            long duration = expiresAt - createdAt;

            return duration >= MinimumSessionDuration &&
                   duration <= MaximumSessionDuration;
        }
    }
}
