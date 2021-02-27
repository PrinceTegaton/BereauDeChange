using System;

namespace BDC.Core.Models
{
    public class Transaction : BaseModel
    {
        public Guid UserId { get; set; }
        public string SourceBankCode { get; set; }
        public string SourceAccountNumber { get; set; }

        public string DestinationAccountNumber { get; set; }
        public string DestinationAccountName { get; set; }

        public string SourceCurrency { get; set; }
        public string DestinationCurrency { get; set; }

        public string SwiftCode { get; set; }
        public string Country { get; set; }
        public decimal Amount { get; set; }
        public decimal Fee { get; set; }
        public string Email { get; set; }

        public decimal Rate { get; set; }
        public string AuthMethod { get; set; }
        public string DebitResponse { get; set; }
        public string CreditResponse { get; set; }
        public TransactionStatus Status { get; set; }
    }

    public enum TransactionStatus
    {
        Unknown = 0,
        Pending = 1,
        Processing = 2,
        Successful = 3,
        Failed = 4
    }
}
