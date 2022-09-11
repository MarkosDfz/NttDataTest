using MediatR;
using NttDataTest.Domain.Account;
using NttDataTest.Services.Account.Interfaces;
using NttDataTest.Services.Account.Specifications;
using NttDataTest.Services.Account.Wrappers;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NttDataTest.Services.Account.Queries
{
    public class AccountGetAllQuery : IRequest<PagedResponse<List<Cuenta>>>
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public string NumeroCuenta { get; set; }
    }

    public class AccountGetAllEventHandler : IRequestHandler<AccountGetAllQuery, PagedResponse<List<Cuenta>>>
    {
        private readonly IRepositoryAsync<Cuenta> _repositoryAsync;

        public AccountGetAllEventHandler(IRepositoryAsync<Cuenta> repositoryAsync)
        {
            _repositoryAsync = repositoryAsync;
        }

        public async Task<PagedResponse<List<Cuenta>>> Handle(AccountGetAllQuery command, CancellationToken cancellationToken)
        {
            var specification = new AccountPagedSpecification(command.PageSize, command.PageNumber, command.NumeroCuenta);

            List<Cuenta> cuentas = await _repositoryAsync.ListAsync(specification);

            return new PagedResponse<List<Cuenta>>(cuentas, command.PageNumber, command.PageSize);
        }
    }
}
