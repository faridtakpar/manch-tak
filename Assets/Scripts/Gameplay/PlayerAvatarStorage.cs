
using UnityEngine;
using System;

namespace ManchTak
{
    public static class PlayerAvatarStorage
    {
        private const string SaveKeyPrefix =
            "ManchTak_SelectedAvatar_";

        private static string GetSaveKey(string playerId)
        {
            if (string.IsNullOrWhiteSpace(playerId))
                throw new ArgumentException(
                    "شناسه بازیکن معتبر نیست.",
                    nameof(playerId)
                );

            return SaveKeyPrefix + playerId.Trim();
        }

        public static void Save(string playerId, int avatarId)
        {
            if (PlayerAvatarCatalog.Find(avatarId) == null)
                return;

            PlayerPrefs.SetInt(
                GetSaveKey(playerId),
                avatarId
            );

            PlayerPrefs.Save();
        }

        public static int Load(string playerId)
        {
            string key = GetSaveKey(playerId);

            int avatarId = PlayerPrefs.GetInt(key, 1);

            if (PlayerAvatarCatalog.Find(avatarId) == null)
                return 1;

            return avatarId;
        }

        public static void DeleteSave(string playerId)
        {
            PlayerPrefs.DeleteKey(GetSaveKey(playerId));
            PlayerPrefs.Save();
        }
    }
}
