using BDC.Core.Models;
using System;
using System.Collections.Generic;

namespace BDC.Core.Managers
{
    public interface IAccountManager
    {
        IEnumerable<Bank> GetBanks();
        IEnumerable<ExchangeRate> GetExchangeRates();
        Result<decimal> GetRate(string sourceCurrency, string targetCurrency);
        IEnumerable<Account> GetUserAccounts(Guid userId);
        IEnumerable<KeyValuePair<string, decimal>> GetCurrenciesWithRate();
    }
}