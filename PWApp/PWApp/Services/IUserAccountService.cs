using System.Collections.Generic;
using PWApp.Models;

namespace PWApp.Services
{
    public interface IUserAccountService
    {
        void CreateUserAccount(UserAccount userAccount);
        UserAccount GetUserAccount(string userId);
        List<string> GetEmailListByTerm(string term, string currentUserId);
        bool CanMakeTransaction(string userId, decimal amount);
    }
}