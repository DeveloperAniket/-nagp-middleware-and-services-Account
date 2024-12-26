namespace AccountService.Contexts.Entities
{
    public class StatementResponse
    {
        public int AccountNumber { get; set; }

        public string? Name { get; set; }

        public string? AccountType { get; set; }

        public decimal AccountBalance { get; set; }

        public List<TransactionDetails>? StatementDetails { get; set; }
    }

    public class TransactionDetails
    {
        public Guid TransactionId { get; set; }
        public DateTimeOffset TransactionDateTime { get; set; }
        public int ToAccount { get; set; }
        public string? TransactionType { get; set; }
        public decimal Amount { get; set; }
    }
}
