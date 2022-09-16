using AutoMapper;
using NttDataTest.Domain.Transaction;
using NttDataTest.Services.Transaction.Commands;
using NttDataTest.Services.Transaction.DTOs;

namespace NttDataTest.UnitTest.Transaction
{
    public class MappingTest : Profile
    {
        public MappingTest()
        {
            CreateMap<Movimiento, MovimientoDto>();

            CreateMap<TransactionCreateCommand, Movimiento>();
        }
    }
}
