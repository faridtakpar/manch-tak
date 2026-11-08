using UnityEngine;
using UnityEngine.UI;

namespace ManchTak
{
    public class PlayerLevelBadgeView : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Image badgeBackground;
        [SerializeField] private Text levelText;

        public void SetLevel(int level)
        {
            if (!PlayerLevelBadgeRules.IsValidLevel(level))
                level = 1;

            if (badgeBackground != null)
            {
                badgeBackground.color =
                    PlayerLevelBadgeRules.GetColor(level);
            }

            if (levelText != null)
            {
                levelText.text =
                    PlayerLevelBadgeRules.GetLevelText(level);
            }
        }

        public void SetProfile(PlayerProfileViewData profile)
        {
            if (profile == null)
                return;

            SetLevel(profile.Level);
        }
    }
}
