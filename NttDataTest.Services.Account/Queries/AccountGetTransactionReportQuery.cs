using MediatR;
using NttDataTest.Services.Account.DTOs;
using NttDataTest.Services.Account.Exceptions;
using NttDataTest.Services.Account.Interfaces;
using NttDataTest.Services.Account.Wrappers;
using NttDataTest.Services.Proxies.Account.Client.Commands;
using NttDataTest.Services.Proxies.Account.Client;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NttDataTest.Services.Proxies.Account.Transaction;
using NttDataTest.Services.Proxies.Account.Transaction.Commands;
using NttDataTest.Domain.Account;

namespace NttDataTest.Services.Account.Queries
{
    public class AccountGetTransactionReportQuery : IRequest<PagedResponse<List<ReporteMovimientoDTO>>>
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public string CuentaGuid { get; set; }

        public string FechaInicio { get; set; }

        public string FechaFin { get; set; }
    }

    public class AccountGetTransactionEventHandler : IRequestHandler<AccountGetTransactionReportQuery, PagedResponse<List<ReporteMovimientoDTO>>>
    {
        private readonly IRepositoryAsync<ReporteMovimientoDTO> _repositoryAsync;
        private readonly IClientProxy _clientProxy;
        private readonly ITransactionProxy _transactionProxy;
        private readonly IRepositoryAsync<Cuenta> _repositoryCuentaAsync;

        public AccountGetTransactionEventHandler(
            IRepositoryAsync<ReporteMovimientoDTO> repositoryAsync,
            IClientProxy clientProxy,
            ITransactionProxy transactionProxy,
            IRepositoryAsync<Cuenta> repositoryCuentaAsync)
        {
            _repositoryAsync = repositoryAsync;
            _clientProxy = clientProxy;
            _transactionProxy = transactionProxy;
            _repositoryCuentaAsync = repositoryCuentaAsync;
        }

        public async Task<PagedResponse<List<ReporteMovimientoDTO>>> Handle(AccountGetTransactionReportQuery command, CancellationToken cancellationToken)
        {
            bool isvalidInitialDate = DateTime.TryParse(command.FechaInicio, out DateTime initialDate);

            if (!isvalidInitialDate)
                throw new ApiException("La fecha de inicio es inválida");

            bool isvalidEndDate = DateTime.TryParse(command.FechaFin, out DateTime endDate);

            if (!isvalidEndDate)
                throw new ApiException("La fecha de fin es inválida");

            bool guidCuentaValido = Guid.TryParse(command.CuentaGuid, out Guid cuentaGuid);

            if (!guidCuentaValido)
                throw new ApiException($"ClienteGuid contiene un valor inválido : {command.CuentaGuid}");

            Cuenta cuenta = await _repositoryCuentaAsync.GetByIdAsync(cuentaGuid);

            if (cuenta == null)
                throw new ApiException($"Registro no encontrado con el ID : {command.CuentaGuid}");

            var resultClientRemote = await _clientProxy.GetClienteAsync(new ClientGetByIdCommand { ClienteGuid = cuenta.ClienteGuid.ToString() });

            if (!resultClientRemote.procesoCorrecto || resultClientRemote.cliente == null)
                throw new ApiException(resultClientRemote.Error);

            ClienteRemoteResponse cliente = resultClientRemote.cliente;

            TransactionGetAllCommand transactionCommand = new()
            {
                CuentaGuid  = command.CuentaGuid,
                PageSize    = command.PageSize,
                FechaInicio = command.FechaInicio,
                FechaFin    = command.FechaFin,
            };

            var resultTransactionsRemote = await _transactionProxy.GetMovimientosAsync(transactionCommand);

            if (!resultTransactionsRemote.procesoCorrecto || resultTransactionsRemote.movimientos == null)
                throw new ApiException(resultTransactionsRemote.Error);

            List<MovimientoRemoteResponse> movimientos = resultTransactionsRemote.movimientos;

            List<ReporteMovimientoDTO> repMovimientos = new List<ReporteMovimientoDTO>();

            foreach (MovimientoRemoteResponse item in movimientos)
            {
                ReporteMovimientoDTO reporte = new()
                {
                    Cliente         = cliente.Nombre,
                    NumeroCuenta    = cuenta.NumeroCuenta,
                    EstadoCuenta    = cliente.Estado,
                    TipoCuenta      = cuenta.TipoCuenta.ToString(),
                    FechaMovimiento = item.Fecha.ToString("dd/MM/yyyy"),
                    TipoMovimiento  = item.TipoMovimiento,
                    ValorMovimiento = item.ValorMovimiento,
                    SaldoDisponible = item.SaldoDisponible
                };

                repMovimientos.Add(reporte);
            }
            
            return new PagedResponse<List<ReporteMovimientoDTO>>(repMovimientos, command.PageNumber, command.PageSize);
        }
    }
}
