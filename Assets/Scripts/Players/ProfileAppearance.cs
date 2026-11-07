
using System;

namespace ManchTak
{
    // دسته بندی آواتارها
    public enum AvatarCategory
    {
        Male,
        Female,
        Neutral
    }

    // قاب پروفایل
    public enum ProfileFrame
    {
        Default,
        Silver,
        Gold,
        Diamond,
        Royal,
        Legendary
    }

    // نشان بازیکن
    public enum PlayerBadge
    {
        None,
        Beginner,
        Winner,
        Champion,
        Professional,
        Legendary
    }

    [Serializable]
    public class ProfileAppearance
    {
        public AvatarCategory Category;
        public string AvatarId;
        public ProfileFrame Frame;
        public PlayerBadge Badge;

        public ProfileAppearance()
        {
            Category = AvatarCategory.Neutral;
            AvatarId = "avatar_default";
            Frame = ProfileFrame.Default;
            Badge = PlayerBadge.None;
        }
    }

    public static class ProfileAppearanceCatalog
    {
        // آواتارهای اولیه دخترانه
        public static string[] GetFemaleAvatars()
        {
            return new string[]
            {
                "female_01",
                "female_02",
                "female_03",
                "female_04",
                "female_05",
                "female_06"
            };
        }

        // آواتارهای اولیه پسرانه
        public static string[] GetMaleAvatars()
        {
            return new string[]
            {
                "male_01",
                "male_02",
                "male_03",
                "male_04",
                "male_05",
                "male_06"
            };
        }

        // آواتارهای خنثی
        public static string[] GetNeutralAvatars()
        {
            return new string[]
            {
                "neutral_01",
                "neutral_02"
            };
        }

        // بررسی شناسه آواتار
        public static bool IsValidAvatar(
            AvatarCategory category,
            string avatarId)
        {
            if (string.IsNullOrWhiteSpace(avatarId))
                return false;

            string[] avatars;

            switch (category)
            {
                case AvatarCategory.Male:
                    avatars = GetMaleAvatars();
                    break;

                case AvatarCategory.Female:
                    avatars = GetFemaleAvatars();
                    break;

                default:
                    avatars = GetNeutralAvatars();
                    break;
            }

            foreach (string avatar in avatars)
            {
                if (avatar == avatarId)
                    return true;
            }

            return false;
        }
    }
}
