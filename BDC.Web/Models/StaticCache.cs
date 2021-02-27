using BDC.Core.Models;
using System.Collections.Generic;

namespace BDC.Web.Models
{
    public class StaticCache
    {
        public static DashboardViewModel Dashboard { get; set; }
        public static User User { get; set; }
        public static IEnumerable<KeyValuePair<string, decimal>> CurrencyAndRate { get; set; }
        public static IEnumerable<ExchangeRate> ExchangeRate { get; set; }
        public static IEnumerable<Bank> Banks { get; set; }
        public static IEnumerable<Transaction> TransferHistory { get; set; }
    }
}
