
using System;
using UnityEngine;

namespace ManchTak
{
    public static class PlayerProgressStorage
    {
        private const string SaveKeyPrefix =
            "ManchTak_PlayerProgress_";

        // ساخت کلید اختصاصی هر بازیکن
        private static string GetSaveKey(string playerId)
        {
            if (string.IsNullOrWhiteSpace(playerId))
                throw new ArgumentException(
                    "شناسه بازیکن معتبر نیست.",
                    nameof(playerId)
                );

            return SaveKeyPrefix + playerId.Trim();
        }

        // ذخیره اطلاعات بازیکن
        public static bool Save(PlayerProgressData data)
        {
            if (data == null ||
                string.IsNullOrWhiteSpace(data.PlayerId))
                return false;

            return Save(data.PlayerId, data);
        }

        // ذخیره با شناسه اختصاصی
        public static bool Save(
            string playerId,
            PlayerProgressData data)
        {
            if (data == null ||
                string.IsNullOrWhiteSpace(playerId))
                return false;

            string normalizedId = playerId.Trim();

            data.PlayerId = normalizedId;

            string key = GetSaveKey(normalizedId);
            string json = JsonUtility.ToJson(data);

            PlayerPrefs.SetString(key, json);
            PlayerPrefs.Save();

            return true;
        }

        // بارگذاری اطلاعات بازیکن
        public static PlayerProgressData Load(string playerId)
        {
            if (string.IsNullOrWhiteSpace(playerId))
                return null;

            string key = GetSaveKey(playerId);

            if (!PlayerPrefs.HasKey(key))
                return new PlayerProgressData(playerId, 1, 0, 0);

            string json = PlayerPrefs.GetString(key);

            if (string.IsNullOrWhiteSpace(json))
                return new PlayerProgressData(playerId, 1, 0, 0);

            try
            {
                PlayerProgressData data =
                    JsonUtility.FromJson<PlayerProgressData>(json);

                if (data == null)
                    return new PlayerProgressData(playerId, 1, 0, 0);

                // جلوگیری از بارگذاری اطلاعات با شناسه اشتباه
                data.PlayerId = playerId.Trim();

                return data;
            }
            catch (Exception)
            {
                Debug.LogWarning(
                    "اطلاعات پیشرفت بازیکن قابل بارگذاری نیست: " +
                    playerId
                );

                return new PlayerProgressData(playerId, 1, 0, 0);
            }
        }

        // بررسی وجود اطلاعات ذخیره‌شده
        public static bool HasSavedData(string playerId)
        {
            if (string.IsNullOrWhiteSpace(playerId))
                return false;

            return PlayerPrefs.HasKey(GetSaveKey(playerId));
        }

        // حذف اطلاعات فقط همان بازیکن
        public static void DeleteSave(string playerId)
        {
            if (string.IsNullOrWhiteSpace(playerId))
                return;

            string key = GetSaveKey(playerId);

            if (PlayerPrefs.HasKey(key))
            {
                PlayerPrefs.DeleteKey(key);
                PlayerPrefs.Save();
            }
        }
    }
}
