
using System;

namespace ManchTak
{
    public enum ChatMessageType
    {
        Text,
        Sticker,
        System,
        GameQuickMessage,
        Gift
    }

    public enum ChatMessageStatus
    {
        Pending,
        Sent,
        Delivered,
        Read,
        Failed
    }

    [Serializable]
    public class ChatMessage
    {
        public string MessageId;
        public string ConversationId;
        public string SenderId;
        public string ReceiverId;

        public ChatMessageType Type;
        public string Content;
        public string StickerId;

        public long CreatedAt;
        public ChatMessageStatus Status;

        public bool IsPrivate;
        public bool IsDeleted;

        public ChatMessage()
        {
            MessageId = Guid.NewGuid().ToString("N");
            CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            Type = ChatMessageType.Text;
            Status = ChatMessageStatus.Pending;
            IsPrivate = true;
            IsDeleted = false;
        }

        public bool IsValid()
        {
            if (string.IsNullOrWhiteSpace(SenderId))
                return false;

            if (string.IsNullOrWhiteSpace(Content) &&
                string.IsNullOrWhiteSpace(StickerId))
                return false;

            return true;
        }
    }
}
