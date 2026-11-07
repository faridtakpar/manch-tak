
using System;

namespace ManchTak
{
    public enum PlayerReportReason
    {
        Cheating,
        OffensiveBehavior,
        Harassment,
        InappropriateName,
        FakeAccount,
        Other
    }

    public enum PlayerReportStatus
    {
        Pending,
        UnderReview,
        Accepted,
        Rejected
    }

    [Serializable]
    public class PlayerReport
    {
        public string ReportId;

        public string ReporterPlayerId;
        public string ReportedPlayerId;

        public PlayerReportReason Reason;
        public PlayerReportStatus Status;

        public string Description;

        public long CreatedAt;
        public long ReviewedAt;

        public string ReviewedByAdmin;

        public PlayerReport()
        {
            ReportId = Guid.NewGuid().ToString("N");

            ReporterPlayerId = string.Empty;
            ReportedPlayerId = string.Empty;

            Reason = PlayerReportReason.Other;
            Status = PlayerReportStatus.Pending;

            Description = string.Empty;
            ReviewedByAdmin = string.Empty;

            CreatedAt = 0;
            ReviewedAt = 0;
        }

        public bool IsValid()
        {
            return
                !string.IsNullOrWhiteSpace(ReportId) &&
                !string.IsNullOrWhiteSpace(ReporterPlayerId) &&
                !string.IsNullOrWhiteSpace(ReportedPlayerId) &&
                ReporterPlayerId != ReportedPlayerId &&
                Enum.IsDefined(typeof(PlayerReportReason), Reason) &&
                Enum.IsDefined(typeof(PlayerReportStatus), Status) &&
                CreatedAt > 0 &&
                Description != null &&
                Description.Length <= 1000;
        }
    }

    public static class PlayerReportModels
    {
        public const int MaximumDescriptionLength = 1000;

        public static PlayerReport CreateReport(
            string reporterId,
            string reportedId,
            PlayerReportReason reason,
            string description,
            long serverTime)
        {
            if (string.IsNullOrWhiteSpace(reporterId) ||
                string.IsNullOrWhiteSpace(reportedId) ||
                reporterId == reportedId ||
                !Enum.IsDefined(typeof(PlayerReportReason), reason) ||
                serverTime <= 0)
                return null;

            description = description ?? string.Empty;

            if (description.Length > MaximumDescriptionLength)
                return null;

            PlayerReport report = new PlayerReport();

            report.ReporterPlayerId = reporterId;
            report.ReportedPlayerId = reportedId;
            report.Reason = reason;
            report.Description = description;
            report.CreatedAt = serverTime;

            return report.IsValid() ? report : null;
        }
    }
}
