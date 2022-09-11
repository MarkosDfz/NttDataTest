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

namespace NttDataTest.Services.Client.Queries
{
    public class ClienteGetAllQuery : IRequest<PagedResponse<List<ClienteDTO>>>
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public string Nombre { get; set; }
    }

    public class ClientGetAllEventHandler : IRequestHandler<ClienteGetAllQuery, PagedResponse<List<ClienteDTO>>>
    {
        private readonly IRepositoryAsync<Cliente> _repositoryAsync;
        private readonly IMapper _mapper;

        public ClientGetAllEventHandler(IRepositoryAsync<Cliente> repositoryAsync, IMapper mapper)
        {
            _repositoryAsync = repositoryAsync;
            _mapper = mapper;
        }

        public async Task<PagedResponse<List<ClienteDTO>>> Handle(ClienteGetAllQuery command, CancellationToken cancellationToken)
        {
            var specification = new ClientPagedSpecification(command.PageSize, command.PageNumber, command.Nombre);

            List<Cliente> clientes = await _repositoryAsync.ListAsync(specification);

            List<ClienteDTO> clientesDto = _mapper.Map<List<ClienteDTO>>(clientes);

            return new PagedResponse<List<ClienteDTO>>(clientesDto, command.PageNumber, command.PageSize);
        }
    }
}
