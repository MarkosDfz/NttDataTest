using MediatR;
using NttDataTest.Domain.Account;
using NttDataTest.Services.Account.Exceptions;
using NttDataTest.Services.Account.Interfaces;
using NttDataTest.Services.Account.Wrappers;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NttDataTest.Services.Account.Queries
{
    public class AccountGetByIdQuery : IRequest<Response<Cuenta>>
    {
        public string CuentaGuid { get; set; }
    }

    public class AccountGetByIdEventHandler : IRequestHandler<AccountGetByIdQuery, Response<Cuenta>>
    {
        private readonly IRepositoryAsync<Cuenta> _repositoryAsync;

        public AccountGetByIdEventHandler(IRepositoryAsync<Cuenta> repositoryAsync)
        {
            _repositoryAsync = repositoryAsync;
        }

        public async Task<Response<Cuenta>> Handle(AccountGetByIdQuery command, CancellationToken cancellationToken)
        {
            bool guidValido = Guid.TryParse(command.CuentaGuid, out Guid cuentaGuid);

            if (!guidValido)
                throw new ApiException($"CuentaGuid contiene un valor inválido : {command.CuentaGuid}");

            Cuenta cuenta = await _repositoryAsync.GetByIdAsync(cuentaGuid);

            if (cuenta == null)
                throw new KeyNotFoundException($"Cuenta no encontrada con el ID: {command.CuentaGuid}");

            return new Response<Cuenta>(cuenta);
        }
    }
}
