namespace API.DTO
{
    public class LoginDTO
    {
        public required string? Email { get; set; } // El correo electrónico del usuario
        public required string? Password { get; set; } // La contraseña del usuario
    }
}
