using AccountService.Contexts.Entities;
using AccountService.Dtos;
using AccountService.Services;
using DatabaseContext.Contexts.Entities;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using SharedProject;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AccountService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountDataBaseService _accountDataBaseService;
        private readonly IRabbitMqService _rabbitMqService;
        public AccountsController(IAccountDataBaseService accountDataBaseService, IRabbitMqService rabbitMqService)
        {
            _accountDataBaseService = accountDataBaseService ?? throw new ArgumentNullException("accountDataBaseService");
            _rabbitMqService = rabbitMqService ?? throw new ArgumentNullException("rabbitMqService");
        }

        [HttpGet]
        [Route("statement/{accountNumber}")]
        public IActionResult Get(int accountNumber)
        {
            var result = _accountDataBaseService.GetStatementDetails(accountNumber);


            if (result == null || result.AccountNumber != accountNumber)
            {
                return BadRequest("Account Not Found");
            }

            var response = new StatementResponseDto()
            {
                AccountNumber = result.AccountNumber,
                AccountType = result.AccountType,
                AccountBalance = result.Balance,
                Name = result.Name,
                StatementDetails = new List<TransactionDetailDto>()
            };
            foreach (var transaction in result.Transactions ?? [])
            {
                var transactionDetail = new TransactionDetailDto()
                {
                    Amount = transaction.Amount,
                    ToAccount = transaction.ToAccount,
                    TransactionDateTime = transaction.TransactionDateTime,
                    TransactionId = transaction.TransactionId,
                    TransactionType = transaction.TransactionType.ToString()
                };
                response.StatementDetails.Add(transactionDetail);
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("statement/pdf/{accountNumber}")]
        public IActionResult RequestPdfStatement(int accountNumber)
        {
            var result = _accountDataBaseService.GetStatementDetails(accountNumber);

            if (result != null && result.AccountNumber == accountNumber)
            {
                var requestId = Guid.NewGuid();
                var request = new CreatePdfGenerateEventRequest() { AccountNumber = accountNumber, RequestId = requestId };
                _rabbitMqService.RaisePDFGenerateRequest(request);
                return Ok(request);
            }
            else
            {
                return BadRequest("Account Not Found");
            }
        }


        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> PostAsync([FromBody] CreateAccountRequestDto accountRequest)
        {
            var acc = new AccountModel()
            {
                AccountType = accountRequest.AccountType.ToString(),
                Name = accountRequest.FullName
            };

            var result = _accountDataBaseService.CreateNewAccount(acc);
            if (result != null)
            {
                var eventReq = new CreateAccountEventRequest()
                {
                    FullName = result.Name,
                    AccountNumber = result.AccountNumber,
                    AccountType = result.AccountType
                };

                var eventResult = await _rabbitMqService.RaiseCreateAccount(eventReq);

                var response = new CreateAccountRespeonseDto()
                {
                    IsSuccess = true,
                    AccountNumber = result.AccountNumber
                };
                return Ok(response);

            }
            else
            {
                return BadRequest();
            }
        }
    }
}
