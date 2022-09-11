using MediatR;
using NttDataTest.Domain.Account;
using NttDataTest.Services.Account.Exceptions;
using NttDataTest.Services.Account.Interfaces;
using NttDataTest.Services.Account.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NttDataTest.Services.Account.Commands
{
    public class AccountInitialBalanceUpdateCommand : IRequest<Response<string>>
    {
        public string CuentaGuid { get; set; }

        public decimal NuevoSaldo { get; set; }
    }

    public class AccountInitialBalanceEventHandler : IRequestHandler<AccountInitialBalanceUpdateCommand, Response<string>>
    {
        private readonly IRepositoryAsync<Cuenta> _repositoryAsync;

        public AccountInitialBalanceEventHandler(IRepositoryAsync<Cuenta> repositoryAsync)
        {
            _repositoryAsync = repositoryAsync;
        }

        public async Task<Response<string>> Handle(AccountInitialBalanceUpdateCommand command, CancellationToken cancellationToken)
        {
            bool guidValido = Guid.TryParse(command.CuentaGuid, out Guid cuentaGuid);

            if (!guidValido)
                throw new ApiException($"CuentaGuid contiene un valor inválido : {command.CuentaGuid}");

            Cuenta cuenta = await _repositoryAsync.GetByIdAsync(cuentaGuid);

            if (cuenta == null)
                throw new KeyNotFoundException($"Cuenta no encontrada con el ID: {command.CuentaGuid}");

            cuenta.SaldoInicial = command.NuevoSaldo;

            await _repositoryAsync.UpdateAsync(cuenta);

            return new Response<string>("Nuevo saldo ingresado correctamente", true);
        }
    }
}
