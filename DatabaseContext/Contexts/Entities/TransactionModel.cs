using System.ComponentModel.DataAnnotations;

namespace DatabaseContext.Contexts.Entities
{
    public class TransactionModel
    {
        [Key]
        public Guid TransactionId { get; set; }
        public int FromAccount { get; set; }
        public int ToAccount { get; set; }
        public TransactionType TransactionType { get; set; }
        public double Amount { get; set; }
        public DateTimeOffset TransactionDateTime { get; set; }
        public AccountModel? FromAccountDetails { get; set; }
    }

    public enum TransactionType
    {
        Debit = 1,
        Credit = 2
    }
}
