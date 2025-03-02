using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class AppLogin
    {
        public required string? Email { get; set; } // El correo electrónico del usuario
        public required string? Password { get; set; } // La contraseña del usuario
    }
}