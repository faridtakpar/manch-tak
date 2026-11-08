
using UnityEngine;

namespace ManchTak
{
    public class PlayerProfileBootstrap : MonoBehaviour
    {
        [Header("Profile UI")]
        [SerializeField]
        private PlayerLevelBadgeBinder levelBadgeBinder;

        [Header("Default Profile")]
        [SerializeField]
        private string defaultDisplayName = "بازیکن جدید";

        public PlayerProfileManager ProfileManager
        {
            get;
            private set;
        }

        public bool IsInitialized
        {
            get
            {
                return ProfileManager != null;
            }
        }

        // راه‌اندازی پروفایل پس از ورود موفق بازیکن
        public bool InitializeForPlayer(
            string authenticatedPlayerId)
        {
            if (string.IsNullOrWhiteSpace(
                authenticatedPlayerId))
            {
                Debug.LogError(
                    "شناسه بازیکن معتبر نیست."
                );

                return false;
            }

            if (ProfileManager != null)
            {
                if (ProfileManager.Profile.PlayerId ==
                    authenticatedPlayerId)
                {
                    return true;
                }

                ProfileManager.Close();
                ProfileManager = null;
            }

            try
            {
                ProfileManager = new PlayerProfileManager(
                    authenticatedPlayerId,
                    defaultDisplayName
                );

                if (levelBadgeBinder != null)
                {
                    ProfileManager.BindLevelBadge(
                        levelBadgeBinder
                    );
                }

                Debug.Log(
                    "پروفایل بازیکن با موفقیت راه‌اندازی شد: " +
                    authenticatedPlayerId
                );

                return true;
            }
            catch (System.Exception exception)
            {
                ProfileManager = null;

                Debug.LogError(
                    "خطا در راه‌اندازی پروفایل: " +
                    exception.Message
                );

                return false;
            }
        }

        // اتصال نشان لول پس از آماده شدن رابط کاربری
        public void AttachLevelBadge(
            PlayerLevelBadgeBinder binder)
        {
            if (binder == null)
                return;

            levelBadgeBinder = binder;

            if (ProfileManager != null)
            {
                ProfileManager.BindLevelBadge(
                    levelBadgeBinder
                );
            }
        }

        // ذخیره اطلاعات فعلی بازیکن
        public void SaveProfile()
        {
            if (ProfileManager == null)
                return;

            ProfileManager.Save();
        }

        // بستن پروفایل هنگام خروج از حساب
        public void Logout()
        {
            if (ProfileManager == null)
                return;

            ProfileManager.Close();
            ProfileManager = null;
        }

        private void OnApplicationPause(bool paused)
        {
            if (paused)
                SaveProfile();
        }

        private void OnApplicationQuit()
        {
            SaveProfile();
        }

        private void OnDestroy()
        {
            if (ProfileManager != null)
            {
                ProfileManager.Close();
                ProfileManager = null;
            }
        }
    }
}
