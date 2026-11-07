
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace ManchTak
{
    public static class PlayerIdGenerator
    {
        public const int IdLength = 6;

        private const string Characters =
            "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";

        private const string AdminId = "faridtak";

        // فقط برای تست محلی؛ مرجع نهایی سرور است.
        private static readonly HashSet<string> ReservedIds =
            new HashSet<string>(
                StringComparer.OrdinalIgnoreCase
            )
            {
                AdminId
            };

        // تولید شناسه تصادفی امن
        public static string GenerateId()
        {
            using (RandomNumberGenerator rng =
                   RandomNumberGenerator.Create())
            {
                byte[] bytes = new byte[IdLength];
                char[] result = new char[IdLength];

                int index = 0;

                while (index < IdLength)
                {
                    rng.GetBytes(bytes);

                    for (int i = 0;
                         i < bytes.Length && index < IdLength;
                         i++)
                    {
                        // جلوگیری از توزیع نامتوازن
                        if (bytes[i] >= 224)
                            continue;

                        result[index] =
                            Characters[bytes[i] % Characters.Length];

                        index++;
                    }
                }

                return new string(result);
            }
        }

        // بررسی ساختار شناسه
        public static bool IsValidId(string playerId)
        {
            if (string.IsNullOrWhiteSpace(playerId))
                return false;

            if (playerId.Length != IdLength)
                return false;

            foreach (char c in playerId)
            {
                if (Characters.IndexOf(c) < 0)
                    return false;
            }

            return true;
        }

        // بررسی شناسه رزروشده مدیر
        public static bool IsReservedId(string playerId)
        {
            return !string.IsNullOrWhiteSpace(playerId) &&
                   string.Equals(
                       playerId,
                       AdminId,
                       StringComparison.OrdinalIgnoreCase
                   );
        }

        // ثبت شناسه در حافظه تست
        public static bool RegisterTestId(string playerId)
        {
            if (!IsValidId(playerId) ||
                IsReservedId(playerId))
                return false;

            return ReservedIds.Add(playerId);
        }

        // بررسی شناسه ثبت‌شده در تست
        public static bool IsTestIdAvailable(string playerId)
        {
            return IsValidId(playerId) &&
                   !ReservedIds.Contains(playerId);
        }

        // پاکسازی اطلاعات تست
        public static void ClearTestIds()
        {
            ReservedIds.Clear();
            ReservedIds.Add(AdminId);
        }
    }
}
