
namespace ManchTak
{
    public enum BoardTheme
    {
        ModernClassic,
        Ice,
        LuxuryWood,
        Glass,
        Football,
        Neon
    }

    [System.Serializable]
    public class BoardThemeInfo
    {
        public BoardTheme Theme;
        public string DisplayName;
        public string Description;
        public bool IsUnlocked;
        public int UnlockDiamondCost;

        public BoardThemeInfo(
            BoardTheme theme,
            string displayName,
            string description,
            int cost = 0)
        {
            Theme = theme;
            DisplayName = displayName;
            Description = description;
            UnlockDiamondCost = cost;
            IsUnlocked = cost == 0;
        }
    }

    public static class BoardThemeCatalog
    {
        public static BoardThemeInfo[] GetAllThemes()
        {
            return new BoardThemeInfo[]
            {
                new BoardThemeInfo(
                    BoardTheme.ModernClassic,
                    "کلاسیک مدرن",
                    "تخته کلاسیک با طراحی مدرن"),

                new BoardThemeInfo(
                    BoardTheme.Ice,
                    "تخته یخی",
                    "سطح شیشه‌ای یخی و کریستالی",
                    20),

                new BoardThemeInfo(
                    BoardTheme.LuxuryWood,
                    "چوب لوکس",
                    "تخته چوبی با جزئیات ظریف",
                    30),

                new BoardThemeInfo(
                    BoardTheme.Glass,
                    "تخته شیشه‌ای",
                    "طراحی شفاف و براق",
                    35),

                new BoardThemeInfo(
                    BoardTheme.Football,
                    "فوتبالی",
                    "طراحی الهام‌گرفته از زمین فوتبال",
                    40),

                new BoardThemeInfo(
                    BoardTheme.Neon,
                    "نئون",
                    "نورپردازی درخشان و مدرن",
                    50)
            };
        }
    }
}
