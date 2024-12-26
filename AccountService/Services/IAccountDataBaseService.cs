using AccountService.Contexts.Entities;
using DatabaseContext.Contexts.Entities;

namespace AccountService.Services
{
    public interface IAccountDataBaseService
    {
        public AccountModel? CreateNewAccount(AccountModel account);
        AccountModel? GetStatementDetails(int accountNumber);
    }
}
