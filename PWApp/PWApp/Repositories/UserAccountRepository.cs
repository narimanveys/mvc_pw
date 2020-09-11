using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PWApp.Data;
using PWApp.Models;

namespace PWApp.Repositories
{
    public class UserAccountRepository : IUserAccountRepository
    {
        public static decimal DefaultPwAmount = 500;
        public ApplicationContext _context;
        public UserAccountRepository(ApplicationContext context)
        {
            _context = context;
        }

        public void CreateUserAccount(UserAccount userAccount)
        {
            try
            {
                userAccount.AvailableAmount = DefaultPwAmount;
                _context.UserAccounts.Add(userAccount);

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Save user exception: {ex.Message}");
            }
        }

        public UserAccount GetUserAccount(string userId)
        {
            var userAccount = _context.UserAccounts
                .Include(x => x.User)
                .FirstOrDefault(x => x.User.Id == userId);
            return userAccount;
        }

        public UserAccount GetUserAccountByEmail(string userEmail)
        {
            var userAccount = _context.UserAccounts.FirstOrDefault(x => x.User.Email == userEmail);
            return userAccount;
        }

        public List<string> GetEmailListByTerm(string term, string currentUserId)
        {
            var emails = _context.Users.Where(a => a.Email.Contains(term) && a.Id != currentUserId)
                .Select(a => a.Email)
                .Distinct()
                .ToList();
            return emails;
        }
    }
}