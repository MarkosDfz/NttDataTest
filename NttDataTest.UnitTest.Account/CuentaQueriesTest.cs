using Ardalis.Specification;
using GenFu;
using Moq;
using NttDataTest.Domain.Account;
using NttDataTest.Services.Account.Exceptions;
using NttDataTest.Services.Account.Interfaces;
using NttDataTest.Services.Account.Queries;
using NttDataTest.Services.Account.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace NttDataTest.UnitTest.Account
{
    public class CuentaQueriesTest
    {
        private readonly Mock<IRepositoryAsync<Cuenta>> _mockCuentaRepository = new();
        private readonly Mock<IRepositoryAsync<Cuenta>> _mockCuentaByIdRepository = new();

        public CuentaQueriesTest()
        {
            var cuentas = ObtenerDataPrueba();

            _mockCuentaRepository.Setup(x => x.ListAsync(It.IsAny<ISpecification<Cuenta>>(), default))
                                 .ReturnsAsync(cuentas);

            _mockCuentaByIdRepository.Setup(x => x.GetBySpecAsync(It.IsAny<AccountByNumberSpecification>(), default))
                                      .ReturnsAsync(cuentas[0]);
        }

        private List<Cuenta> ObtenerDataPrueba()
        {
            A.Configure<Cuenta>().Fill(x => x.NumeroCuenta)
                                 .Fill(x => x.TipoCuenta)
                                  .Fill(x => x.Estado, () => { return true; })
                                  .Fill(x => x.CuentaGuid, () => { return Guid.NewGuid(); })
                                  .Fill(x => x.ClienteGuid, () => { return Guid.NewGuid(); });

            var cuentas = A.ListOf<Cuenta>(30);

            cuentas[0].CuentaGuid = new Guid("a36066b1-8d39-42a1-b618-90442e7969d7");
            cuentas[0].ClienteGuid = new Guid("cc5b0156-8419-4c51-8cfc-c5bbc31584f1");
            cuentas[0].NumeroCuenta = "1789852";

            return cuentas;
        }

        [Fact]
        public async Task GetAllAccounts()
        {
            var request = new AccountGetAllQuery();

            var handler = new AccountGetAllEventHandler(_mockCuentaRepository.Object);

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.True(result.Data.Any());
        }

        [Fact]
        public async Task GetCuentaByNumber()
        {
            //System.Diagnostics.Debugger.Launch();

            var request = new AccountGetAllQuery()
            {
                NumeroCuenta = "1789852"
            };

            var handler = new AccountGetAllEventHandler(_mockCuentaRepository.Object);

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.True(result.Data.Any());
            Assert.True(result.Data.Exists(x => x.NumeroCuenta == request.NumeroCuenta));
        }

        [Fact]
        public async Task GetAccountByID()
        {
            //System.Diagnostics.Debugger.Launch();
            var guidExists = new Guid("a36066b1-8d39-42a1-b618-90442e7969d7");

            var cuenta = new Cuenta()
            {
                CuentaGuid = guidExists,
                Estado = true,
                NumeroCuenta = "1789852"
            };

            var request = new AccountGetByIdQuery() { CuentaGuid = guidExists.ToString() };

            _mockCuentaRepository.Setup(x => x.GetByIdAsync(guidExists, default))
                                 .ReturnsAsync(cuenta);

            var handler = new AccountGetByIdEventHandler(_mockCuentaRepository.Object);

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.True(result.Data.CuentaGuid == guidExists);
        }

        [Fact]
        public async Task ThrowWhenIsInvalidGuid()
        {
            //System.Diagnostics.Debugger.Launch();

            var request = new AccountGetByIdQuery() { CuentaGuid = "5645asdasdw55" };

            _mockCuentaRepository.Setup(x => x.GetByIdAsync(It.IsAny<Cuenta>(), default))
                                 .ReturnsAsync(null as Cuenta);

            var handler = new AccountGetByIdEventHandler(_mockCuentaRepository.Object);

            await Assert.ThrowsAsync<ApiException>(async () => await handler.Handle(request, CancellationToken.None));
        }
    }
}
