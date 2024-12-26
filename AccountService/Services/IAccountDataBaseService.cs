using AccountService.Contexts.Entities;

namespace AccountService.Services
{
    public interface IAccountDataBaseService
    {
        public AccountModel? CreateNewAccount(AccountModel account);
    }
}
