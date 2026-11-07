
using System;

namespace ManchTak
{
    public enum AdminActionType
    {
        ReportAccepted,
        ReportRejected,
        PlayerTemporarilyBanned,
        PlayerPermanentlyBanned,
        PlayerBanLifted,
        WalletAdjustment,
        AccountReviewed
    }

    [Serializable]
    public class AdminAuditLog
    {
        public string LogId;
        public string AdminId;
        public string TargetPlayerId;
        public AdminActionType Action;
        public string Description;
        public string RelatedRecordId;
        public long CreatedAt;

        public AdminAuditLog()
        {
            LogId = Guid.NewGuid().ToString("N");
            AdminId = string.Empty;
            TargetPlayerId = string.Empty;
            Action = AdminActionType.AccountReviewed;
            Description = string.Empty;
            RelatedRecordId = string.Empty;
            CreatedAt = 0;
        }

        public bool IsValid()
        {
            return
                !string.IsNullOrWhiteSpace(LogId) &&
                !string.IsNullOrWhiteSpace(AdminId) &&
                !string.IsNullOrWhiteSpace(TargetPlayerId) &&
                Enum.IsDefined(typeof(AdminActionType), Action) &&
                !string.IsNullOrWhiteSpace(Description) &&
                Description.Length <= 1000 &&
                CreatedAt > 0;
        }
    }

    public static class AdminAuditLogModels
    {
        public const int MaximumDescriptionLength = 1000;

        public static AdminAuditLog Create(
            string adminId,
            string targetPlayerId,
            AdminActionType action,
            string description,
            string relatedRecordId,
            long serverTime,
            bool serverVerifiedAdmin)
        {
            if (!serverVerifiedAdmin ||
                !string.Equals(
                    adminId,
                    "faridtak",
                    StringComparison.OrdinalIgnoreCase) ||
                string.IsNullOrWhiteSpace(targetPlayerId) ||
                !Enum.IsDefined(typeof(AdminActionType), action) ||
                serverTime <= 0)
            {
                return null;
            }

            description = description ?? string.Empty;

            if (description.Length > MaximumDescriptionLength)
                return null;

            AdminAuditLog log = new AdminAuditLog();

            log.AdminId = adminId;
            log.TargetPlayerId = targetPlayerId;
            log.Action = action;
            log.Description = description;
            log.RelatedRecordId = relatedRecordId ?? string.Empty;
            log.CreatedAt = serverTime;

            return log.IsValid() ? log : null;
        }
    }
}
