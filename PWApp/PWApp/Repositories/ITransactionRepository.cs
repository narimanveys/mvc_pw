using System.Collections.Generic;
using PWApp.Models;

namespace PWApp.Repositories
{
    public interface ITransactionRepository
    {
        List<Transaction> GetAccountTransactions(int accId);
        void InsertTransaction(Transaction transaction);

        Transaction GetTransactionById(int id);

    }
}