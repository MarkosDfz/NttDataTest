using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NttDataTest.Common.Client.Enums;

namespace NttDataTest.Services.Client.DTOs
{
    public class ClienteDTO
    {
        public string Nombre { get; set; }

        public GeneroType Genero { get; set; }

        public int Edad { get; set; }

        public string Identificacion { get; set; }

        public string Direccion { get; set; }

        public string Telefono { get; set; }

        public bool Estado { get; set; }

        public string ClienteGuid { get; set; }
    }
}
