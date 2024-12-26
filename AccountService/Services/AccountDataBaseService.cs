using AccountService.Contexts;
using AccountService.Contexts.Entities;
using DatabaseContext.Contexts.Entities;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using System;

namespace AccountService.Services
{
    public class AccountDataBaseService : Statement.StatementBase, IAccountDataBaseService
    {
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


        public AccountModel? GetStatementDetails(int accountNumber)
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

        //public override Task<StatementResponse> GetStatement(StatementDetails request, ServerCallContext context)
        //{
        //    var result = GetStatementDetails(request.AccountNumber);


        //    var response = new StatementResponse()
        //    {
        //        Name = result.Name,
        //        AccountType = result.AccountType,
        //        AccountBalance = result.Balance,
        //        Name = result.Name,
        //        StatementDetails = new List<TransactionDetailDto>()
        //    };
        //    foreach (var transaction in result.Transactions ?? [])
        //    {
        //        var transactionDetail = new TransactionDetailDto()
        //        {
        //            Amount = transaction.Amount,
        //            ToAccount = transaction.ToAccount,
        //            TransactionDateTime = transaction.TransactionDateTime,
        //            TransactionId = transaction.TransactionId,
        //            TransactionType = transaction.TransactionType.ToString()
        //        };
        //        response.StatementDetails.Add(transactionDetail);
        //    }
        //    return Task.FromResult(response);
        //}

        public override Task<StatementResponse> GetStatement(StatementRequest request, ServerCallContext context)
        {
            var result = GetStatementDetails(request.AccountNumber);
            if (result != null)
            {
                var response = new StatementDetail()
                {
                    AccountNumber = result.AccountNumber,
                    AccountType = result.AccountType,
                    AccountBalance = result.Balance,
                    Name = result.Name,

                };

                foreach (var transaction in result.Transactions ?? [])
                {
                    var transactionDetail = new TransactionDetail()
                    {
                        Amount = transaction.Amount,
                        ToAccount = transaction.ToAccount,
                        TransactionDatetime = transaction.TransactionDateTime.ToString(),
                        TransactionId = transaction.TransactionId.ToString(),
                        TransactionType = transaction.TransactionType.ToString()
                    };
                    response.TransactionDetails.Add(transactionDetail);
                }

                var responseFinal = new StatementResponse();
                responseFinal.Statementdetail = response;
                return Task.FromResult(responseFinal);
            }

            return Task.FromResult(new StatementResponse());

        }
    }
}
