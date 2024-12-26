using AccountService.Contexts;
using AccountService.Contexts.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace AccountService.Services
{
    public class AccountDataBaseService : IAccountDataBaseService
    {
        public AccountDataBaseService()
        {
            //LoadData();


            //void LoadData()
            //{
            //    using (var context = new AccountContext())
            //    {
            //        var accountModel = new AccountModel() {   AccountNumber = 1, AccountType = "Saving", Balance = 1001, Name = "Test Demo Name" };
            //        context.Accounts.Add(accountModel);
            //        var accountModel1 = new AccountModel() {  AccountNumber = 2, AccountType = "Current", Balance = 50, Name = "Demo Name" };
            //        context.Accounts.Add(accountModel1);
            //        context.SaveChanges();
            //    }
            //}
        }
        public AccountModel? CreateNewAccount(AccountModel account)
        {
            try
            {
                using (var context = new AccountContext())
                {
                    context.Accounts.Add(account);

                    context.SaveChanges();
                    return account;
                }
            }
            catch (Exception ex)
            {
                // Log  exception;
                return null;
            }

        }


        public AccountModel? GetStatement(int accountNumber)
        {
            try
            {
                using (var context = new AccountContext())
                {
                    var details = context.Accounts.Where(x => x.AccountNumber == accountNumber).Include(x => x.Transactions).FirstOrDefault();

                    return details;
                }
            }
            catch (Exception ex)
            {
                // Log  exception;
                return null;
            }

        }
    }
}
