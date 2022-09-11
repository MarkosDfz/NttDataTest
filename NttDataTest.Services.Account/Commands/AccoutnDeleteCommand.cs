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
    public class AccountDeleteCommand : IRequest<Response<string>>
    {
        public string CuentaGuid { get; set; }
    }

    public class AccountDeleteEventHandler : IRequestHandler<AccountDeleteCommand, Response<string>>
    {
        private readonly IRepositoryAsync<Cuenta> _repositoryAsync;

        public AccountDeleteEventHandler(IRepositoryAsync<Cuenta> repositoryAsync)
        {
            _repositoryAsync = repositoryAsync;
        }

        public async Task<Response<string>> Handle(AccountDeleteCommand command, CancellationToken cancellationToken)
        {
            bool guidValido = Guid.TryParse(command.CuentaGuid, out Guid cuentaGuid);

            if (!guidValido)
                throw new ApiException($"CuentaGuid contiene un valor inválido : {command.CuentaGuid}");

            Cuenta cuenta = await _repositoryAsync.GetByIdAsync(cuentaGuid);

            if (cuenta == null)
                throw new KeyNotFoundException($"Registro no encontrado con el ID: {command.CuentaGuid}");

            await _repositoryAsync.DeleteAsync(cuenta);

            return new Response<string>($"Cuenta {cuenta.NumeroCuenta} eliminada con éxito");
        }
    }
}
