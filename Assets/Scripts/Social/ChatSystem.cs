
using System;
using System.Collections.Generic;

namespace ManchTak
{
    public static class ChatSystem
    {
        private const int MaxMessageLength = 500;

        private static readonly List<ChatMessage> Messages =
            new List<ChatMessage>();

        // ایجاد پیام متنی
        public static ChatMessage CreateTextMessage(
            string conversationId,
            string senderId,
            string receiverId,
            string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                return null;

            content = content.Trim();

            if (content.Length > MaxMessageLength)
                return null;

            ChatMessage message = new ChatMessage();

            message.ConversationId = conversationId;
            message.SenderId = senderId;
            message.ReceiverId = receiverId;
            message.Type = ChatMessageType.Text;
            message.Content = content;

            if (!message.IsValid())
                return null;

            Messages.Add(message);
            return message;
        }

        // ایجاد پیام استیکر
        public static ChatMessage CreateStickerMessage(
            string conversationId,
            string senderId,
            string receiverId,
            string stickerId)
        {
            if (string.IsNullOrWhiteSpace(stickerId))
                return null;

            ChatMessage message = new ChatMessage();

            message.ConversationId = conversationId;
            message.SenderId = senderId;
            message.ReceiverId = receiverId;
            message.Type = ChatMessageType.Sticker;
            message.StickerId = stickerId;

            if (!message.IsValid())
                return null;

            Messages.Add(message);
            return message;
        }

        // ایجاد پیام آماده داخل بازی
        public static ChatMessage CreateQuickMessage(
            string conversationId,
            string senderId,
            string receiverId,
            string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return null;

            text = text.Trim();

            if (text.Length > MaxMessageLength)
                return null;

            ChatMessage message = new ChatMessage();

            message.ConversationId = conversationId;
            message.SenderId = senderId;
            message.ReceiverId = receiverId;
            message.Type = ChatMessageType.GameQuickMessage;
            message.Content = text;

            if (!message.IsValid())
                return null;

            Messages.Add(message);
            return message;
        }

        // تغییر وضعیت پیام
        public static bool UpdateMessageStatus(
            string messageId,
            ChatMessageStatus status)
        {
            ChatMessage message = FindMessage(messageId);

            if (message == null || message.IsDeleted)
                return false;

            message.Status = status;
            return true;
        }

        // علامت گذاری پیام به عنوان خوانده شده
        public static bool MarkAsRead(string messageId)
        {
            return UpdateMessageStatus(
                messageId,
                ChatMessageStatus.Read
            );
        }

        // حذف پیام
        public static bool DeleteMessage(string messageId)
        {
            ChatMessage message = FindMessage(messageId);

            if (message == null)
                return false;

            message.IsDeleted = true;
            return true;
        }

        // جستجوی پیام
        public static ChatMessage FindMessage(string messageId)
        {
            if (string.IsNullOrWhiteSpace(messageId))
                return null;

            foreach (ChatMessage message in Messages)
            {
                if (message.MessageId == messageId)
                    return message;
            }

            return null;
        }

        // دریافت پیام های یک گفت و گو
        public static List<ChatMessage> GetConversationMessages(
            string conversationId)
        {
            List<ChatMessage> result =
                new List<ChatMessage>();

            if (string.IsNullOrWhiteSpace(conversationId))
                return result;

            foreach (ChatMessage message in Messages)
            {
                if (message.ConversationId == conversationId &&
                    !message.IsDeleted)
                {
                    result.Add(message);
                }
            }

            return result;
        }

        // پیام های آماده بازی
        public static string[] GetQuickMessages()
        {
            return new string[]
            {
                "سلام خوبی؟",
                "بازی خوبی باشه!",
                "موفق باشی!",
                "نوش جان!",
                "آفرین!",
                "چه تاسی!",
                "یکم صبر کن",
                "خسته نباشی",
                "بازی rematch؟",
                "خداحافظ"
            };
        }

        // پاکسازی پیام های حذف شده
        public static void ClearDeletedMessages()
        {
            Messages.RemoveAll(
                message => message == null || message.IsDeleted
            );
        }

        // تعداد پیام های یک گفت و گو
        public static int GetMessageCount(string conversationId)
        {
            return GetConversationMessages(conversationId).Count;
        }
    }
}
