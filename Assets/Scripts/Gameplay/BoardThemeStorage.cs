
using UnityEngine;
using System;

namespace ManchTak
{
    public static class BoardThemeStorage
    {
        private const string SaveKeyPrefix =
            "ManchTak_SelectedBoard_";

        private static string GetSaveKey(string playerId)
        {
            if (string.IsNullOrWhiteSpace(playerId))
            {
                throw new ArgumentException(
                    "شناسه بازیکن معتبر نیست.",
                    nameof(playerId)
                );
            }

            return SaveKeyPrefix + playerId.Trim();
        }

        public static void Save(
            string playerId,
            MainMenuBoardTheme theme)
        {
            if (!Enum.IsDefined(typeof(MainMenuBoardTheme), theme))
                return;

            PlayerPrefs.SetInt(
                GetSaveKey(playerId),
                (int)theme
            );

            PlayerPrefs.Save();
        }

        public static MainMenuBoardTheme Load(string playerId)
        {
            string key = GetSaveKey(playerId);

            if (!PlayerPrefs.HasKey(key))
                return MainMenuBoardTheme.Classic;

            int savedValue = PlayerPrefs.GetInt(
                key,
                (int)MainMenuBoardTheme.Classic
            );

            if (!Enum.IsDefined(
                typeof(MainMenuBoardTheme),
                savedValue))
            {
                return MainMenuBoardTheme.Classic;
            }

            return (MainMenuBoardTheme)savedValue;
        }

        public static void DeleteSave(string playerId)
        {
            PlayerPrefs.DeleteKey(GetSaveKey(playerId));
            PlayerPrefs.Save();
        }
    }
}
