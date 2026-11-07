
using System;

namespace ManchTak
{
    public enum AvatarGender
    {
        Male,
        Female
    }

    [Serializable]
    public class PlayerAvatarData
    {
        public int AvatarId;
        public AvatarGender Gender;
        public string DisplayName;
        public string ImagePath;
        public bool IsUnlocked;
        public int UnlockLevel;

        public PlayerAvatarData(
            int avatarId,
            AvatarGender gender,
            string displayName,
            string imagePath,
            bool isUnlocked,
            int unlockLevel)
        {
            AvatarId = avatarId;
            Gender = gender;
            DisplayName = displayName;
            ImagePath = imagePath;
            IsUnlocked = isUnlocked;
            UnlockLevel = unlockLevel;
        }
    }
}
