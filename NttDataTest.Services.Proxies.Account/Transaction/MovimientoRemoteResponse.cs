using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NttDataTest.Services.Proxies.Account.Transaction
{
    public class MovimientoRemoteResponse
    {
        public string TipoMovimiento { get; set; }

        public decimal ValorMovimiento { get; set; }

        public decimal SaldoDisponible { get; set; }

        public DateTime Fecha { get; set; }
    }
}
