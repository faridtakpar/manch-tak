
using System;

namespace ManchTak
{
    public class PlayerAvatarManager
    {
        public PlayerAvatarController AvatarController
        {
            get;
            private set;
        }

        private readonly string playerId;

        public PlayerAvatarManager(
            string playerId,
            int playerLevel)
        {
            if (string.IsNullOrWhiteSpace(playerId))
            {
                throw new ArgumentException(
                    "شناسه بازیکن معتبر نیست.",
                    nameof(playerId)
                );
            }

            this.playerId = playerId;

            AvatarController =
                new PlayerAvatarController(playerLevel);

            LoadSelectedAvatar();
        }

        private void LoadSelectedAvatar()
        {
            int savedAvatarId =
                PlayerAvatarStorage.Load(playerId);

            if (!AvatarController.SelectAvatar(savedAvatarId))
            {
                AvatarController.SelectAvatar(1);

                PlayerAvatarStorage.Save(
                    playerId,
                    AvatarController.SelectedAvatarId
                );
            }
        }

        public bool SelectAvatar(int avatarId)
        {
            if (!AvatarController.SelectAvatar(avatarId))
                return false;

            PlayerAvatarStorage.Save(
                playerId,
                avatarId
            );

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
