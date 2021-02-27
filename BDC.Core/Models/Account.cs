using System;

namespace BDC.Core.Models
{
    public class Account : BaseModel
    {
        public Guid UserId { get; set; }
        public string BankCode { get; set; }
        public string BankName { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string Currency { get; set; } = "NGN";
        public AccountStatus Status { get; set; }
    }

    public enum AccountStatus
    {
        Unknown = 0,
        Active = 1,
        Inactive = 2,
        Blacklisted = 3
    }
}
