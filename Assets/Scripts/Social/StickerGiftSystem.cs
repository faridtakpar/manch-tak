
using System;
using System.Collections.Generic;

namespace ManchTak
{
    public enum StickerCategory
    {
        Greeting,
        Emotion,
        Celebration,
        Game,
        Funny
    }

    public enum ProfileGiftType
    {
        Flower,
        Heart,
        Bomb,
        Egg,
        Trophy,
        Diamond
    }

    [Serializable]
    public class StickerData
    {
        public string StickerId;
        public string DisplayName;
        public StickerCategory Category;
        public bool IsPremium;

        public StickerData(
            string id,
            string name,
            StickerCategory category,
            bool premium = false)
        {
            StickerId = id;
            DisplayName = name;
            Category = category;
            IsPremium = premium;
        }
    }

    [Serializable]
    public class ProfileGift
    {
        public string GiftId;
        public string SenderId;
        public string ReceiverId;
        public ProfileGiftType Type;
        public long CreatedAt;
        public string Message;

        public ProfileGift()
        {
            GiftId = Guid.NewGuid().ToString("N");
            CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }
    }

    public static class StickerGiftSystem
    {
        private static readonly List<ProfileGift> Gifts =
            new List<ProfileGift>();

        // فهرست استیکرهای اولیه
        public static StickerData[] GetStickers()
        {
            return new StickerData[]
            {
                new StickerData("hello_01", "سلام", StickerCategory.Greeting),
                new StickerData("hello_02", "درود", StickerCategory.Greeting),

                new StickerData("happy_01", "خوشحال", StickerCategory.Emotion),
                new StickerData("sad_01", "ناراحت", StickerCategory.Emotion),
                new StickerData("love_01", "عاشقانه", StickerCategory.Emotion),

                new StickerData("win_01", "برنده شدم", StickerCategory.Celebration),
                new StickerData("clap_01", "آفرین", StickerCategory.Celebration),

                new StickerData("dice_01", "تاس خوب", StickerCategory.Game),
                new StickerData("game_01", "بزن بریم", StickerCategory.Game),

                new StickerData("funny_01", "خنده", StickerCategory.Funny),
                new StickerData("funny_02", "شوخی", StickerCategory.Funny),

                new StickerData("diamond_01", "الماسی", StickerCategory.Celebration, true),
                new StickerData("royal_01", "سلطنتی", StickerCategory.Celebration, true)
            };
        }

        // بررسی وجود استیکر
        public static bool IsValidSticker(string stickerId)
        {
            if (string.IsNullOrWhiteSpace(stickerId))
                return false;

            foreach (StickerData sticker in GetStickers())
            {
                if (sticker.StickerId == stickerId)
                    return true;
            }

            return false;
        }

        // ثبت هدیه پروفایل
        public static ProfileGift CreateGift(
            string senderId,
            string receiverId,
            ProfileGiftType type,
            string message = "")
        {
            if (string.IsNullOrWhiteSpace(senderId) ||
                string.IsNullOrWhiteSpace(receiverId))
                return null;

            if (senderId == receiverId)
                return null;

            ProfileGift gift = new ProfileGift();

            gift.SenderId = senderId;
            gift.ReceiverId = receiverId;
            gift.Type = type;
            gift.Message = message ?? string.Empty;

            Gifts.Add(gift);

            return gift;
        }

        // دریافت هدیه های یک بازیکن
        public static List<ProfileGift> GetPlayerGifts(string playerId)
        {
            List<ProfileGift> result =
                new List<ProfileGift>();

            if (string.IsNullOrWhiteSpace(playerId))
                return result;

            foreach (ProfileGift gift in Gifts)
            {
                if (gift.ReceiverId == playerId)
                    result.Add(gift);
            }

            return result;
        }

        // تعداد هدیه های دریافتی
        public static int GetGiftCount(string playerId)
        {
            return GetPlayerGifts(playerId).Count;
        }
    }
}
