using System;

namespace NttDataTest.Services.Account.DTOs
{
    public class ReporteMovimientoDTO
    {
        public string FechaMovimiento { get; set; }

        public string Cliente { get; set; }

        public string NumeroCuenta { get; set; }

        public string TipoCuenta { get; set; }

        public bool EstadoCuenta { get; set; }

        public string TipoMovimiento { get; set; }

        public decimal ValorMovimiento { get; set; }

        public decimal SaldoDisponible { get; set; }
    }
}
