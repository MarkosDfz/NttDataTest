using NttDataTest.Services.Account.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NttDataTest.Services.Account.Queries
{
    public class AccountGetAllParameters : RequestParameter
    {
        public string NumeroCuenta { get; set; }
    }
}
