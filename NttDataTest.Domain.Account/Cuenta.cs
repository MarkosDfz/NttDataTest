using System;
using System.ComponentModel.DataAnnotations;
using static NttDataTest.Common.Account.Enums;

namespace NttDataTest.Domain.Account
{
    public class Cuenta
    {
        [Key]
        public Guid CuentaGuid { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [MaxLength(12, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres")]
        public string NumeroCuenta { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public CuentaType TipoCuenta { get; set; }

        public decimal? SaldoInicial { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public bool Estado { get; set; }

        public Guid ClienteGuid { get; set; }
    }
}
