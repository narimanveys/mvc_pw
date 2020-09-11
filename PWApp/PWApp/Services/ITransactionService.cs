using System.Collections.Generic;
using PWApp.Models;
using PWApp.ViewModels;

namespace PWApp.Services
{
    public interface ITransactionService
    {
        public List<TransactionHistoryViewModel> GetAccountTransactions(int accId);
        void CreateTransaction(TransactionViewModel transaction, string creditorId);
        Transaction GetTransactionById(int id);
        void CloneTransaction(Transaction transaction);
    }
}