using System.ComponentModel.DataAnnotations;

namespace NttDataTest.Domain.Client
{
    public class Cliente : Persona
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [MinLength(6, ErrorMessage = "El campo {0} debe tener mínimo {1} caractéres")]
        public string Contrasenia { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public bool Estado { get; set; }

        public string ClienteGuid { get; set; }
    }
}
