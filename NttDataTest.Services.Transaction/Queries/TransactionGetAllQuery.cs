using AutoMapper;
using MediatR;
using NttDataTest.Domain.Transaction;
using NttDataTest.Services.Proxies.Transaction.Account;
using NttDataTest.Services.Proxies.Transaction.Account.Commands;
using NttDataTest.Services.Transaction.DTOs;
using NttDataTest.Services.Transaction.Exceptions;
using NttDataTest.Services.Transaction.Interfaces;
using NttDataTest.Services.Transaction.Specifications;
using NttDataTest.Services.Transaction.Wrappers;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NttDataTest.Services.Transaction.Queries
{
    public class TransactionGetAllQuery : IRequest<Response<List<MovimientoDto>>>
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public string CuentaGuid { get; set; }

        public string FechaInicio { get; set; }

        public string FechaFin { get; set; }
    }

    public class TransactionGetAllEventHandler : IRequestHandler<TransactionGetAllQuery, Response<List<MovimientoDto>>>
    {
        private readonly IRepositoryAsync<Movimiento> _repositoryAsync;
        private readonly IMapper _mapper;
        private readonly IAccountProxy _accountProxy;

        public TransactionGetAllEventHandler(
            IRepositoryAsync<Movimiento> repositoryAsync,
            IMapper mapper,
            IAccountProxy accountProxy)
        {
            _repositoryAsync = repositoryAsync;
            _mapper = mapper;
            _accountProxy = accountProxy;
        }

        public async Task<Response<List<MovimientoDto>>> Handle(TransactionGetAllQuery command, CancellationToken cancellationToken)
        {
            bool isvalidInitialDate = DateTime.TryParse(command.FechaInicio, out DateTime initialDate);

            if (!isvalidInitialDate)
                throw new ApiException("La fecha de inicio es inválida");

            bool isvalidEndDate = DateTime.TryParse(command.FechaFin, out DateTime endDate);

            if (!isvalidEndDate)
                throw new ApiException("La fecha de fin es inválida");

            bool guidValido = Guid.TryParse(command.CuentaGuid, out Guid cuentaGuid);

            if (!guidValido)
                throw new ApiException($"CuentaGuid contiene un valor inválido : {command.CuentaGuid}");

            var resultAccountRemote = await _accountProxy.GetCuentaAsync(new AccountGetByIdCommand { CuentaGuid = command.CuentaGuid });

            if (!resultAccountRemote.procesoCorrecto || resultAccountRemote.cuenta == null)
                throw new ApiException(resultAccountRemote.Error);

            TransactionPagedSpecification specification = new(
                command.PageSize,
                command.PageNumber,
                initialDate,
                endDate,
                command.CuentaGuid
            );

            List<Movimiento> movimientos = await _repositoryAsync.ListAsync(specification);

            List<MovimientoDto> movimientosDto = _mapper.Map<List<MovimientoDto>>(movimientos);

            return new Response<List<MovimientoDto>>(movimientosDto);
        }
    }
}
