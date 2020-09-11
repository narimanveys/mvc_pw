using System;
using System.Collections.Generic;
using PWApp.Models;
using PWApp.Repositories;
using PWApp.ViewModels;

namespace PWApp.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUserAccountRepository _userAccountRepository;
        public TransactionService(ITransactionRepository transactionRepository, IUserAccountRepository userAccountRepository)
        {
            _transactionRepository = transactionRepository;
            _userAccountRepository = userAccountRepository;
        }

        public List<TransactionHistoryViewModel> GetAccountTransactions(int accId)
        {
            var usersTransactions = _transactionRepository.GetAccountTransactions(accId);
            var transactionsList = new List<TransactionHistoryViewModel>();
            foreach (var transaction in usersTransactions)
            {
                var transactionForView = new TransactionHistoryViewModel
                {
                    Id = transaction.Id,
                    Created = transaction.Created
                };
                if (transaction.Creditor.Id == accId)
                {
                    transactionForView.CorrespondentName = transaction.Debtor.User.FullName;
                    transactionForView.ResultingBalance = transaction.CreditorResultingBalance;
                    transactionForView.Credit = transaction.Amount;
                }
                else if (transaction.Debtor.Id == accId)
                {
                    transactionForView.CorrespondentName = transaction.Creditor.User.FullName;
                    transactionForView.ResultingBalance = transaction.DebtorResultingBalance;
                    transactionForView.Debit = transaction.Amount;
                }

                transactionsList.Add(transactionForView);
            }
            return transactionsList;
        }

        public void CreateTransaction(TransactionViewModel model, string creditorId)
        {
            var transaction = new Transaction
            {
                Amount = model.Amount,
                Created = DateTime.UtcNow
            };
            var creditor = _userAccountRepository.GetUserAccount(creditorId);
            var debtor = _userAccountRepository.GetUserAccountByEmail(model.CreditorEmail);

            if (debtor != null && creditor != null)
            {
                transaction.Creditor = creditor;
                transaction.Debtor = debtor;
                transaction.CreditorResultingBalance = creditor.AvailableAmount - model.Amount;
                transaction.DebtorResultingBalance = debtor.AvailableAmount + model.Amount;

                _transactionRepository.InsertTransaction(transaction);
            }
        }

        public Transaction GetTransactionById(int id)
        {
            return _transactionRepository.GetTransactionById(id);
        }

        public void CloneTransaction(Transaction transaction)
        {
            var newTransaction = new Transaction
            {
                Amount = transaction.Amount,
                Creditor = transaction.Creditor,
                Debtor = transaction.Debtor,
                Created = DateTime.UtcNow,
                CreditorResultingBalance = transaction.Creditor.AvailableAmount - transaction.Amount,
                DebtorResultingBalance = transaction.Debtor.AvailableAmount + transaction.Amount
            };
            _transactionRepository.InsertTransaction(newTransaction);
        }
    }
}