
using UnityEngine;

namespace ManchTak
{
    public static class BoardThemeStorage
    {
        private const string SaveKey = "ManchTak_SelectedBoard";

        public static void Save(MainMenuBoardTheme theme)
        {
            PlayerPrefs.SetInt(SaveKey, (int)theme);
            PlayerPrefs.Save();
        }

        public static MainMenuBoardTheme Load()
        {
            if (!PlayerPrefs.HasKey(SaveKey))
                return MainMenuBoardTheme.Classic;

            int savedValue = PlayerPrefs.GetInt(
                SaveKey,
                (int)MainMenuBoardTheme.Classic
            );

            if (!System.Enum.IsDefined(
                typeof(MainMenuBoardTheme),
                savedValue))
            {
                return MainMenuBoardTheme.Classic;
            }

            return (MainMenuBoardTheme)savedValue;
        }

        public static void DeleteSave()
        {
            PlayerPrefs.DeleteKey(SaveKey);
            PlayerPrefs.Save();
        }
    }
}
