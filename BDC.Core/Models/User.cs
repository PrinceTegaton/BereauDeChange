using System;

namespace BDC.Core.Models
{
    public class User : BaseModel
    {
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string MobileNumber { get; set; }
        public UserStatus Status { get; set; }
        public string Password { get; set; }
        public DateTime LastLogin { get; set; }
        public bool IsAdmin { get; set; }
    }

    public enum UserStatus
    {
        Unknown = 0,
        Active = 1,
        Inactive = 2
    }
}
