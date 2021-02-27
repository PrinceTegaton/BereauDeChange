using BDC.Core.Models;
using System.Collections.Generic;

namespace BDC.Web.Models
{
    public class IndexPageViewModel
    {
        public IEnumerable<ExchangeRate> ExchangeRates { get; set; }
        public IEnumerable<Bank> Banks { get; set; }
    }

    public class AdminPageViewModel
    {
        public User User { get; set; }
        public IEnumerable<User> Users { get; set; }
        public IEnumerable<Transaction> Transactions { get; set; }
    }

    public class DashboardViewModel
    {
        public User User { get; set; }
        public Account Account { get; set; }
        public IEnumerable<Bank> Banks { get; set; }
        public IEnumerable<KeyValuePair<string, decimal>> CurrencyAndRate { get; set; }
        public List<Transaction> TransferHistory { get; set; }
    }

    public class CreateUserViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string Bank { get; set; }
        public string AccountNo { get; set; }
    }

    public class LoginViewModel
    {
        public string LoginEmail { get; set; }
        public string Password { get; set; }
    }

    public class TransferViewModel
    {
        public string ReceiverName { get; set; }
        public string SwiftCode { get; set; }
        public string Country { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string AccountNo { get; set; }
        public string Email { get; set; }
        public string PIN { get; set; }
    }
}
