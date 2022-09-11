using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NttDataTest.Services.Proxies.Account.Transaction.Commands
{
    public class TransactionGetAllCommand
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public string CuentaGuid { get; set; }

        public string FechaInicio { get; set; }

        public string FechaFin { get; set; }
    }
}
