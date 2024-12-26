using AccountService.Contexts.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Contexts
{
    public class AccountContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "AccountDb");

        }
        public DbSet<AccountModel> Accounts { get; set; }
    }
}
