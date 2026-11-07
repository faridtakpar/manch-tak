
using System;
using UnityEngine;

namespace ManchTak
{
    public static class PlayerProfileStorage
    {
        private const string SaveKeyPrefix =
            "ManchTak_PlayerProfile_";

        public static bool Save(PlayerProfileData profile)
        {
            if (profile == null ||
                string.IsNullOrWhiteSpace(profile.PlayerId))
            {
                return false;
            }

            try
            {
                if (profile.ProcessedMatchIds == null)
                {
                    profile.ProcessedMatchIds =
                        new string[0];
                }

                string json = JsonUtility.ToJson(profile);

                if (string.IsNullOrEmpty(json))
                    return false;

                string key = SaveKeyPrefix + profile.PlayerId;

                PlayerPrefs.SetString(key, json);
                PlayerPrefs.Save();

                return true;
            }
            catch (Exception exception)
            {
                Debug.LogError(
                    "خطا در ذخیره پروفایل: " +
                    exception.Message
                );

                return false;
            }
        }

        public static PlayerProfileData Load(string playerId)
        {
            if (string.IsNullOrWhiteSpace(playerId))
                return null;

            try
            {
                string key = SaveKeyPrefix + playerId;

                if (!PlayerPrefs.HasKey(key))
                    return null;

                string json = PlayerPrefs.GetString(key);

                if (string.IsNullOrEmpty(json))
                    return null;

                PlayerProfileData profile =
                    JsonUtility.FromJson<PlayerProfileData>(json);

                if (profile == null ||
                    !string.Equals(
                        profile.PlayerId,
                        playerId,
                        StringComparison.Ordinal))
                {
                    return null;
                }

                if (profile.ProcessedMatchIds == null)
                {
                    profile.ProcessedMatchIds =
                        new string[0];
                }

                return profile;
            }
            catch (Exception exception)
            {
                Debug.LogError(
                    "خطا در بارگذاری پروفایل: " +
                    exception.Message
                );

                return null;
            }
        }

        public static bool HasSavedProfile(string playerId)
        {
            if (string.IsNullOrWhiteSpace(playerId))
                return false;

            return PlayerPrefs.HasKey(
                SaveKeyPrefix + playerId
            );
        }

        public static void Delete(string playerId)
        {
            if (string.IsNullOrWhiteSpace(playerId))
                return;

            PlayerPrefs.DeleteKey(
                SaveKeyPrefix + playerId
            );

            PlayerPrefs.Save();
        }
    }
}
