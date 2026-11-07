
using System;
using System.Collections.Generic;

namespace ManchTak
{
    public static class WalletTransactionRules
    {
        private static readonly HashSet<string> ProcessedTransactions =
            new HashSet<string>();

        // بررسی اعتبار تراکنش
        public static bool IsValidTransaction(
            WalletTransaction transaction)
        {
            return transaction != null &&
                   transaction.IsValid();
        }

        // بررسی تکراری نبودن تراکنش
        public static bool IsNewTransaction(
            string transactionId)
        {
            return !string.IsNullOrWhiteSpace(transactionId) &&
                   !ProcessedTransactions.Contains(transactionId);
        }

        // ثبت شناسه تراکنش
        public static bool RegisterTransaction(
            WalletTransaction transaction)
        {
            if (!IsValidTransaction(transaction))
                return false;

            if (!IsNewTransaction(transaction.TransactionId))
                return false;

            ProcessedTransactions.Add(
                transaction.TransactionId
            );

            transaction.Status =
                WalletTransactionStatus.Confirmed;

            return true;
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
                amount <= 0)
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

            return transaction.IsValid()
                ? transaction
                : null;
        }

        // پاکسازی شناسه های ثبت شده
        // فقط برای تست محلی
        public static void ClearTestTransactions()
        {
            ProcessedTransactions.Clear();
        }

        // تعداد تراکنش های ثبت شده در حافظه
        public static int GetProcessedTransactionCount()
        {
            return ProcessedTransactions.Count;
        }
    }
}
