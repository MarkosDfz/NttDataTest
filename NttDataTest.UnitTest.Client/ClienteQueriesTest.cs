using Ardalis.Specification;
using AutoMapper;
using GenFu;
using Moq;
using NttDataTest.Domain.Client;
using NttDataTest.Services.Client.Commands;
using NttDataTest.Services.Client.Interfaces;
using NttDataTest.Services.Client.Queries;
using NttDataTest.Services.Client.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace NttDataTest.UnitTest.Client
{
    public class ClienteQueriesTest
    {
        private readonly Mock<IRepositoryAsync<Cliente>> _mockClienteRepository = new();
        private readonly Mock<IRepositoryAsync<Cliente>> _mockClienteByIdRepository = new();
        private readonly IMapper _mapper;

        public ClienteQueriesTest()
        {
            var clientes = ObtenerDataPrueba();

            _mockClienteRepository.Setup(x => x.ListAsync(It.IsAny<ISpecification<Cliente>>(), default))
                                  .ReturnsAsync(clientes);

            _mockClienteByIdRepository.Setup(x => x.GetBySpecAsync(It.IsAny<ClientByGuiSpecification>(), default))
                                      .ReturnsAsync(clientes[0]);

            var mapConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingTest());
            });

            var mapper = mapConfig.CreateMapper();

            _mapper = mapper;
        }

        private List<Cliente> ObtenerDataPrueba()
        {
            A.Configure<Cliente>().Fill(x => x.Nombre).AsFirstName()
                                  .Fill(x => x.Telefono).AsPhoneNumber()
                                  .Fill(x => x.Direccion).AsAddress()
                                  .Fill(x => x.Identificacion)
                                  .Fill(x => x.Estado, () => { return true; })
                                  .Fill(x => x.Edad, () => { return 26; })
                                  .Fill(x => x.Genero)
                                  .Fill(x => x.ClienteGuid, () => { return Guid.NewGuid().ToString(); });

            var clientes = A.ListOf<Cliente>(30);

            clientes[0].ClienteGuid = "cc5b0156-8419-4c51-8cfc-c5bbc31584f1";

            return clientes;
        }

        [Fact]
        public async Task GetAllClients()
        {
            //System.Diagnostics.Debugger.Launch();

            var request = new ClienteGetAllQuery();

            var handler = new ClientGetAllEventHandler(_mockClienteRepository.Object, _mapper);

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.True(result.Data.Any());
        }

        [Fact]
        public async Task GetClienteByID()
        {
            //System.Diagnostics.Debugger.Launch();

            var request = new ClientGetByIdQuery() { ClienteGuid = "cc5b0156-8419-4c51-8cfc-c5bbc31584f1" };

            var handler = new ClientGetByIdEventHandler(_mockClienteByIdRepository.Object, _mapper);

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.True(result.Data.ClienteGuid == "cc5b0156-8419-4c51-8cfc-c5bbc31584f1");
        }

        [Fact]
        public async Task ThrowWhenClientNotExists()
        {
            //System.Diagnostics.Debugger.Launch();

            var request = new ClientGetByIdQuery() { ClienteGuid = "cc5b0156-8419-4c51-8cfc-5145454" };

            _mockClienteByIdRepository.Setup(x => x.GetBySpecAsync(It.IsAny<ClientByGuiSpecification>(), default))
                                      .ReturnsAsync(null as Cliente);

            var handler = new ClientGetByIdEventHandler(_mockClienteByIdRepository.Object, _mapper);

            await Assert.ThrowsAsync<KeyNotFoundException>(async () => await handler.Handle(request, CancellationToken.None));
        }

        [Fact]
        public async Task AddNewClient()
        {
            //System.Diagnostics.Debugger.Launch();

            Cliente cliente = new()
            {
                Nombre = "Cliente Test",
                Direccion = "Quito",
                Edad = 27,
                Estado = true,
                Contrasenia = "123456789",
                Genero = 0,
                Identificacion = "8151899966",
                Telefono = "0986989654"
            };

            _mockClienteRepository.Setup(x => x.AddAsync(It.IsAny<Cliente>(), default))
                                  .ReturnsAsync(cliente);

            var handler = new ClientCreateEventHandler(_mockClienteRepository.Object, _mapper);

            var response = await handler.Handle(new ClientCreateCommand(), CancellationToken.None);

            _mockClienteRepository.Verify(x => x.AddAsync(It.IsAny<Cliente>(), default), Times.Once);

            Assert.True(response.Succeeded);
        }
    }
}
