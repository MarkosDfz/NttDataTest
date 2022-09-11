using MediatR;
using Microsoft.AspNetCore.Mvc;
using NttDataTest.Services.Client.Commands;
using NttDataTest.Services.Client.Queries;
using System.Threading.Tasks;

namespace NttDataTest.Api.Client.Controllers
{
    [Route("api/clientes")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClienteController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] ClientGetAllParameters filter)
        {
            ClienteGetAllQuery clientQuery = new()
            {
                PageNumber = filter.PageNumber,
                PageSize   = filter.PageSize,
                Nombre     = filter.Nombre
            };

            return Ok(await _mediator.Send(clientQuery));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            return Ok(await _mediator.Send(new ClientGetByIdQuery { ClienteGuid = id }));
        }

        [HttpPost]
        public async Task<IActionResult> Create(ClientCreateCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, ClientUpdateCommand command)
        {
            if (id != command.ClienteGuid) return BadRequest();

            return Ok(await _mediator.Send(command));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            return Ok(await _mediator.Send(new ClientDeleteCommand { ClienteGuid = id }));
        }

    }
}
