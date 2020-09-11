using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PWApp.Services;
using PWApp.ViewModels;

namespace PWApp.Controllers
{
    public class AccountTransactionsController : Controller
    {
        private readonly ITransactionService _transactionService;
        private readonly IUserAccountService _userAccountService;

        public AccountTransactionsController(IUserAccountService userAccountService,
            ITransactionService transactionService)
        {
            _userAccountService = userAccountService;
            _transactionService = transactionService;
        }

        public IActionResult Index(string sortOrder, string searchString, string currentFilter, int? pageNumber)
        {
            ViewData["NameSortParm"] = sortOrder == "name" ? "name_desc" : "name";
            ViewData["AmountSortParm"] = sortOrder == "amount" ? "amount_desc" : "amount";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentSort"] = sortOrder;

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userAccount = _userAccountService.GetUserAccount(userId);
            ViewBag.UserAccount = userAccount;

            var userTransactions = _transactionService.GetAccountTransactions(userAccount.Id);
            if (userTransactions != null && userTransactions.Any())
            {

                if (!string.IsNullOrEmpty(searchString))
                {
                    userTransactions = userTransactions.Where(s => s.CorrespondentName.Contains(searchString)).ToList();

                }

                switch (sortOrder)
                {
                    case "name_desc":
                        userTransactions = userTransactions.OrderByDescending(s => s.CorrespondentName).ToList();
                        break;
                    case "name":
                        userTransactions = userTransactions.OrderBy(s => s.CorrespondentName).ToList();
                        break;
                    case "Date":
                        userTransactions = userTransactions.OrderBy(s => s.Created).ToList();
                        break;
                    case "date_desc":
                        userTransactions = userTransactions.OrderByDescending(s => s.Created).ToList();
                        break;
                    case "amount":
                        userTransactions = userTransactions.OrderBy(s => s.ResultingBalance).ToList();
                        break;
                    case "amount_desc":
                        userTransactions = userTransactions.OrderByDescending(s => s.ResultingBalance).ToList();
                        break;
                }
            }
            
            var tableData =
                PaginatedList<TransactionHistoryViewModel>.CreateAsync(userTransactions, pageNumber ?? 1, 5);

            ViewBag.History = tableData;
             
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(TransactionViewModel model)
        {
            var creditorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = User.FindFirstValue(ClaimTypes.Name);

            var userAccount = _userAccountService.GetUserAccount(creditorId);
            ViewBag.UserAccount = userAccount;

            var userTransactions = _transactionService.GetAccountTransactions(userAccount.Id);

            var tableData =
                PaginatedList<TransactionHistoryViewModel>.CreateAsync(userTransactions, 1, 5);

            ViewBag.History = tableData;
            if (userAccount.User.Email == model.CreditorEmail.Trim())
            {
                ModelState.AddModelError("Receiver", "Сan't borrow money for yourself!");
            }

            if (!_userAccountService.CanMakeTransaction(creditorId, model.Amount))
            {
                ModelState.AddModelError("Amount", "Insufficient funds in the account!"); 
            }

            try
            {
                _transactionService.CreateTransaction(model, creditorId);
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("DB", $"Something went wrong during transaction saving: {ex.Message}");
            }

            if (!ModelState.IsValid)
            {
                return View();
            }

            _transactionService.CreateTransaction(model, creditorId);

            return View();
        }

        [HttpPost]
        public ActionResult AutocompleteSearch(string term)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var emails = _userAccountService.GetEmailListByTerm(term, userId);
            return Json(emails);
        }

        [HttpGet]
        public ActionResult CloneTransaction(int id)
        {
            var transaction = _transactionService.GetTransactionById(id);
            _transactionService.CloneTransaction(transaction);
            return Redirect("/AccountTransactions");
        }
    }
}