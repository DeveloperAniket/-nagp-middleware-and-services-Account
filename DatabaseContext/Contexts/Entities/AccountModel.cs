using System.ComponentModel.DataAnnotations;

#nullable disable

namespace DatabaseContext.Contexts.Entities
{
    public class AccountModel
    {
        [Key]
        public int AccountNumber { get; set; }
        public string AccountType { get; set; }
        public string Name { get; set; }
        public double Balance { get; set; }

        public ICollection<TransactionModel> Transactions { get; }
    }
}
