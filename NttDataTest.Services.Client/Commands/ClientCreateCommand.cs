using AutoMapper;
using MediatR;
using NttDataTest.Domain.Client;
using NttDataTest.Services.Client.Interfaces;
using NttDataTest.Services.Client.Wrappers;
using System;
using System.Threading;
using System.Threading.Tasks;
using static NttDataTest.Common.Client.Enums;

namespace NttDataTest.Services.Client.Commands
{
    public class ClientCreateCommand : IRequest<Response<string>>
    {
        public string Nombre { get; set; }

        public GeneroType Genero { get; set; }

        public int Edad { get; set; }

        public string Identificacion { get; set; }

        public string Direccion { get; set; }

        public string Telefono { get; set; }

        public string Contrasenia { get; set; }

        public bool Estado { get; set; }

        public string ClienteGuid { get; set; }
    }

    public class ClientCreateEventHandler : IRequestHandler<ClientCreateCommand, Response<string>>
    {
        private readonly IRepositoryAsync<Cliente> _repositoryAsync;
        private readonly IMapper _mapper;

        public ClientCreateEventHandler(IRepositoryAsync<Cliente> repositoryAsync, IMapper mapper)
        {
            _repositoryAsync = repositoryAsync;
            _mapper = mapper;
        }

        public async Task<Response<string>> Handle(ClientCreateCommand command, CancellationToken cancellationToken)
        {
            Guid guid = Guid.NewGuid();

            command.ClienteGuid = guid.ToString();

            Cliente nuevoRegistro = _mapper.Map<Cliente>(command);

            Cliente cliente = await _repositoryAsync.AddAsync(nuevoRegistro);

            return new Response<string>($"Cliente {cliente.ClienteGuid} creado con éxito", true);
        }
    }
}
