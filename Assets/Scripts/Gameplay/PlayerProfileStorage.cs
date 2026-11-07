
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

            string json = JsonUtility.ToJson(profile);

            PlayerPrefs.SetString(
                SaveKeyPrefix + profile.PlayerId,
                json
            );

            PlayerPrefs.Save();

            return true;
        }

        public static PlayerProfileData Load(string playerId)
        {
            if (string.IsNullOrWhiteSpace(playerId))
                return null;

            string key = SaveKeyPrefix + playerId;

            if (!PlayerPrefs.HasKey(key))
                return null;

            string json = PlayerPrefs.GetString(key);

            if (string.IsNullOrEmpty(json))
                return null;

            try
            {
                PlayerProfileData profile =
                    JsonUtility.FromJson<PlayerProfileData>(json);

                if (profile == null ||
                    profile.PlayerId != playerId)
                {
                    return null;
                }

                return profile;
            }
            catch
            {
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
