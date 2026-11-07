
using UnityEngine;

namespace ManchTak
{
    public static class PlayerProgressStorage
    {
        private const string SaveKey = "ManchTak_PlayerProgress";

        public static void Save(PlayerProgressData data)
        {
            if (data == null)
                return;

            string json = JsonUtility.ToJson(data);

            PlayerPrefs.SetString(SaveKey, json);
            PlayerPrefs.Save();
        }

        public static PlayerProgressData Load()
        {
            if (!PlayerPrefs.HasKey(SaveKey))
                return new PlayerProgressData();

            string json = PlayerPrefs.GetString(SaveKey);

            if (string.IsNullOrEmpty(json))
                return new PlayerProgressData();

            PlayerProgressData data =
                JsonUtility.FromJson<PlayerProgressData>(json);

            return data ?? new PlayerProgressData();
        }

        public static bool HasSavedData()
        {
            return PlayerPrefs.HasKey(SaveKey);
        }

        public static void DeleteSave()
        {
            PlayerPrefs.DeleteKey(SaveKey);
            PlayerPrefs.Save();
        }
    }
}
