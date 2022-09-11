using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NttDataTest.Services.Proxies.Transaction.Account
{
    public enum CuentaType
    {
        Ahorro,
        Corriente
    }

    public class CuentaRemoteResponse
    {
        public Guid CuentaGuid { get; set; }
        public string NumeroCuenta { get; set; }

        public CuentaType TipoCuenta { get; set; }

        public decimal SaldoInicial { get; set; }

        public bool Estado { get; set; }

        public Guid ClienteGuid { get; set; }
    }
}
