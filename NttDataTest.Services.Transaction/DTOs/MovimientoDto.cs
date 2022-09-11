using System;

namespace NttDataTest.Services.Transaction.DTOs
{
    public class MovimientoDto
    {
        public string TipoMovimiento { get; set; }

        public decimal ValorMovimiento { get; set; }

        public decimal SaldoDisponible { get; set; }

        public DateTime Fecha { get; set; }
    }
}
