
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

        // رنگ پس زمینه
        public string BackgroundColor;

        // رنگ سطح اصلی تخته
        public string BoardColor;

        // رنگ خانه های مسیر
        public string PathColor;

        // رنگ جزئیات و خطوط
        public string AccentColor;

        // سبک سطح تخته
        public string SurfaceStyle;

        public BoardThemeInfo(
            BoardTheme theme,
            string name,
            string description,
            string background,
            string board,
            string path,
            string accent,
            string surface)
        {
            Theme = theme;
            DisplayName = name;
            Description = description;
            BackgroundColor = background;
            BoardColor = board;
            PathColor = path;
            AccentColor = accent;
            SurfaceStyle = surface;
        }
    }

    public static class BoardThemeCatalog
    {
        public static BoardThemeInfo[] GetAllThemes()
        {
            return new BoardThemeInfo[]
            {
                // 1 - کلاسیک مدرن
                new BoardThemeInfo(
                    BoardTheme.ModernClassic,
                    "کلاسیک مدرن",
                    "طراحی ساده و لوکس با رنگ های متعادل",
                    "#182333",
                    "#F1F0E9",
                    "#FFFFFF",
                    "#D9B66F",
                    "Matte"
                ),

                // 2 - یخی
                new BoardThemeInfo(
                    BoardTheme.Ice,
                    "یخی",
                    "سطح سرد و تمیز با رنگ های یخی",
                    "#172B40",
                    "#C9E6F3",
                    "#F5FBFF",
                    "#70C9EF",
                    "Frosted"
                ),

                // 3 - چوب لوکس
                new BoardThemeInfo(
                    BoardTheme.LuxuryWood,
                    "چوب لوکس",
                    "چوب تیره با پرداخت ظریف",
                    "#211A17",
                    "#654735",
                    "#E9DCC8",
                    "#C69A62",
                    "DarkWood"
                ),

                // 4 - شیشه ای
                new BoardThemeInfo(
                    BoardTheme.Glass,
                    "شیشه ای",
                    "ظاهر شفاف و ساده",
                    "#172B39",
                    "#AFCBD7",
                    "#F4FAFC",
                    "#8BBFD2",
                    "ClearGlass"
                ),

                // 5 - فوتبالی
                new BoardThemeInfo(
                    BoardTheme.Football,
                    "فوتبالی",
                    "الهام گرفته از زمین فوتبال",
                    "#10291E",
                    "#286B42",
                    "#E9F0E8",
                    "#A9D477",
                    "Turf"
                ),

                // 6 - نئون
                new BoardThemeInfo(
                    BoardTheme.Neon,
                    "نئون",
                    "زمینه تیره با جزئیات نوری محدود",
                    "#111421",
                    "#252B3D",
                    "#E8EAF4",
                    "#7C8CFF",
                    "DarkMatte"
                )
            };
        }

        public static BoardThemeInfo GetTheme(BoardTheme theme)
        {
            BoardThemeInfo[] themes = GetAllThemes();

            foreach (BoardThemeInfo item in themes)
            {
                if (item.Theme == theme)
                    return item;
            }

            return themes[0];
        }
    }
}
