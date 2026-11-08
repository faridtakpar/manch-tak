using UnityEngine;

namespace ManchTak
{
    public class PlayerLevelBadgeBinder : MonoBehaviour
    {
        [Header("Level Badge")]
        [SerializeField]
        private PlayerLevelBadgeView badgeView;

        private PlayerProfileViewController profileController;

        // اتصال نشان لول به مدیر پروفایل
        public void Bind(PlayerProfileViewController controller)
        {
            Unbind();

            if (controller == null)
                return;

            profileController = controller;

            profileController.OnProfileChanged +=
                HandleProfileChanged;

            // نمایش اطلاعات فعلی بازیکن
            HandleProfileChanged(profileController.Profile);
        }

        // دریافت تغییرات پروفایل
        private void HandleProfileChanged(
            PlayerProfileViewData profile)
        {
            if (badgeView == null || profile == null)
                return;

            badgeView.SetProfile(profile);
        }

        // قطع اتصال قبلی
        public void Unbind()
        {
            if (profileController != null)
            {
                profileController.OnProfileChanged -=
                    HandleProfileChanged;

                profileController = null;
            }
        }

        private void OnDestroy()
        {
            Unbind();
        }
    }
}
