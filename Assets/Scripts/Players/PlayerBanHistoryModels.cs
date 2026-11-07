
using System;

namespace ManchTak
{
    public enum BanHistoryAction
    {
        TemporaryBan,
        PermanentBan,
        BanExpired,
        BanLifted
    }

    [Serializable]
    public class PlayerBanHistoryRecord
    {
        public string HistoryId;
        public string PlayerId;
        public string AdminId;
        public BanHistoryAction Action;
        public string Reason;
        public string RelatedBanId;
        public long CreatedAt;
        public long ExpiresAt;
        public long LiftedAt;

        public PlayerBanHistoryRecord()
        {
            HistoryId = Guid.NewGuid().ToString("N");
            PlayerId = string.Empty;
            AdminId = string.Empty;
            Action = BanHistoryAction.TemporaryBan;
            Reason = string.Empty;
            RelatedBanId = string.Empty;
            CreatedAt = 0;
            ExpiresAt = 0;
            LiftedAt = 0;
        }

        public bool IsValid()
        {
            if (string.IsNullOrWhiteSpace(HistoryId) ||
                string.IsNullOrWhiteSpace(PlayerId) ||
                string.IsNullOrWhiteSpace(AdminId) ||
                string.IsNullOrWhiteSpace(RelatedBanId) ||
                !Enum.IsDefined(typeof(BanHistoryAction), Action) ||
                string.IsNullOrWhiteSpace(Reason) ||
                Reason.Length > 1000 ||
                CreatedAt <= 0)
                return false;

            if (Action == BanHistoryAction.TemporaryBan)
                return ExpiresAt > CreatedAt && LiftedAt == 0;

            if (Action == BanHistoryAction.PermanentBan)
                return ExpiresAt == 0 && LiftedAt == 0;

            if (Action == BanHistoryAction.BanExpired ||
                Action == BanHistoryAction.BanLifted)
                return LiftedAt >= CreatedAt;

            return false;
        }
    }

    public static class PlayerBanHistoryModels
    {
        public static PlayerBanHistoryRecord Create(
            string playerId,
            string adminId,
            BanHistoryAction action,
            string reason,
            string relatedBanId,
            long serverTime,
            long expiresAt,
            long liftedAt,
            bool serverVerifiedAdmin)
        {
            if (!serverVerifiedAdmin ||
                string.IsNullOrWhiteSpace(adminId) ||
                !string.Equals(
                    adminId,
                    "faridtak",
                    StringComparison.OrdinalIgnoreCase) ||
                string.IsNullOrWhiteSpace(playerId) ||
                string.IsNullOrWhiteSpace(relatedBanId) ||
                !Enum.IsDefined(typeof(BanHistoryAction), action) ||
                serverTime <= 0)
                return null;

            reason = reason ?? string.Empty;

            if (reason.Length > 1000)
                return null;

            PlayerBanHistoryRecord record =
                new PlayerBanHistoryRecord();

            record.PlayerId = playerId;
            record.AdminId = adminId;
            record.Action = action;
            record.Reason = reason;
            record.RelatedBanId = relatedBanId;
            record.CreatedAt = serverTime;
            record.ExpiresAt = expiresAt;
            record.LiftedAt = liftedAt;

            return record.IsValid() ? record : null;
        }
    }
}
