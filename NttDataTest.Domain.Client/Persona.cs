using System.ComponentModel.DataAnnotations;
using static NttDataTest.Common.Client.Enums;

namespace NttDataTest.Domain.Client
{
    public class Persona
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [MinLength(6, ErrorMessage = "El campo {0} debe tener mínimo {1} caractéres")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public GeneroType Genero { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public int Edad { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [MinLength(10, ErrorMessage = "El campo {0} debe tener mínimo {1} caractéres")]
        [MaxLength(15, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres")]
        public string Identificacion { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [MinLength(4, ErrorMessage = "El campo {0} debe tener mínimo {1} caractéres")]
        [MaxLength(250, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [MinLength(10, ErrorMessage = "El campo {0} debe tener mínimo {1} caractéres")]
        [MaxLength(15, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres")]
        public string Telefono { get; set; }
    }
}
