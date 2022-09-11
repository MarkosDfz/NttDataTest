using AutoMapper;
using MediatR;
using NttDataTest.Common.Transaction;
using NttDataTest.Domain.Transaction;
using NttDataTest.Services.Proxies.Transaction.Account;
using NttDataTest.Services.Proxies.Transaction.Account.Commands;
using NttDataTest.Services.Transaction.Exceptions;
using NttDataTest.Services.Transaction.Interfaces;
using NttDataTest.Services.Transaction.Wrappers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NttDataTest.Services.Transaction.Commands
{
    public class TransactionCreateCommand : IRequest<Response<string>>
    {
        public string CuentaGuid { get; set; }

        public TipoMovimientoType TipoMovimiento { get; set; }

        public decimal ValorMovimiento { get; set; }
    }

    public class TransactionCreateEventHandler : IRequestHandler<TransactionCreateCommand, Response<string>>
    {
        private readonly IRepositoryAsync<Movimiento> _repositoryAsync;
        private readonly IMapper _mapper;
        private readonly IAccountProxy _accountProxy;
        private readonly IAccountUpdateInitialBalanceProxy _accountUpdateBalanceProxy;

        public TransactionCreateEventHandler(
            IRepositoryAsync<Movimiento> repositoryAsync,
            IMapper mapper,
            IAccountProxy accountProxy,
            IAccountUpdateInitialBalanceProxy accountUpdateBalanceProxy)
        {
            _repositoryAsync = repositoryAsync;
            _mapper = mapper;
            _accountProxy = accountProxy;
            _accountUpdateBalanceProxy = accountUpdateBalanceProxy;
        }

        public async Task<Response<string>> Handle(TransactionCreateCommand command, CancellationToken cancellationToken)
        {
            bool guidValido = Guid.TryParse(command.CuentaGuid, out Guid cuentaGuid);

            if (!guidValido)
                throw new ApiException($"CuentaGuid contiene un valor inválido : {command.CuentaGuid}");

            var resultAccountRemote = await _accountProxy.GetCuentaAsync(new AccountGetByIdCommand { CuentaGuid = command.CuentaGuid });

            if (!resultAccountRemote.procesoCorrecto || resultAccountRemote.cuenta == null)
                throw new ApiException(resultAccountRemote.Error);

            CuentaRemoteResponse cuenta = resultAccountRemote.cuenta;

            Movimiento nuevoRegistro = _mapper.Map<Movimiento>(command);

            if (command.TipoMovimiento == TipoMovimientoType.Deposito)
            {
                if (command.ValorMovimiento < 0)
                    throw new ApiException("Si el tipo de movimiento es Deposito el valor debe ser positivo");

                nuevoRegistro.SaldoDisponible = cuenta.SaldoInicial;

                nuevoRegistro.SaldoDisponible += nuevoRegistro.ValorMovimiento;
            }
            else
            {
                if (command.ValorMovimiento > 0)
                    throw new ApiException("Si el tipo de movimiento es Retiro el valor debe ser negativo");

                if (cuenta.SaldoInicial == 0)
                    throw new ApiException("Saldo no disponible para esta transacción");

                if (cuenta.SaldoInicial < Math.Abs(nuevoRegistro.ValorMovimiento))
                    throw new ApiException("Saldo insuficiente para esta transacción");

                nuevoRegistro.SaldoDisponible = cuenta.SaldoInicial;

                nuevoRegistro.SaldoDisponible += nuevoRegistro.ValorMovimiento;
            }

            nuevoRegistro.Fecha = DateTime.Now;

            AccountUpdateInitialBalanceCommand initialBalanceProxy = new()
            {
                CuentaGuid = command.CuentaGuid,
                NuevoSaldo = nuevoRegistro.SaldoDisponible
            };

            var resultAccountUpdateBalanceRemote = await _accountUpdateBalanceProxy.UpdateCuentaSaldoInicialAsync(initialBalanceProxy);

            if (!resultAccountUpdateBalanceRemote.procesoCorrecto)
                throw new ApiException(resultAccountRemote.Error);

            Movimiento movimiento = await _repositoryAsync.AddAsync(nuevoRegistro);

            return new Response<string>($"Movimiento {movimiento.MovimientoGuid} creado con éxito", true);
        }
    }
}
