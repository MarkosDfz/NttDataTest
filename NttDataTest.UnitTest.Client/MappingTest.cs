using AutoMapper;
using NttDataTest.Domain.Client;
using NttDataTest.Services.Client.Commands;
using NttDataTest.Services.Client.DTOs;

namespace NttDataTest.UnitTest.Client
{
    internal class MappingTest : Profile
    {
        public MappingTest()
        {
            CreateMap<Cliente, ClienteDTO>();

            CreateMap<ClientCreateCommand, Cliente>();
        }
    }
}
