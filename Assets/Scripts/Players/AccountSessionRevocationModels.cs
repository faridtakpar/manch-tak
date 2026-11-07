
using System;

namespace ManchTak
{
    public enum SessionRevocationReason
    {
        UserLogout,
        AdminAction,
        SecurityIncident,
        SessionExpired,
        AccountBanned,
        PasswordOrCredentialChanged
    }

    [Serializable]
    public class AccountSessionRevocation
    {
        public string RevocationId;
        public string PlayerId;
        public string SessionTokenHash;
        public SessionRevocationReason Reason;
        public string Description;
        public long RevokedAt;
        public string RevokedBy;

        public AccountSessionRevocation()
        {
            RevocationId = Guid.NewGuid().ToString("N");
            PlayerId = string.Empty;
            SessionTokenHash = string.Empty;
            Reason = SessionRevocationReason.UserLogout;
            Description = string.Empty;
            RevokedAt = 0;
            RevokedBy = string.Empty;
        }

        public bool IsValid()
        {
            return
                !string.IsNullOrWhiteSpace(RevocationId) &&
                !string.IsNullOrWhiteSpace(PlayerId) &&
                !string.IsNullOrWhiteSpace(SessionTokenHash) &&
                Enum.IsDefined(
                    typeof(SessionRevocationReason),
                    Reason
                ) &&
                Description != null &&
                Description.Length <= 500 &&
                RevokedAt > 0;
        }
    }

    public static class AccountSessionRevocationModels
    {
        public const int MaximumDescriptionLength = 500;

        public static AccountSessionRevocation Create(
            string playerId,
            string sessionTokenHash,
            SessionRevocationReason reason,
            string description,
            long serverTime,
            string revokedBy)
        {
            if (string.IsNullOrWhiteSpace(playerId) ||
                string.IsNullOrWhiteSpace(sessionTokenHash) ||
                !Enum.IsDefined(
                    typeof(SessionRevocationReason),
                    reason
                ) ||
                serverTime <= 0)
                return null;

            description = description ?? string.Empty;

            if (description.Length > MaximumDescriptionLength)
                return null;

            AccountSessionRevocation record =
                new AccountSessionRevocation();

            record.PlayerId = playerId;
            record.SessionTokenHash = sessionTokenHash;
            record.Reason = reason;
            record.Description = description;
            record.RevokedAt = serverTime;
            record.RevokedBy = revokedBy ?? string.Empty;

            return record.IsValid() ? record : null;
        }
    }
}
