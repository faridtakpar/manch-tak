
using System;
using System.Collections.Generic;

namespace ManchTak
{
    public static class AdminAuditLogRules
    {
        // اعتبارسنجی مجوز مدیر
        public static bool CanCreateLog(
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

        // اعتبارسنجی اطلاعات تاریخچه
        public static bool IsValidLog(AdminAuditLog log)
        {
            return log != null && log.IsValid();
        }

        // بررسی امکان ثبت تاریخچه جدید
        public static bool CanAppend(
            AdminAuditLog log,
            string adminId,
            bool serverVerifiedAdmin,
            long serverTime)
        {
            if (!CanCreateLog(adminId, serverVerifiedAdmin))
                return false;

            if (!IsValidLog(log))
                return false;

            if (!string.Equals(
                log.AdminId,
                adminId,
                StringComparison.OrdinalIgnoreCase))
                return false;

            if (serverTime <= 0 || log.CreatedAt > serverTime)
                return false;

            return true;
        }

        // بررسی شناسه‌های تکراری در مجموعه دریافتی
        public static bool HasDuplicateLogIds(
            IReadOnlyList<AdminAuditLog> logs)
        {
            if (logs == null)
                return false;

            HashSet<string> ids = new HashSet<string>(
                StringComparer.Ordinal
            );

            foreach (AdminAuditLog log in logs)
            {
                if (log == null ||
                    string.IsNullOrWhiteSpace(log.LogId))
                    return true;

                if (!ids.Add(log.LogId))
                    return true;
            }

            return false;
        }

        // بررسی ترتیب زمانی تاریخچه
        public static bool IsChronological(
            IReadOnlyList<AdminAuditLog> logs)
        {
            if (logs == null)
                return false;

            long previousTime = 0;

            foreach (AdminAuditLog log in logs)
            {
                if (!IsValidLog(log))
                    return false;

                if (log.CreatedAt < previousTime)
                    return false;

                previousTime = log.CreatedAt;
            }

            return true;
        }
    }
}
