using Ardalis.Specification;
using NttDataTest.Domain.Account;
using System.Linq;

namespace NttDataTest.Services.Account.Specifications
{
    public class AccountPagedSpecification : Specification<Cuenta>
    {
        public AccountPagedSpecification(int pageSize, int pageNumber, string numeroCuenta)
        {
            Query.Skip((pageNumber - 1) * pageSize)
                 .Take(pageSize);

            if (!string.IsNullOrEmpty(numeroCuenta))
            {
                Query.Search(x => x.NumeroCuenta, "%" + numeroCuenta + "%");
            }
        }
    }
}
