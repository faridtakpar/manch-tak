
using System;
using UnityEngine;

namespace ManchTak
{
    public static class DiceVisualStorage
    {
        private const string SaveKey = "ManchTak_SelectedDice";

        public static void Save(DiceVisualType dice)
        {
            PlayerPrefs.SetInt(SaveKey, (int)dice);
            PlayerPrefs.Save();
        }

        public static DiceVisualType Load()
        {
            if (!PlayerPrefs.HasKey(SaveKey))
                return DiceVisualType.Classic;

            int savedValue = PlayerPrefs.GetInt(
                SaveKey,
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

        public static void DeleteSave()
        {
            PlayerPrefs.DeleteKey(SaveKey);
            PlayerPrefs.Save();
        }
    }
}
