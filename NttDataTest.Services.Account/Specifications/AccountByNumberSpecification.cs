using Ardalis.Specification;
using NttDataTest.Domain.Account;
using System.Linq;

namespace NttDataTest.Services.Account.Specifications
{
    public class AccountByNumberSpecification : Specification<Cuenta>, ISingleResultSpecification
    {
        public AccountByNumberSpecification(string nuumeroCuenta)
        {
            Query.Where(x => x.NumeroCuenta == nuumeroCuenta);
        }
    }
}
