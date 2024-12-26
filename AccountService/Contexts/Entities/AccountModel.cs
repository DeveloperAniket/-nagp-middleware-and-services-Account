using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace AccountService.Contexts.Entities
{
    public class AccountModel
    {
        [Key]
        public int AccountNumber { get; set; }
        public string AccountType { get; set; }
        public string Name { get; set; } 
        public decimal Balance { get; set; }

        public ICollection<TransactionModel> Transactions { get; }
    }
}
