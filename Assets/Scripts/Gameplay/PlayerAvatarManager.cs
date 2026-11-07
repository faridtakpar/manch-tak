
namespace ManchTak
{
    public class PlayerAvatarManager
    {
        public PlayerAvatarController AvatarController
        {
            get;
            private set;
        }

        public PlayerAvatarManager(int playerLevel)
        {
            AvatarController =
                new PlayerAvatarController(playerLevel);

            LoadSelectedAvatar();
        }

        private void LoadSelectedAvatar()
        {
            int savedAvatarId = PlayerAvatarStorage.Load();

            if (!AvatarController.SelectAvatar(savedAvatarId))
            {
                AvatarController.SelectAvatar(1);
                PlayerAvatarStorage.Save(1);
            }
        }

        public bool SelectAvatar(int avatarId)
        {
            if (!AvatarController.SelectAvatar(avatarId))
                return false;

            PlayerAvatarStorage.Save(avatarId);

            return true;
        }

        public void UpdatePlayerLevel(int newLevel)
        {
            AvatarController.UpdatePlayerLevel(newLevel);
        }

        public int GetSelectedAvatarId()
        {
            return AvatarController.SelectedAvatarId;
        }

        public PlayerAvatarData GetSelectedAvatar()
        {
            return AvatarController.GetSelectedAvatarData();
        }
    }
}
