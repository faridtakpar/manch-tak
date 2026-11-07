using System;

namespace ManchTak
{
    public enum PlayerAccountStatus
    {
        Active,
        TemporarilyBanned,
        PermanentlyBanned
    }

    [Serializable]
    public class PlayerAccountIdentity
    {
        public string PlayerId;
        public string PublicId;

        public long CreatedAt;
        public long LastLoginAt;

        public PlayerAccountStatus Status;

        public PlayerAccountIdentity()
        {
            PlayerId = string.Empty;
            PublicId = string.Empty;

            CreatedAt = 0;
            LastLoginAt = 0;

            Status = PlayerAccountStatus.Active;
        }

        public bool IsValid()
        {
            return
                !string.IsNullOrWhiteSpace(PlayerId) &&
                !string.IsNullOrWhiteSpace(PublicId) &&
                PublicId.Length >= 5 &&
                PublicId.Length <= 12 &&
                Enum.IsDefined(
                    typeof(PlayerAccountStatus),
                    Status
                );
        }
    }

    [Serializable]
    public class AccountLoginRequest
    {
        public string PublicId;

        // کد محرمانه بازیابی حساب
        public string RecoveryCode;
    }

    [Serializable]
    public class AccountLoginResult
    {
        public bool Success;
        public string PlayerId;
        public string SessionToken;
        public string ErrorMessage;

        public AccountLoginResult()
        {
            Success = false;
            PlayerId = string.Empty;
            SessionToken = string.Empty;
            ErrorMessage = string.Empty;
        }
    }

    [Serializable]
    public class PlayerAccountSession
    {
        public string PlayerId;
        public string SessionToken;

        public long CreatedAt;
        public long ExpiresAt;

        public bool IsValid()
        {
            return
                !string.IsNullOrWhiteSpace(PlayerId) &&
                !string.IsNullOrWhiteSpace(SessionToken) &&
                ExpiresAt > CreatedAt;
        }
    }
}
