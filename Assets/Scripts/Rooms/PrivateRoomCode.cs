
using System;
using System.Text;

namespace ManchTak
{
    public static class PrivateRoomCode
    {
        // حروف و اعدادی که با هم اشتباه نمی شوند
        private const string AllowedCharacters =
            "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";

        // طول کد دعوت
        public const int CodeLength = 6;

        // تولید کد دعوت
        public static string GenerateCode()
        {
            StringBuilder code = new StringBuilder();

            for (int i = 0; i < CodeLength; i++)
            {
                int index = UnityEngine.Random.Range(
                    0,
                    AllowedCharacters.Length
                );

                code.Append(AllowedCharacters[index]);
            }

            return code.ToString();
        }

        // بررسی معتبر بودن کد وارد شده
        public static bool IsValidCode(string code)
        {
            if (string.IsNullOrEmpty(code))
                return false;

            if (code.Length != CodeLength)
                return false;

            foreach (char character in code)
            {
                if (!AllowedCharacters.Contains(character))
                    return false;
            }

            return true;
        }

        // یکسان سازی کد برای جستجو
        public static string NormalizeCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return string.Empty;

            return code.Trim().ToUpperInvariant();
        }
    }
}
