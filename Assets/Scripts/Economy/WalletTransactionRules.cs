
using System;
using System.Collections.Generic;

namespace ManchTak
{
    public static class WalletTransactionRules
    {
        private static readonly HashSet<string> ProcessedTransactions =
            new HashSet<string>();

        private static readonly object TransactionLock =
            new object();

        // بررسی اعتبار تراکنش
        public static bool IsValidTransaction(
            WalletTransaction transaction)
        {
            return transaction != null &&
                   transaction.IsValid() &&
                   transaction.Amount > 0;
        }

        // بررسی تکراری نبودن تراکنش
        public static bool IsNewTransaction(
            string transactionId)
        {
            if (string.IsNullOrWhiteSpace(transactionId))
                return false;

            lock (TransactionLock)
            {
                return !ProcessedTransactions.Contains(
                    transactionId
                );
            }
        }

        // رزرو شناسه تراکنش برای جلوگیری از ثبت تکراری
        public static bool RegisterTransaction(
            WalletTransaction transaction)
        {
            if (!IsValidTransaction(transaction))
                return false;

            if (transaction.Status !=
                WalletTransactionStatus.Pending)
                return false;

            lock (TransactionLock)
            {
                if (ProcessedTransactions.Contains(
                    transaction.TransactionId))
                    return false;

                ProcessedTransactions.Add(
                    transaction.TransactionId
                );

                return true;
            }
        }

        // تأیید تراکنش پس از اجرای موفق عملیات
        public static bool ConfirmTransaction(
            WalletTransaction transaction)
        {
            if (!IsValidTransaction(transaction))
                return false;

            lock (TransactionLock)
            {
                if (!ProcessedTransactions.Contains(
                    transaction.TransactionId))
                    return false;

                if (transaction.Status !=
                    WalletTransactionStatus.Pending)
                    return false;

                transaction.Status =
                    WalletTransactionStatus.Confirmed;

                return true;
            }
        }

        // رد کردن تراکنش ناموفق
        public static bool RejectTransaction(
            WalletTransaction transaction)
        {
            if (!IsValidTransaction(transaction))
                return false;

            lock (TransactionLock)
            {
                if (!ProcessedTransactions.Contains(
                    transaction.TransactionId))
                    return false;

                if (transaction.Status !=
                    WalletTransactionStatus.Pending)
                    return false;

                transaction.Status =
                    WalletTransactionStatus.Rejected;

                return true;
            }
        }

        // ساخت تراکنش جدید
        public static WalletTransaction CreateTransaction(
            string playerId,
            WalletCurrency currency,
            WalletTransactionType type,
            long amount,
            string matchId = "",
            string description = "")
        {
            if (string.IsNullOrWhiteSpace(playerId) ||
                amount <= 0 ||
                !Enum.IsDefined(typeof(WalletCurrency), currency) ||
                !Enum.IsDefined(typeof(WalletTransactionType), type))
                return null;

            WalletTransaction transaction =
                new WalletTransaction();

            transaction.PlayerId = playerId;
            transaction.Currency = currency;
            transaction.Type = type;
            transaction.Amount = amount;
            transaction.MatchId = matchId ?? string.Empty;
            transaction.Description =
                description ?? string.Empty;

            return IsValidTransaction(transaction)
                ? transaction
                : null;
        }

        // پاکسازی مخصوص تست محلی
        public static void ClearTestTransactions()
        {
            lock (TransactionLock)
            {
                ProcessedTransactions.Clear();
            }
        }

        // تعداد تراکنش های ثبت شده
        public static int GetProcessedTransactionCount()
        {
            lock (TransactionLock)
            {
                return ProcessedTransactions.Count;
            }
        }
    }
}
