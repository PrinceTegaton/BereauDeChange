using BDC.Core.Models;
using System;
using System.Collections.Generic;

namespace BDC.Core.Managers
{
    public interface IUserManager
    {
        User Get(Guid userId);
        IEnumerable<User> GetAll();
        Result<Guid> Create(User user, Account account);
        Result<User> Login(string email, string pwd);
    }
}