using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Entities;
using API.Data;
using API.DTO;
using API.Errors;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly DataContext _context;
        // private readonly string? _jwtSecretKey = "mi_clave_secreta_de_32_caracteres_12345"; // Cambia esta clave con una clave secreta segura
        private readonly string? _jwtSecretKey;
        private readonly string? _issuer;
        private readonly string? _audience;

        public AuthController(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _jwtSecretKey = configuration["JwtSettings:SecretKey"];
            _issuer = configuration["JwtSettings:Issuer"];
            _audience = configuration["JwtSettings:Audience"];
        }

        // Método para generar el JWT
        private string GenerateJwtToken(string email, string role)
        {
            var claims = new[]
            {
                
                new Claim(ClaimTypes.NameIdentifier, email),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecretKey!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(UserDTO model)
        {
            // Verifica si el usuario ya existe
            if (_context.Users.Any(u => u.Email == model.Email))
            {
                throw new ApiException(400, "El correo ya está registrado."); // Lanza una ApiException
            }
            
            // Crea el usuario con la contraseña encriptada
            var user = new AppUser
            
            {
                
                Name = model.Name,
                Email = model.Email,
                Role = model.Role,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.PasswordHash,workFactor: 11) // Hashear la contraseña
            };
            var token = GenerateJwtToken(user.Email!, user.Role.ToString());

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // 🔑 Generar Token JWT para el usuario registrado
            

            var userDTO = new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role
            };

            return Ok(new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                Token = token
            });
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            Console.WriteLine($"El usuario {user}");
            
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            {
                throw new ApiException(401, "Credenciales incorrectas."); // Lanza una ApiException
            }

            // 🔑 Generar Token JWT para el usuario autenticado
            var token = GenerateJwtToken(user.Email!, user.Role.ToString());

            var userDTO = new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role
            };

            return Ok(new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                Token = token
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                throw new ApiException(404, "Usuario no encontrado."); // Lanza una ApiException si el usuario no se encuentra
            }

            return new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role
            };
        }
    }
}
