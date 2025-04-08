using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvernaderoInteligenteMovil.Models
{
    public class UsuarioModel
    {
        public string UsuarioId { get; set; }
        public string NombreCompleto { get; set; }
        public string Email { get; set; }
        public string Contrasena { get; set; }
        public int Rol { get; set; }
        public List<string> Invernaderos { get; set; }
    }
}
