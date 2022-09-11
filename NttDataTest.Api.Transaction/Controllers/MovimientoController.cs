using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NttDataTest.Services.Transaction.Commands;
using NttDataTest.Services.Transaction.Queries;
using System.Threading.Tasks;

namespace NttDataTest.Api.Transaction.Controllers
{
    [Route("api/movimientos")]
    [ApiController]
    public class MovimientoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MovimientoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] TransactionGetAllParameters filter)
        {
            TransactionGetAllQuery clientQuery = new()
            {
                PageNumber  = filter.PageNumber,
                PageSize    = filter.PageSize,
                CuentaGuid  = filter.CuentaGuid,
                FechaInicio = filter.FechaInicio,
                FechaFin    = filter.FechaFin
            };

            return Ok(await _mediator.Send(clientQuery));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            return Ok(await _mediator.Send(new TransactionGetByIdQuery { MovimientoGuid = id }));
        }

        [HttpPost]
        public async Task<IActionResult> Create(TransactionCreateCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}
