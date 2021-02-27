using BDC.Core.Models;
using BDC.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BDC.Core.Managers
{
    public class UserManager : IUserManager
    {
        private readonly DataContext<User> _userContext;
        private readonly DataContext<Account> _accountContext;

        public UserManager()
        {
            _userContext = new DataContext<User>();
            _accountContext = new DataContext<Account>();
        }

        public User Get(Guid userId)
        {
            return _userContext.GetById(userId);
        }

        public IEnumerable<User> GetAll()
        {
            return _userContext.GetAll();
        }

        public Result<User> Login(string email, string pwd)
        {
            var user = _userContext.GetAll().FirstOrDefault(a => a.EmailAddress.ToLower() == email.ToLower());
            if (user == null)
                return Result<User>.Failure("Email address or password is incorrect");

            if (user.Password != pwd)
                return Result<User>.Failure("Email address or password is incorrect");

            user.LastLogin = DateTime.Now;
            _userContext.Update(user);

            return Result<User>.Success(user, "Login successful");
        }

        public Result<bool> Create(User user, Account account)
        {
            if (string.IsNullOrEmpty(user.Name)) return Result<bool>.Failure("Full name is required");
            if (string.IsNullOrEmpty(user.EmailAddress)) return Result<bool>.Failure("Email address is required");
            if (string.IsNullOrEmpty(user.Password)) return Result<bool>.Failure("Password is required");
            if (string.IsNullOrEmpty(account.AccountNumber)) return Result<bool>.Failure("Bank account number is required");


            // validate
            var users = _userContext.GetAll();
            if (users.FirstOrDefault(a => a.EmailAddress.ToLower() == user.EmailAddress.ToLower()) != null)
                return Result<bool>.Failure($"Sorry, the email address '{user.EmailAddress}' already exist");

            if (users.FirstOrDefault(a => a.MobileNumber == user.MobileNumber) != null)
                return Result<bool>.Failure($"Sorry, the mobile number '{user.MobileNumber}' already exist");

            var accounts = _accountContext.GetAll();
            if (accounts.FirstOrDefault(a => a.BankCode == account.BankCode && a.AccountNumber == account.AccountNumber) != null)
                return Result<bool>.Failure($"The supplied account number already exist");

            var userId = _userContext.Add(user);
            if (userId == Guid.Empty)
                return Result<bool>.Failure("Unable to create account at the moment");

            account.UserId = userId;
            var acc = _accountContext.Add(account);

            return Result<bool>.Success("Account created successfully");
        }
    }
}
