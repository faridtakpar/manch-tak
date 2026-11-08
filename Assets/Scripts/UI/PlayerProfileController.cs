public bool SyncProgress(PlayerProfileData source)
{
    if (source == null)
        return false;

    if (!PlayerLevelBadgeRules.IsValidLevel(
        source.PlayerLevel))
        return false;

    Profile.DisplayName = string.IsNullOrWhiteSpace(
        source.DisplayName)
        ? "بازیکن جدید"
        : source.DisplayName;

    Profile.Level = source.PlayerLevel;
    Profile.TotalXP = Math.Max(0, source.TotalXP);

    Profile.TotalWins = Math.Max(0, source.TotalWins);
    Profile.TotalLosses = Math.Max(0, source.TotalLosses);

    Profile.TotalMatches = Profile.TotalWins +
                           Profile.TotalLosses;

    Profile.RefreshLevelBadge();

    NotifyProfileChanged();
    return true;
}
