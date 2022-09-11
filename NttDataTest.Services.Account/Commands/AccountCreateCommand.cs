using AutoMapper;
using MediatR;
using NttDataTest.Domain.Account;
using NttDataTest.Services.Account.Exceptions;
using NttDataTest.Services.Account.Interfaces;
using NttDataTest.Services.Account.Specifications;
using NttDataTest.Services.Account.Wrappers;
using NttDataTest.Services.Proxies.Account.Client;
using NttDataTest.Services.Proxies.Account.Client.Commands;
using System;
using System.Threading;
using System.Threading.Tasks;
using static NttDataTest.Common.Account.Enums;

namespace NttDataTest.Services.Account.Commands
{
    public class AccountCreateCommand : IRequest<Response<string>>
    {
        public string NumeroCuenta { get; set; }

        public CuentaType TipoCuenta { get; set; }

        public decimal? SaldoInicial { get; set; }

        public bool Estado { get; set; }

        public string ClienteGuid { get; set; }
    }

    public class AccountCreateEventHandler : IRequestHandler<AccountCreateCommand, Response<string>>
    {
        private readonly IRepositoryAsync<Cuenta> _repositoryAsync;
        private readonly IMapper _mapper;
        private readonly IClientProxy _clienttProxy;

        public AccountCreateEventHandler(
            IRepositoryAsync<Cuenta> repositoryAsync,
            IMapper mapper,
            IClientProxy clientProxy)
        {
            _repositoryAsync = repositoryAsync;
            _mapper = mapper;
            _clienttProxy = clientProxy;
        }

        public async Task<Response<string>> Handle(AccountCreateCommand command, CancellationToken cancellationToken)
        {
            bool guidValido = Guid.TryParse(command.ClienteGuid, out Guid clienteGui);

            if (!guidValido)
                throw new ApiException($"ClienteGuid contiene un valor inválido : {command.ClienteGuid}");

            var resultClientRemote = await _clienttProxy.GetClienteAsync(new ClientGetByIdCommand { ClienteGuid = command.ClienteGuid });

            if (!resultClientRemote.procesoCorrecto || resultClientRemote.cliente == null)
                throw new ApiException(resultClientRemote.Error);

            ClienteRemoteResponse cliente = resultClientRemote.cliente;

            var specification = new AccountByNumberSpecification(command.NumeroCuenta);

            var cuentaExistente = await _repositoryAsync.GetBySpecAsync(specification);

            if (cuentaExistente != null)
                throw new ApiException($"Ya existe una cuenta con el número: {command.NumeroCuenta}");

            command.SaldoInicial ??= 0M;

            Cuenta nuevoRegistro = _mapper.Map<Cuenta>(command);

            Cuenta cuenta = await _repositoryAsync.AddAsync(nuevoRegistro);

            return new Response<string>($"Cuenta {cuenta.NumeroCuenta} creada con éxito");
        }
    }
}
