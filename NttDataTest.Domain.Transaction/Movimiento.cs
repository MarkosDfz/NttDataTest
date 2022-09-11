using NttDataTest.Common.Transaction;
using System;
using System.ComponentModel.DataAnnotations;

namespace NttDataTest.Domain.Transaction
{
    public class Movimiento
    {
        [Key]
        public Guid MovimientoGuid { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public Guid CuentaGuid { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public TipoMovimientoType TipoMovimiento { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public decimal ValorMovimiento { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public decimal SaldoDisponible { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public DateTime Fecha { get; set; }
    }
}
