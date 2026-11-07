using System;

namespace ManchTak
{
    public enum DiceVisualType
    {
        Classic,
        Golden,
        Diamond,
        Fire,
        Ice,
        Lightning,
        Football,
        Galaxy,
        Royal,
        Neon,
        Crystal,
        Shadow
    }

    [Serializable]
    public class DiceVisualData
    {
        public DiceVisualType DiceId;
        public string DisplayName;
        public string Description;
        public string ImagePath;
        public string PreviewImagePath;
        public bool IsUnlocked;
        public int UnlockLevel;

        public DiceVisualData(
            DiceVisualType diceId,
            string displayName,
            string description,
            string imagePath,
            string previewImagePath,
            bool isUnlocked,
            int unlockLevel)
        {
            DiceId = diceId;
            DisplayName = displayName;
            Description = description;
            ImagePath = imagePath;
            PreviewImagePath = previewImagePath;
            IsUnlocked = isUnlocked;
            UnlockLevel = unlockLevel;
        }
    }
}
