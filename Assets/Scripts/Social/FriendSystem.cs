
using System;

namespace ManchTak
{
    public enum FriendStatus
    {
        Offline,
        Online,
        InGame
    }

    public enum FriendRequestStatus
    {
        Pending,
        Accepted,
        Rejected
    }

    [Serializable]
    public class FriendData
    {
        public string UserId;
        public string DisplayName;
        public int Level;
        public FriendStatus Status;

        public FriendData(
            string userId,
            string displayName,
            int level,
            FriendStatus status)
        {
            UserId = userId;
            DisplayName = displayName;
            Level = level;
            Status = status;
        }
    }

    [Serializable]
    public class FriendRequest
    {
        public string RequestId;
        public string SenderId;
        public string ReceiverId;
        public FriendRequestStatus Status;

        public FriendRequest(
            string requestId,
            string senderId,
            string receiverId)
        {
            RequestId = requestId;
            SenderId = senderId;
            ReceiverId = receiverId;
            Status = FriendRequestStatus.Pending;
        }
    }

    public static class FriendSystem
    {
        // بررسی امکان ارسال درخواست دوستی
        public static bool CanSendRequest(
            string senderId,
            string receiverId)
        {
            if (string.IsNullOrWhiteSpace(senderId) ||
                string.IsNullOrWhiteSpace(receiverId))
                return false;

            return senderId != receiverId;
        }

        // پذیرش درخواست دوستی
        public static bool AcceptRequest(FriendRequest request)
        {
            if (request == null)
                return false;

            if (request.Status != FriendRequestStatus.Pending)
                return false;

            request.Status = FriendRequestStatus.Accepted;
            return true;
        }

        // رد درخواست دوستی
        public static bool RejectRequest(FriendRequest request)
        {
            if (request == null)
                return false;

            if (request.Status != FriendRequestStatus.Pending)
                return false;

            request.Status = FriendRequestStatus.Rejected;
            return true;
        }

        // بررسی وضعیت آنلاین دوست
        public static bool IsOnline(FriendData friend)
        {
            return friend != null &&
                   friend.Status == FriendStatus.Online;
        }

        // بررسی حضور دوست در مسابقه
        public static bool IsInGame(FriendData friend)
        {
            return friend != null &&
                   friend.Status == FriendStatus.InGame;
        }
    }
}
