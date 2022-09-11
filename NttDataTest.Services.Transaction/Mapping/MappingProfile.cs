using AutoMapper;
using NttDataTest.Domain.Transaction;
using NttDataTest.Services.Transaction.Commands;
using NttDataTest.Services.Transaction.DTOs;

namespace NttDataTest.Services.Transaction.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region DTOs
            CreateMap<Movimiento, MovimientoDto>();
            #endregion

            #region Command
            CreateMap<TransactionCreateCommand, Movimiento>();
            #endregion
        }
    }
}
