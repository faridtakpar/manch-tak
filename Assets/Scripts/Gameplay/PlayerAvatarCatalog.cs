
using System.Collections.Generic;

namespace ManchTak
{
    public static class PlayerAvatarCatalog
    {
        private static readonly List<PlayerAvatarData> Avatars =
            new List<PlayerAvatarData>
            {
                // Male Avatars
                new PlayerAvatarData(
                    1, AvatarGender.Male,
                    "پسر کلاسیک",
                    "Avatars/Male/male_01",
                    true, 1
                ),

                new PlayerAvatarData(
                    2, AvatarGender.Male,
                    "پسر اسپرت",
                    "Avatars/Male/male_02",
                    true, 1
                ),

                new PlayerAvatarData(
                    3, AvatarGender.Male,
                    "پسر سلطنتی",
                    "Avatars/Male/male_03",
                    false, 5
                ),

                new PlayerAvatarData(
                    4, AvatarGender.Male,
                    "پسر حرفه‌ای",
                    "Avatars/Male/male_04",
                    false, 10
                ),

                new PlayerAvatarData(
                    5, AvatarGender.Male,
                    "پسر نئونی",
                    "Avatars/Male/male_05",
                    false, 15
                ),

                // Female Avatars
                new PlayerAvatarData(
                    6, AvatarGender.Female,
                    "دختر کلاسیک",
                    "Avatars/Female/female_01",
                    true, 1
                ),

                new PlayerAvatarData(
                    7, AvatarGender.Female,
                    "دختر شیک",
                    "Avatars/Female/female_02",
                    true, 1
                ),

                new PlayerAvatarData(
                    8, AvatarGender.Female,
                    "دختر سلطنتی",
                    "Avatars/Female/female_03",
                    false, 5
                ),

                new PlayerAvatarData(
                    9, AvatarGender.Female,
                    "دختر حرفه‌ای",
                    "Avatars/Female/female_04",
                    false, 10
                ),

                new PlayerAvatarData(
                    10, AvatarGender.Female,
                    "دختر نئونی",
                    "Avatars/Female/female_05",
                    false, 15
                )
            };

        public static IReadOnlyList<PlayerAvatarData> GetAll()
        {
            return Avatars.AsReadOnly();
        }

        public static PlayerAvatarData Find(int avatarId)
        {
            return Avatars.Find(
                item => item.AvatarId == avatarId
            );
        }

        public static List<PlayerAvatarData> GetByGender(
            AvatarGender gender)
        {
            return Avatars.FindAll(
                item => item.Gender == gender
            );
        }

        public static bool IsUnlocked(
            int avatarId,
            int playerLevel)
        {
            PlayerAvatarData avatar = Find(avatarId);

            if (avatar == null || playerLevel < 1)
                return false;

            return avatar.IsUnlocked ||
                   playerLevel >= avatar.UnlockLevel;
        }

        public static List<PlayerAvatarData> GetUnlocked(
            int playerLevel)
        {
            return Avatars.FindAll(
                item => IsUnlocked(item.AvatarId, playerLevel)
            );
        }
    }
}
