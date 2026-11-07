
using System;
using System.Collections.Generic;

namespace ManchTak.UI
{
    [Serializable]
    public class PlayerAvatarData
    {
        public string AvatarId;
        public PlayerAvatarType AvatarType;
        public string DisplayName;
        public string ImagePath;
    }

    public static class PlayerAvatarCatalog
    {
        private static readonly List<PlayerAvatarData> Avatars =
            new List<PlayerAvatarData>
            {
                new PlayerAvatarData
                {
                    AvatarId = "male_01",
                    AvatarType = PlayerAvatarType.Male,
                    DisplayName = "پسرانه ۱",
                    ImagePath = "Avatars/Male/male_01"
                },
                new PlayerAvatarData
                {
                    AvatarId = "male_02",
                    AvatarType = PlayerAvatarType.Male,
                    DisplayName = "پسرانه ۲",
                    ImagePath = "Avatars/Male/male_02"
                },
                new PlayerAvatarData
                {
                    AvatarId = "female_01",
                    AvatarType = PlayerAvatarType.Female,
                    DisplayName = "دخترانه ۱",
                    ImagePath = "Avatars/Female/female_01"
                },
                new PlayerAvatarData
                {
                    AvatarId = "female_02",
                    AvatarType = PlayerAvatarType.Female,
                    DisplayName = "دخترانه ۲",
                    ImagePath = "Avatars/Female/female_02"
                }
            };

        public static IReadOnlyList<PlayerAvatarData> GetAll()
        {
            return Avatars.AsReadOnly();
        }

        public static IReadOnlyList<PlayerAvatarData> GetByType(
            PlayerAvatarType avatarType)
        {
            return Avatars.FindAll(
                avatar => avatar.AvatarType == avatarType
            ).AsReadOnly();
        }

        public static PlayerAvatarData FindById(string avatarId)
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
