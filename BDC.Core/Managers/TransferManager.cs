using BDC.Core.Adapters;
using BDC.Core.Models;
using BDC.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BDC.Core.Managers
{
    public class TransferManager : ITransferManager
    {
        private readonly DataContext<Transaction> _trnxContext;
        private readonly IPaymentEngineAdapter _paymentEngine;
        private readonly IFxAdapter _fxAdapter;

        private const decimal dailyLimit = 500m;

        public TransferManager(IPaymentEngineAdapter paymentEngine, IFxAdapter fxAdapter)
        {
            _trnxContext = new DataContext<Transaction>();
            _paymentEngine = paymentEngine;
            _fxAdapter = fxAdapter;
        }

        public Transaction Get(Guid userId)
        {
            return _trnxContext.GetById(userId);
        }

        public Result<string> Transfer(Transaction transaction)
        {
            // business rules
            if (transaction.Amount <= 0)
                return Result<string>.Failure("Transfer amount must be greater than zero");

            if (transaction.Amount > dailyLimit)
                return Result<string>.Failure($"Sorry, you cannot transfer above the daily limit of {transaction.DestinationCurrency} {dailyLimit}");

            var sum = GetTransferHistory(transaction.UserId, TransactionStatus.Successful).Where(a => a.CreatedOn.Date == DateTime.Now.Date)?.Sum(a => a.Amount);

            if (sum.HasValue)
            {
                if (sum.Value >= dailyLimit)
                    return Result<string>.Failure($"Daily limit of {dailyLimit.ToString("N2")} exceeded");

                decimal availableLimit = dailyLimit - sum.Value;

                if ((sum.Value + transaction.Amount) > dailyLimit)
                    return Result<string>.Failure($"Amount to be transferred will limit exceeded daily limit. You have {(availableLimit.ToString("N2"))} left to transfer today");
            }


            // debit source account
            var debit = _paymentEngine.PostPaymentFromBankAccount(transaction.SourceBankCode, transaction.SourceAccountNumber, transaction.Amount);
            if (!debit.IsSuccess)
                return Result<string>.Failure(null, debit.Message);

            transaction.Status = TransactionStatus.Processing;
            transaction.DebitResponse = debit.Data;

            var trnx = _trnxContext.Add(transaction);

            // simulate FX payment via SWIFT
            Task.Run(() =>
            {
                Thread.Sleep(120000); // cause 2mins delay to simulate wait time. allow user to see trnx status as 'processing'

                var credit = _fxAdapter.PostFx(transaction);
                if (credit.IsSuccess)
                {
                    transaction.Status = TransactionStatus.Successful;
                    transaction.CreditResponse = credit.Data;
                }
                else
                {
                    transaction.Status = TransactionStatus.Failed;
                    transaction.CreditResponse = credit.Message;
                }

                _trnxContext.Update(transaction);
            });

            return Result<string>.Success(trnx.ToString(), "Transfer processing");
        }

        public IEnumerable<Transaction> GetTransferHistory(Guid? userId = null, TransactionStatus? status = null)
        {
            var history = _trnxContext.GetAll().Where(a => userId.HasValue ? a.UserId == userId : a.Id != Guid.Empty);

            if (status.HasValue)
            {
                history = history.Where(a => a.Status == status.Value);
            }

            return history.OrderByDescending(a => a.CreatedOn);
        }
    }
}
