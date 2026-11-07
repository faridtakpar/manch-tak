
using UnityEngine;

namespace ManchTak
{
    public static class PlayerAvatarStorage
    {
        private const string SaveKey = "ManchTak_SelectedAvatar";

        public static void Save(int avatarId)
        {
            if (PlayerAvatarCatalog.Find(avatarId) == null)
                return;

            PlayerPrefs.SetInt(SaveKey, avatarId);
            PlayerPrefs.Save();
        }

        public static int Load()
        {
            int avatarId = PlayerPrefs.GetInt(SaveKey, 1);

            if (PlayerAvatarCatalog.Find(avatarId) == null)
                return 1;

            return avatarId;
        }

        public static void DeleteSave()
        {
            PlayerPrefs.DeleteKey(SaveKey);
            PlayerPrefs.Save();
        }
    }
}
