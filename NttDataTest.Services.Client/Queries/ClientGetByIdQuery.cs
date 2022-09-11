using AutoMapper;
using MediatR;
using NttDataTest.Domain.Client;
using NttDataTest.Services.Client.Commands;
using NttDataTest.Services.Client.DTOs;
using NttDataTest.Services.Client.Interfaces;
using NttDataTest.Services.Client.Specifications;
using NttDataTest.Services.Client.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NttDataTest.Services.Client.Queries
{
    public class ClientGetByIdQuery : IRequest<Response<ClienteDTO>>
    {
        public string ClienteGuid { get; set; }
    }

    public class ClientGetByIdEventHandler : IRequestHandler<ClientGetByIdQuery, Response<ClienteDTO>>
    {
        private readonly IRepositoryAsync<Cliente> _repositoryAsync;
        private readonly IMapper _mapper;

        public ClientGetByIdEventHandler(IRepositoryAsync<Cliente> repositoryAsync, IMapper mapper)
        {
            _repositoryAsync = repositoryAsync;
            _mapper = mapper;
        }

        public async Task<Response<ClienteDTO>> Handle(ClientGetByIdQuery command, CancellationToken cancellationToken)
        {
            var specification = new ClientByGuiSpecification(command.ClienteGuid);

            Cliente cliente = await _repositoryAsync.GetBySpecAsync(specification);

            if (cliente == null)
            {
                throw new KeyNotFoundException($"Registro no encontrado con el ID: {command.ClienteGuid}");
            }

            ClienteDTO clienteDto = _mapper.Map<ClienteDTO>(cliente);

            return new Response<ClienteDTO>(clienteDto);
        }
    }
}
