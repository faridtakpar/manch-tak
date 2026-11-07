
using System;

namespace ManchTak
{
    public class PlayerAvatarController
    {
        public int SelectedAvatarId { get; private set; }

        public int PlayerLevel { get; private set; }

        public event Action<int> OnAvatarChanged;

        public PlayerAvatarController(int playerLevel)
        {
            PlayerLevel = Math.Max(1, playerLevel);
            SelectedAvatarId = 1;
        }

        public bool SelectAvatar(int avatarId)
        {
            if (!PlayerAvatarCatalog.IsUnlocked(
                avatarId,
                PlayerLevel))
            {
                return false;
            }

            SelectedAvatarId = avatarId;

            OnAvatarChanged?.Invoke(SelectedAvatarId);

            return true;
        }

        public void UpdatePlayerLevel(int newLevel)
        {
            PlayerLevel = Math.Max(1, newLevel);
        }

        public bool IsSelected(int avatarId)
        {
            return SelectedAvatarId == avatarId;
        }

        public bool IsAvailable(int avatarId)
        {
            return PlayerAvatarCatalog.IsUnlocked(
                avatarId,
                PlayerLevel
            );
        }

        public PlayerAvatarData GetSelectedAvatarData()
        {
            return PlayerAvatarCatalog.Find(
                SelectedAvatarId
            );
        }
    }
}
