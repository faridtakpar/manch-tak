
using System;

namespace ManchTak.UI
{
    public class PlayerProfileController
    {
        public PlayerProfileViewData Profile { get; private set; }

        public event Action<PlayerProfileViewData> OnProfileChanged;

        public PlayerProfileController(
            PlayerProfileViewData profile = null)
        {
            Profile = profile ?? new PlayerProfileViewData();
        }

        public bool ChangeDisplayName(string newName)
        {
            if (!PlayerProfileRules.SetDisplayName(
                Profile,
                newName))
            {
                return false;
            }

            NotifyProfileChanged();
            return true;
        }

        public bool ChangeAvatar(string avatarId)
        {
            PlayerAvatarData avatar =
                PlayerAvatarCatalog.FindById(avatarId);

            if (avatar == null)
                return false;

            if (!PlayerProfileRules.SetAvatar(
                Profile,
                avatar.AvatarType,
                avatar.AvatarId))
            {
                return false;
            }

            NotifyProfileChanged();
            return true;
        }

        public bool RecordMatchResult(bool won)
        {
            if (!PlayerProfileRules.UpdateMatchStats(
                Profile,
                won))
            {
                return false;
            }

            NotifyProfileChanged();
            return true;
        }

        public float GetWinRate()
        {
            return PlayerProfileRules.GetWinRate(Profile);
        }

        private void NotifyProfileChanged()
        {
            OnProfileChanged?.Invoke(Profile);
        }
    }
}
