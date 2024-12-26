namespace AccountService.Dtos
{
    public class CreateAccountRequestDto
    {
        public required string FullName { get; set; }

        public AccountType AccountType { get; set; }
    }
}
