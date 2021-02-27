using BDC.Core.Models;
using System;
using System.Collections;
using System.Collections.Generic;

namespace BDC.Core.Managers
{
    public interface IUserManager
    {
        User Get(Guid userId);
        IEnumerable<User> GetAll();
        Result<bool> Create(User user, Account account);
        Result<User> Login(string email, string pwd);
    }
}