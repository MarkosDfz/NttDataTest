using AutoMapper;
using MediatR;
using NttDataTest.Domain.Transaction;
using NttDataTest.Services.Transaction.DTOs;
using NttDataTest.Services.Transaction.Exceptions;
using NttDataTest.Services.Transaction.Interfaces;
using NttDataTest.Services.Transaction.Wrappers;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NttDataTest.Services.Transaction.Queries
{
    public class TransactionGetByIdQuery : IRequest<Response<MovimientoDto>>
    {
        public string MovimientoGuid { get; set; }
    }

    public class TransactionGetById : IRequestHandler<TransactionGetByIdQuery, Response<MovimientoDto>>
    {
        private readonly IRepositoryAsync<Movimiento> _repositoryAsync;
        private readonly IMapper _mapper;

        public TransactionGetById(IRepositoryAsync<Movimiento> repositoryAsync, IMapper mapper)
        {
            _repositoryAsync = repositoryAsync;
            _mapper = mapper;
        }

        public async Task<Response<MovimientoDto>> Handle(TransactionGetByIdQuery command, CancellationToken cancellationToken)
        {
            bool guidValido = Guid.TryParse(command.MovimientoGuid, out Guid movimientoGuid);

            if (!guidValido)
                throw new ApiException($"MovimientoGuid contiene un valor inválido : {command.MovimientoGuid}");

            Movimiento movimiento = await _repositoryAsync.GetByIdAsync(movimientoGuid);

            if (movimiento == null)
                throw new KeyNotFoundException($"Movimiento no encontrado con el ID: {command.MovimientoGuid}");

            MovimientoDto movimientoDto = _mapper.Map<MovimientoDto>(movimiento);

            return new Response<MovimientoDto>(movimientoDto);
        }
    }
}
