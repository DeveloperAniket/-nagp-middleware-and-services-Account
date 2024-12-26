namespace AccountService.Dtos
{
    public class CreateAccountEventRequest
    {
        public required string FullName { get; set; }

        public required string AccountType { get; set; }

        public int AccountNumber { get; set; }
    }
}
