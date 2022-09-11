using MediatR;
using NttDataTest.Domain.Account;
using NttDataTest.Services.Account.Exceptions;
using NttDataTest.Services.Account.Interfaces;
using NttDataTest.Services.Account.Wrappers;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using static NttDataTest.Common.Account.Enums;

namespace NttDataTest.Services.Account.Commands
{
    public class AccountUpdateCommand : IRequest<Response<Cuenta>>
    {
        public string CuentaGuid { get; set; }

        public CuentaType TipoCuenta { get; set; }

        public bool Estado { get; set; }
    }

    public class AccountUpdateEventHandler : IRequestHandler<AccountUpdateCommand, Response<Cuenta>>
    {
        private IRepositoryAsync<Cuenta> _repositoryAsync;

        public AccountUpdateEventHandler(IRepositoryAsync<Cuenta> repositoryAsync)
        {
            _repositoryAsync = repositoryAsync;
        }

        public async Task<Response<Cuenta>> Handle(AccountUpdateCommand command, CancellationToken cancellationToken)
        {
            bool guidValido = Guid.TryParse(command.CuentaGuid, out Guid cuentaGuid);

            if (!guidValido)
                throw new ApiException($"CuentaGuid contiene un valor inválido : {command.CuentaGuid}");

            Cuenta cuenta = await _repositoryAsync.GetByIdAsync(cuentaGuid);

            if (cuenta == null)
                throw new KeyNotFoundException($"Cuenta no encontrada con el ID: {command.CuentaGuid}");

            cuenta.TipoCuenta = command.TipoCuenta;
            cuenta.Estado = command.Estado;

            await _repositoryAsync.UpdateAsync(cuenta);

            return new Response<Cuenta>(cuenta);
        }
    }
}
