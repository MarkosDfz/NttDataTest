using AutoMapper;
using MediatR;
using NttDataTest.Domain.Client;
using NttDataTest.Services.Client.DTOs;
using NttDataTest.Services.Client.Interfaces;
using NttDataTest.Services.Client.Specifications;
using NttDataTest.Services.Client.Wrappers;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using static NttDataTest.Common.Client.Enums;

namespace NttDataTest.Services.Client.Commands
{
    public class ClientUpdateCommand : IRequest<Response<ClienteDTO>>
    {
        public string Nombre { get; set; }

        public GeneroType Genero { get; set; }

        public int Edad { get; set; }

        public string Identificacion { get; set; }

        public string Direccion { get; set; }

        public string Telefono { get; set; }

        public bool Estado { get; set; }

        public string ClienteGuid { get; set; }
    }

    public class ClientUpdateEventHandler : IRequestHandler<ClientUpdateCommand, Response<ClienteDTO>>
    {
        private readonly IRepositoryAsync<Cliente> _repositoryAsync;

        private readonly IMapper _mapper;

        public ClientUpdateEventHandler(IRepositoryAsync<Cliente> repositoryAsync, IMapper mapper)
        {
            _repositoryAsync = repositoryAsync;
            _mapper = mapper;
        }

        public async Task<Response<ClienteDTO>> Handle(ClientUpdateCommand command, CancellationToken cancellationToken)
        {
            var specification = new ClientByGuiSpecification(command.ClienteGuid);

            Cliente cliente = await _repositoryAsync.GetBySpecAsync(specification);

            if (cliente == null)
                throw new KeyNotFoundException($"Cliente no encontrado con el ID: {command.ClienteGuid}");

            cliente.Nombre = command.Nombre;
            cliente.Genero = command.Genero;
            cliente.Edad = command.Edad;
            cliente.Identificacion = command.Identificacion;
            cliente.Direccion = command.Direccion;
            cliente.Telefono = command.Telefono;
            cliente.Estado = command.Estado;

            await _repositoryAsync.UpdateAsync(cliente);

            ClienteDTO clienteDto = _mapper.Map<ClienteDTO>(cliente);

            return new Response<ClienteDTO>(clienteDto);
        }
    }
}
