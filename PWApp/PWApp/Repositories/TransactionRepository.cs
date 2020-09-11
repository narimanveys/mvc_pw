using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PWApp.Data;
using PWApp.Models;

namespace PWApp.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        public ApplicationContext _context;
        public TransactionRepository(ApplicationContext context)
        {
            _context = context;
        }
        public List<Transaction> GetAccountTransactions(int accId)
        {
            return _context.Transactions
                .Include(c => c.Creditor).ThenInclude(c => c.User)
                .Include(c => c.Debtor).ThenInclude(c => c.User)
                .Where(t => t.Creditor.Id == accId || t.Debtor.Id == accId).ToList();
        }

        public void InsertTransaction(Transaction transactionEntity)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Transactions.Add(transactionEntity);

                    transactionEntity.Creditor.AvailableAmount -= transactionEntity.Amount;
                    transactionEntity.Debtor.AvailableAmount += transactionEntity.Amount;

                    _context.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception($"Save transaction error: {ex.Message}");
                }
            }
        }

        public Transaction GetTransactionById(int id)
        {
            return _context.Transactions
                .Include(c => c.Creditor)
                .ThenInclude(c => c.User)
                .Include(c => c.Debtor)
                .ThenInclude(c => c.User)
                .FirstOrDefault(x => x.Id == id);
        }
    }
}