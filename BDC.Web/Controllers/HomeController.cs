using BDC.Core;
using BDC.Core.Managers;
using BDC.Core.Models;
using BDC.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;

namespace BDC.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserManager _userManager;
        private readonly IAccountManager _accountManager;
        private readonly ITransferManager _trnxManager;

        public HomeController(ILogger<HomeController> logger,
                                IUserManager userManager,
                                IAccountManager accountManager,
                                ITransferManager trnxManager)
        {
            _logger = logger;
            _userManager = userManager;
            _accountManager = accountManager;
            _trnxManager = trnxManager;
        }

        public IActionResult Index()
        {
            if (StaticCache.User != null)
                return RedirectToAction(nameof(Dashboard));

            if (StaticCache.ExchangeRate == null)
            {
                StaticCache.ExchangeRate = _accountManager.GetExchangeRates();
            }

            if (StaticCache.Banks == null)
            {
                StaticCache.Banks = _accountManager.GetBanks(); ;
            }
            
            var pageModel = new IndexPageViewModel
            {
                Banks = StaticCache.Banks,
                ExchangeRates = StaticCache.ExchangeRate
            };

            return View(pageModel);
        }

        public IActionResult Dashboard()
        {
            if (StaticCache.User == null)
                return RedirectToAction(nameof(Index));

            if (StaticCache.User.IsAdmin)
                return RedirectToAction(nameof(Admin));

            if (StaticCache.Dashboard == null)
            {
                // cache dashboard data
                var userAccount = _accountManager.GetUserAccounts(StaticCache.User.Id).FirstOrDefault();
                var history = _trnxManager.GetTransferHistory(StaticCache.User.Id);
                if (history.Count() > 6)
                {
                    history = history.Take(6);
                }

                var bank = _accountManager.GetBanks()?.FirstOrDefault(a => a.Code.ToLower() == userAccount.BankCode.ToLower()); ;

                userAccount.BankName = bank?.Name ?? "N/A";
                StaticCache.CurrencyAndRate = _accountManager.GetCurrenciesWithRate();

                var dashboard = new DashboardViewModel
                {
                    User = StaticCache.User,
                    Account = userAccount,
                    CurrencyAndRate = StaticCache.CurrencyAndRate,
                    TransferHistory = history.ToList()
                };

                StaticCache.Dashboard = dashboard;
                return View(dashboard);
            }

            return View(StaticCache.Dashboard);
        }

        [HttpPost]
        public IActionResult CreateAccount([FromForm] CreateUserViewModel model)
        {
            if (model == null)
                return Json(Result.Failure("Form data not supplied"));

            var res = _userManager.Create(new User
            {
                Name = model.Name,
                EmailAddress = model.Email,
                MobileNumber = model.MobileNo,
                Status = UserStatus.Active,
                Password = "123456"
            },
            new Account
            {
                BankCode = model.Bank,
                AccountName = model.Name,
                AccountNumber = model.AccountNo,
                Status = AccountStatus.Active
            });

            return Json(res);
        }

        [HttpPost]
        public IActionResult Login([FromForm] LoginViewModel model)
        {
            var res = _userManager.Login(model.LoginEmail, model.Password);
            if (res.IsSuccess)
                StaticCache.User = res.Data;

            return Json(res);
        }

        [HttpPost]
        public IActionResult Transfer([FromForm] TransferViewModel model)
        {
            if (StaticCache.User == null)
                return RedirectToAction(nameof(Index));

            if (model.PIN != "1234") return Json(Result<string>.Failure("Incorrect PIN"));

            var userAccount = _accountManager.GetUserAccounts(StaticCache.User.Id).FirstOrDefault();

            var rate = _accountManager.GetRate(userAccount.Currency, model.Currency);
            if (rate.Data == 0) return Json(Result<string>.Failure(rate.Message));

            var trnx = new Transaction
            {
                UserId = StaticCache.User.Id,
                SourceAccountNumber = userAccount.AccountNumber,
                SourceBankCode = userAccount.BankCode,
                Country = model.Country,
                SwiftCode = model.SwiftCode,
                SourceCurrency = userAccount.Currency,
                DestinationAccountNumber = model.AccountNo,
                DestinationAccountName = model.ReceiverName,
                DestinationCurrency = model.Currency,
                Amount = model.Amount,
                Rate = rate.Data,
                Fee = 0,
                Email = model.Email,
                AuthMethod = "PIN",
                Status = TransactionStatus.Processing
            };

            var res = _trnxManager.Transfer(trnx);

            if (res.IsSuccess)
            {
                StaticCache.Dashboard.TransferHistory.Insert(0, trnx);
            }

            return Json(res);
        }

        public IActionResult Admin()
        {
            if (StaticCache.User == null)
                return RedirectToAction(nameof(Index));

            if (!StaticCache.User.IsAdmin)
                return RedirectToAction(nameof(Dashboard));

            var history = _trnxManager.GetTransferHistory();
            if (history.Count() > 20)
            {
                history = history.Take(20);
            }

            var users = _userManager.GetAll();
            if (users.Count() > 20)
            {
                users = users.Take(20);
            }

            var dashboard = new AdminPageViewModel
            {
                User = StaticCache.User,
                Users = users,
                Transactions = history.ToList()
            };

            return View(dashboard);
        }

        [HttpPost]
        public IActionResult Logout()
        {
            // clear cache on logout
            StaticCache.User = null;
            StaticCache.Dashboard = null;
            StaticCache.CurrencyAndRate = null;
            StaticCache.ExchangeRate = null;
            StaticCache.TransferHistory = null;

            return Json(new { IsSuccess = true });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public static string GetTransactionStatusColor(TransactionStatus status)
        {
            string color = "badge-primary";

            switch (status)
            {
                case TransactionStatus.Pending:
                    color = "badge-secondary"; break;

                case TransactionStatus.Processing:
                    color = "badge-primary"; break;

                case TransactionStatus.Successful:
                    color = "badge-success"; break;

                case TransactionStatus.Failed:
                    color = "badge-danger"; break;
            }

            return color;
        }
    }
}
