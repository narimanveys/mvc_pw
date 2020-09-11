using System.Collections.Generic;
using PWApp.Models;

namespace PWApp.Repositories
{
    public interface IUserAccountRepository
    {
        void CreateUserAccount(UserAccount userAccount);
        UserAccount GetUserAccount(string userId);
        UserAccount GetUserAccountByEmail(string userEmail);

        List<string> GetEmailListByTerm(string term, string currentUserId);

    }
}