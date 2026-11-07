using System;
using System.Collections.Generic;

namespace ManchTak
{
    [Serializable]
    public class ProfileAvatarData
    {
        public string AvatarId;
        public PlayerAvatarType AvatarType;
        public string DisplayName;
        public string ImagePath;
    }

    public static class ProfileAvatarCatalog
    {
        private static readonly List<ProfileAvatarData> Avatars =
            new List<ProfileAvatarData>
            {
                new ProfileAvatarData
                {
                    AvatarId = "male_01",
                    AvatarType = PlayerAvatarType.Male,
                    DisplayName = "پسرانه ۱",
                    ImagePath = "Avatars/Male/male_01"
                },
                new ProfileAvatarData
                {
                    AvatarId = "male_02",
                    AvatarType = PlayerAvatarType.Male,
                    DisplayName = "پسرانه ۲",
                    ImagePath = "Avatars/Male/male_02"
                },
                new ProfileAvatarData
                {
                    AvatarId = "female_01",
                    AvatarType = PlayerAvatarType.Female,
                    DisplayName = "دخترانه ۱",
                    ImagePath = "Avatars/Female/female_01"
                },
                new ProfileAvatarData
                {
                    AvatarId = "female_02",
                    AvatarType = PlayerAvatarType.Female,
                    DisplayName = "دخترانه ۲",
                    ImagePath = "Avatars/Female/female_02"
                }
            };

        public static IReadOnlyList<ProfileAvatarData> GetAll()
        {
            return Avatars.AsReadOnly();
        }

        public static IReadOnlyList<ProfileAvatarData> GetByType(
            PlayerAvatarType avatarType)
        {
            return Avatars.FindAll(
                avatar => avatar.AvatarType == avatarType
            ).AsReadOnly();
        }

        public static ProfileAvatarData FindById(string avatarId)
        {
            if (string.IsNullOrWhiteSpace(avatarId))
                return null;

            return Avatars.Find(
                avatar => avatar.AvatarId == avatarId
            );
        }

        public static bool Exists(string avatarId)
        {
            return FindById(avatarId) != null;
        }
    }
}
