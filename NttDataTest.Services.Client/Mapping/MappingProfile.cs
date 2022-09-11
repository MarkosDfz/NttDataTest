using AutoMapper;
using NttDataTest.Domain.Client;
using NttDataTest.Services.Client.Commands;
using NttDataTest.Services.Client.DTOs;
using NttDataTest.Services.Client.Queries;

namespace NttDataTest.Services.Client.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region DTOs
            CreateMap<Cliente, ClienteDTO>();
            #endregion

            #region Commands
            CreateMap<ClientCreateCommand, Cliente>();
            #endregion
        }
    }
}
