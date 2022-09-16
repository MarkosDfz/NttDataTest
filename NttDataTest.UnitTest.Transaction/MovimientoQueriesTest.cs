using AutoMapper;
using Moq;
using NttDataTest.Common.Transaction;
using NttDataTest.Domain.Transaction;
using NttDataTest.Services.Proxies.Transaction.Account;
using NttDataTest.Services.Proxies.Transaction.Account.Commands;
using NttDataTest.Services.Transaction.Commands;
using NttDataTest.Services.Transaction.Exceptions;
using NttDataTest.Services.Transaction.Interfaces;
using NttDataTest.Services.Transaction.Queries;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace NttDataTest.UnitTest.Transaction
{
    public class MovimientoQueriesTest
    {
        private readonly Mock<IRepositoryAsync<Movimiento>> _mockMovimientoRepository = new();
        private readonly Mock<IRepositoryAsync<Movimiento>> _mockMovimientoByIdRepository = new();
        private readonly IMapper _mapper;
        private readonly Mock<IAccountProxy> _accountProxy = new();
        private readonly Mock<IAccountUpdateInitialBalanceProxy> _accountUpdateBalanceProxy = new();

        public MovimientoQueriesTest()
        {
            var cuenta = new CuentaRemoteResponse()
            {
                ClienteGuid = new Guid("a36066b1-8d39-42a1-b618-90442e7969d7"),
                CuentaGuid = Guid.Empty,
                Estado = true,
                NumeroCuenta = "1448454",
                SaldoInicial = 80,
                TipoCuenta = 0
            };

            _accountProxy.SetupSequence(x => x.GetCuentaAsync(It.IsAny<AccountGetByIdCommand>()))
                          .ReturnsAsync((true, cuenta, null));

            _accountUpdateBalanceProxy.SetupSequence(x => x.UpdateCuentaSaldoInicialAsync(It.IsAny<AccountUpdateInitialBalanceCommand>()))
                                      .ReturnsAsync((true, null));

            var mapConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingTest());
            });

            var mapper = mapConfig.CreateMapper();

            _mapper = mapper;
        }

        [Fact]
        public async Task CreateNewTransaction()
        {
            //System.Diagnostics.Debugger.Launch();

            var movimiento = new Movimiento()
            {
                CuentaGuid = Guid.Empty,
                Fecha = DateTime.Now,
                MovimientoGuid = new Guid(),
                TipoMovimiento = TipoMovimientoType.Deposito,
                ValorMovimiento = 60,
                SaldoDisponible = 80
            };

            _mockMovimientoRepository.Setup(x => x.AddAsync(It.IsAny<Movimiento>(), default))
                                     .ReturnsAsync(movimiento);            

            var handler = new TransactionCreateEventHandler(_mockMovimientoRepository.Object, _mapper,
                                                            _accountProxy.Object, _accountUpdateBalanceProxy.Object);

            var response = await handler.Handle(new TransactionCreateCommand() { CuentaGuid = Guid.NewGuid().ToString() }, CancellationToken.None);

            _mockMovimientoRepository.Verify(x => x.AddAsync(It.IsAny<Movimiento>(), default), Times.Once);

            Assert.True(response.Succeeded);
        }

        [Fact]
        public async Task ThrowExceptionWhenCuentaGuidTransactionIsInvalid()
        {
            //System.Diagnostics.Debugger.Launch();

            _mockMovimientoRepository.Setup(x => x.AddAsync(It.IsAny<Movimiento>(), default))
                                     .ReturnsAsync(null as Movimiento);

            var handler = new TransactionCreateEventHandler(_mockMovimientoRepository.Object, _mapper,
                                                            _accountProxy.Object, _accountUpdateBalanceProxy.Object);

            var request = new TransactionCreateCommand()
            {
                CuentaGuid = "dsfdsdffsd"
            };

            await Assert.ThrowsAsync<ApiException>(async () => await handler.Handle(request, CancellationToken.None));
        }

        [Fact]
        public async Task ThrowExceptionWhenAccountIsInvalid()
        {
            //System.Diagnostics.Debugger.Launch();

            _mockMovimientoRepository.Setup(x => x.AddAsync(It.IsAny<Movimiento>(), default))
                                     .ReturnsAsync(null as Movimiento);

            _accountProxy.SetupSequence(x => x.GetCuentaAsync(It.IsAny<AccountGetByIdCommand>()))
                          .ReturnsAsync((false, null, "Error"));

            var handler = new TransactionCreateEventHandler(_mockMovimientoRepository.Object, _mapper,
                                                            _accountProxy.Object, _accountUpdateBalanceProxy.Object);

            var request = new TransactionCreateCommand()
            {
                CuentaGuid = new Guid().ToString()
            };

            await Assert.ThrowsAsync<ApiException>(async () => await handler.Handle(request, CancellationToken.None));
        }

        [Fact]
        public async Task GetMovimientoById()
        {
            //System.Diagnostics.Debugger.Launch();
            var guidExists = new Guid("a36066b1-8d39-42a1-b618-90442e7969d7");

            var movimiento = new Movimiento()
            {
                CuentaGuid = Guid.Empty,
                Fecha = DateTime.Now,
                MovimientoGuid = guidExists,
                TipoMovimiento = 0,
                ValorMovimiento = -5,
                SaldoDisponible = 20
            };

            var request = new TransactionGetByIdQuery() { MovimientoGuid = guidExists.ToString() };

            _mockMovimientoByIdRepository.Setup(x => x.GetByIdAsync(guidExists, default))
                                         .ReturnsAsync(movimiento);

            var handler = new TransactionGetById(_mockMovimientoByIdRepository.Object, _mapper);

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.True(result.Succeeded);
        }
    }
}
