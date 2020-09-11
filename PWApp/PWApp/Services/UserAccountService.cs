using System;
using System.Collections.Generic;
using PWApp.Models;
using PWApp.Repositories;

namespace PWApp.Services
{
    public class UserAccountService : IDisposable, IUserAccountService
    {
        private readonly IUserAccountRepository _userAccountRepository;
        public UserAccountService(IUserAccountRepository userAccountRepository)
        {
            _userAccountRepository = userAccountRepository;
        }

        public void CreateUserAccount(UserAccount userAccount)
        {
            _userAccountRepository.CreateUserAccount(userAccount);
        }

        public UserAccount GetUserAccount(string userId)
        {
            return _userAccountRepository.GetUserAccount(userId);
        }

        public List<string> GetEmailListByTerm(string term, string currentUserId)
        {
            return _userAccountRepository.GetEmailListByTerm(term,  currentUserId);
        }

        public bool CanMakeTransaction(string userId, decimal amount)
        {
            var userAccount = _userAccountRepository.GetUserAccount(userId);
            return amount <= userAccount.AvailableAmount;
        }

        public void Dispose()
        {
        }
    }
}