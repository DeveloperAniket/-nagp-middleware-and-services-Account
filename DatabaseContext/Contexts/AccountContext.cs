using DatabaseContext.Contexts.Entities;
 
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Xml.Linq;

namespace AccountService.Contexts
{
    public class AccountContext : DbContext
    {
        public AccountContext()
        {
            Database.EnsureCreated();
        }
        public DbSet<AccountModel> Accounts { get; set; }

        public DbSet<TransactionModel> Transactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "AccountDb");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<AccountModel>()
                .HasMany(x => x.Transactions)
                .WithOne(e => e.FromAccountDetails)
                .HasForeignKey(e => e.FromAccount)
                .HasPrincipalKey(e => e.AccountNumber);


            modelBuilder.Entity<AccountModel>().HasData(
             new AccountModel

             {
                 AccountNumber = 1,
                 AccountType = "Saving",
                 Balance = 1001,
                 Name = "Test Demo Name"
             },

              new AccountModel
              {
                  AccountNumber = 2,
                  AccountType = "Current",
                  Balance = 50,
                  Name = "Demo Name"
              }
         );

            modelBuilder.Entity<TransactionModel>().HasData(
            new TransactionModel { TransactionId = Guid.NewGuid(), FromAccount = 2, ToAccount = 1, Amount = 500, TransactionDateTime = new DateTimeOffset(DateTime.Parse("2024-05-12 14:40:52")), TransactionType = TransactionType.Debit },
            new TransactionModel { TransactionId = Guid.NewGuid(), FromAccount = 2, ToAccount = 1, Amount = 1500, TransactionDateTime = new DateTimeOffset(DateTime.Parse("2024-07-12 02:05:52")), TransactionType = TransactionType.Debit },
            new TransactionModel { TransactionId = Guid.NewGuid(), FromAccount = 1, ToAccount = 2, Amount = 2000, TransactionDateTime = new DateTimeOffset(DateTime.Parse("2024-12-20 01:01:02")), TransactionType = TransactionType.Debit }
            );
        }
    }
}
