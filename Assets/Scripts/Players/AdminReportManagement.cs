
using System;

namespace ManchTak
{
    public static class AdminReportManagement
    {
        // بررسی مجوز مدیر توسط سرور
        private static bool IsAuthorizedAdmin(
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

        // شروع بررسی گزارش
        public static bool StartReview(
            PlayerReport report,
            string adminId,
            bool serverVerifiedAdmin)
        {
            if (report == null ||
                !report.IsValid() ||
                !IsAuthorizedAdmin(adminId, serverVerifiedAdmin) ||
                report.Status != PlayerReportStatus.Pending)
                return false;

            report.Status = PlayerReportStatus.UnderReview;
            report.ReviewedByAdmin = adminId;

            return true;
        }

        // تأیید گزارش
        public static bool AcceptReport(
            PlayerReport report,
            string adminId,
            bool serverVerifiedAdmin,
            long serverTime)
        {
            if (!CanReview(
                report,
                adminId,
                serverVerifiedAdmin,
                serverTime))
                return false;

            report.Status = PlayerReportStatus.Accepted;
            report.ReviewedAt = serverTime;
            report.ReviewedByAdmin = adminId;

            return true;
        }

        // رد گزارش
        public static bool RejectReport(
            PlayerReport report,
            string adminId,
            bool serverVerifiedAdmin,
            long serverTime)
        {
            if (!CanReview(
                report,
                adminId,
                serverVerifiedAdmin,
                serverTime))
                return false;

            report.Status = PlayerReportStatus.Rejected;
            report.ReviewedAt = serverTime;
            report.ReviewedByAdmin = adminId;

            return true;
        }

        // بررسی شرایط نهایی کردن گزارش
        private static bool CanReview(
            PlayerReport report,
            string adminId,
            bool serverVerifiedAdmin,
            long serverTime)
        {
            return report != null &&
                   report.IsValid() &&
                   IsAuthorizedAdmin(adminId, serverVerifiedAdmin) &&
                   report.Status == PlayerReportStatus.UnderReview &&
                   !string.IsNullOrWhiteSpace(
                       report.ReviewedByAdmin
                   ) &&
                   string.Equals(
                       report.ReviewedByAdmin,
                       adminId,
                       StringComparison.OrdinalIgnoreCase
                   ) &&
                   serverTime >= report.CreatedAt;
        }

        // بررسی گزارش‌های در انتظار
        public static bool IsPending(PlayerReport report)
        {
            return report != null &&
                   report.IsValid() &&
                   report.Status == PlayerReportStatus.Pending;
        }

        // بررسی گزارش‌های تعیین تکلیف شده
        public static bool IsCompleted(PlayerReport report)
        {
            return report != null &&
                   report.IsValid() &&
                   (report.Status == PlayerReportStatus.Accepted ||
                    report.Status == PlayerReportStatus.Rejected);
        }
    }
}
