namespace AccountService.Contexts.Entities
{
    public class StatementResponseDto
    {
        public int AccountNumber { get; set; }

        public string? Name { get; set; }

        public string? AccountType { get; set; }

        public double AccountBalance { get; set; }

        public List<TransactionDetailDto>? StatementDetails { get; set; }
    }

    public class TransactionDetailDto
    {
        public Guid TransactionId { get; set; }
        public DateTimeOffset TransactionDateTime { get; set; }
        public int ToAccount { get; set; }
        public string? TransactionType { get; set; }
        public double Amount { get; set; }
    }
}
