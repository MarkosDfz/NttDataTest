using MediatR;
using Microsoft.AspNetCore.Mvc;
using NttDataTest.Services.Account.Commands;
using NttDataTest.Services.Account.Queries;
using NttDataTest.Services.Proxies.Account.Transaction.Commands;
using System.Threading.Tasks;

namespace NttDataTest.Api.Account.Controllers
{
    [Route("api/cuentas")]
    [ApiController]
    public class CuentaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CuentaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] AccountGetAllParameters filter)
        {
            AccountGetAllQuery accountQuery = new()
            {
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                NumeroCuenta = filter.NumeroCuenta
            };

            return Ok(await _mediator.Send(accountQuery));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            return Ok(await _mediator.Send(new AccountGetByIdQuery { CuentaGuid = id }));
        }

        [HttpPost]
        public async Task<IActionResult> Create(AccountCreateCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, AccountUpdateCommand command)
        {
            if (id != command.CuentaGuid)
                return BadRequest();

            return Ok(await _mediator.Send(command));
        }

        [HttpPut("saldo/{id}")]
        public async Task<IActionResult> UpdateSaldoInical(string id, AccountInitialBalanceUpdateCommand command)
        {
            if (id != command.CuentaGuid)
                return BadRequest();

            return Ok(await _mediator.Send(command));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            return Ok(await _mediator.Send(new AccountDeleteCommand { CuentaGuid = id }));
        }

        [HttpGet("reportes")]
        public async Task<IActionResult> GetTransactionsReport([FromQuery] AccountTransactionReportParameters filter)
        {
            AccountGetTransactionReportQuery reportQuery = new()
            {
                CuentaGuid = filter.CuentaGuid,
                PageSize = filter.PageSize,
                FechaInicio = filter.FechaInicio,
                FechaFin = filter.FechaFin
            };

            return Ok(await _mediator.Send(reportQuery));
        }
    }
}
