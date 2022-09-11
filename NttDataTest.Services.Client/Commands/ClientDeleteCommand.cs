using AutoMapper;
using MediatR;
using NttDataTest.Domain.Client;
using NttDataTest.Services.Client.Interfaces;
using NttDataTest.Services.Client.Specifications;
using NttDataTest.Services.Client.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static NttDataTest.Services.Client.Commands.ClientUpdateEventHandler;

namespace NttDataTest.Services.Client.Commands
{
    public class ClientDeleteCommand : IRequest<Response<int>>
    {
        public string ClienteGuid { get; set; }
    }

    public class ClientDeleteEventHandler : IRequestHandler<ClientDeleteCommand, Response<int>>
    {
        private readonly IRepositoryAsync<Cliente> _repositoryAsync;

        public ClientDeleteEventHandler(IRepositoryAsync<Cliente> repositoryAsync)
        {
            _repositoryAsync = repositoryAsync;
        }

        public async Task<Response<int>> Handle(ClientDeleteCommand command, CancellationToken cancellationToken)
        {
            var specification = new ClientByGuiSpecification(command.ClienteGuid);

            Cliente cliente = await _repositoryAsync.GetBySpecAsync(specification);

            if (cliente == null)
                throw new KeyNotFoundException($"Cliente no encontrado con el ID: {command.ClienteGuid}");

            await _repositoryAsync.DeleteAsync(cliente);

            return new Response<int>(cliente.Id);
        }
    }
}
