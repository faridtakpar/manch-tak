
using System;
using UnityEngine;

namespace ManchTak
{
    public static class DiceVisualStorage
    {
        private const string SaveKeyPrefix =
            "ManchTak_SelectedDice_";

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
            DiceVisualType dice)
        {
            if (!Enum.IsDefined(typeof(DiceVisualType), dice))
                return;

            PlayerPrefs.SetInt(
                GetSaveKey(playerId),
                (int)dice
            );

            PlayerPrefs.Save();
        }

        public static DiceVisualType Load(string playerId)
        {
            string key = GetSaveKey(playerId);

            if (!PlayerPrefs.HasKey(key))
                return DiceVisualType.Classic;

            int savedValue = PlayerPrefs.GetInt(
                key,
                (int)DiceVisualType.Classic
            );

            if (!Enum.IsDefined(
                typeof(DiceVisualType),
                savedValue))
            {
                return DiceVisualType.Classic;
            }

            return (DiceVisualType)savedValue;
        }

        public static void DeleteSave(string playerId)
        {
            PlayerPrefs.DeleteKey(GetSaveKey(playerId));
            PlayerPrefs.Save();
        }
    }
}
