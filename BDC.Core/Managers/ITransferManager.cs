using BDC.Core.Models;
using System;
using System.Collections.Generic;

namespace BDC.Core.Managers
{
    public interface ITransferManager
    {
        Transaction Get(Guid userId);
        Result<string> Transfer(Transaction transaction);
        IEnumerable<Transaction> GetTransferHistory(Guid? userId = null, TransactionStatus? status = null);
    }
}