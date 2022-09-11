using AutoMapper;
using NttDataTest.Domain.Account;
using NttDataTest.Services.Account.Commands;

namespace NttDataTest.Services.Account.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Command
            CreateMap<AccountCreateCommand, Cuenta>();
            #endregion
        }
    }
}
