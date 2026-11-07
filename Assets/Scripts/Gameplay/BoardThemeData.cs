using System;

namespace ManchTak
{
    [Serializable]
    public class BoardThemeData
    {
        public MainMenuBoardTheme ThemeId;
        public string DisplayName;
        public string Description;
        public string BoardImagePath;
        public string PreviewImagePath;
        public bool IsUnlocked;
        public int UnlockLevel;

        public BoardThemeData(
            MainMenuBoardTheme themeId,
            string displayName,
            string description,
            string boardImagePath,
            string previewImagePath,
            bool isUnlocked,
            int unlockLevel)
        {
            ThemeId = themeId;
            DisplayName = displayName;
            Description = description;
            BoardImagePath = boardImagePath;
            PreviewImagePath = previewImagePath;
            IsUnlocked = isUnlocked;
            UnlockLevel = unlockLevel;
        }
    }
}
