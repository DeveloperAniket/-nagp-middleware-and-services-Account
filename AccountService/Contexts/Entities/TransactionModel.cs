using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace AccountService.Contexts.Entities
{
    public class TransactionModel
    {
        [Key]
        public Guid TransactionId { get; set; }
        public int FromAccount { get; set; }
        public int ToAccount { get; set; }
        public TransactionType TransactionType { get; set; }
        public decimal Amount { get; set; }
        public DateTimeOffset TransactionDateTime { get; set; }
        public AccountModel? FromAccountDetails { get; set; }
    }

    public enum TransactionType
    {
        Debit = 1,
        Credit = 2
    }
}
