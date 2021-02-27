using BDC.Core.Models;
using BDC.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BDC.Core.Managers
{
    public class AccountManager : IAccountManager
    {
        private readonly DataContext<Account> _accountContext;
        private readonly DataContext<Bank> _bankContext;
        private readonly DataContext<ExchangeRate> _rateContext;

        public AccountManager()
        {
            _bankContext = new DataContext<Bank>();
            _accountContext = new DataContext<Account>();
            _rateContext = new DataContext<ExchangeRate>();
        }

        public IEnumerable<Bank> GetBanks()
        {
            return _bankContext.GetAll();
        }

        public IEnumerable<Account> GetUserAccounts(Guid userId)
        {
            return _accountContext.GetAll().Where(a => a.UserId == userId);
        }

        public IEnumerable<ExchangeRate> GetExchangeRates()
        {
            return _rateContext.GetAll().Where(a => a.TradingStart < DateTime.Now || a.TradingEnd > DateTime.Now);
        }

        public Result<decimal> GetRate(string sourceCurrency, string targetCurrency)
        {
            var rate = _rateContext.GetAll().LastOrDefault(a => a.SourceCurrency.ToLower().Trim() == sourceCurrency.ToLower().Trim() && a.TargetCurrency.ToLower().Trim() == targetCurrency.ToLower().Trim());

            if (rate == null) return Result<decimal>.Failure("Exchange rate does not exist for the selected currency");
            if (rate.TradingStart > DateTime.Now || rate.TradingEnd < DateTime.Now) return Result<decimal>.Failure("Exchange rate is not within a valid trading window");

            return Result<decimal>.Success(rate.Rate, "Exchange rate retrieved ok");
        }

        public IEnumerable<KeyValuePair<string, decimal>> GetCurrenciesWithRate()
        {
            var rate = _rateContext.GetAll().Where(a => a.TradingStart < DateTime.Now || a.TradingEnd > DateTime.Now);

            var kvp = new List<KeyValuePair<string, decimal>>();
            rate.ToList().ForEach(a => kvp.Add(new KeyValuePair<string, decimal>(a.TargetCurrency, a.Rate)));

            return kvp;
        }
    }
}
