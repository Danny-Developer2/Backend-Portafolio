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
using API.DTOs;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;

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



        private string EncryptToken(string token)
        {
            var key = Encoding.UTF8.GetBytes(_jwtSecretKey!); // La clave debe ser de 256 bits para AES-256
            using (var aes = Aes.Create())
            {
                aes.Key = key;
                aes.GenerateIV(); // Genera un IV aleatorio
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream())
                {
                    // Guardar IV al principio del texto cifrado
                    ms.Write(aes.IV, 0, aes.IV.Length);

                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    using (var sw = new StreamWriter(cs))
                    {
                        sw.Write(token); // Escribe el token en el flujo cifrado
                    }

                    // Devuelve el token cifrado en base64
                    return Convert.ToBase64String(ms.ToArray());
                }
            }

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

            // La clave secreta como cadena normal
            var secretKey = Encoding.UTF8.GetBytes(_jwtSecretKey!);  // Utiliza tu propia clave secreta

            var key = new SymmetricSecurityKey(secretKey);
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.Now.AddHours(24),
                signingCredentials: creds
            );

           

            


            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        private string GenerateJwtData(string email, string role)
        {
            var claims = new[]
            {
        new Claim(ClaimTypes.NameIdentifier, email),
        new Claim(ClaimTypes.Role, role),
        // new Claim(JwtRegisteredClaimNames.Sub, email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

            // La clave secreta como cadena normal
            var secretKey = Encoding.UTF8.GetBytes(_jwtSecretKey!);  // Utiliza tu propia clave secreta

            var key = new SymmetricSecurityKey(secretKey);
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

          

            var data = new JwtSecurityToken(
                expires: DateTime.Now.AddDays(7),
                signingCredentials: creds,
                 issuer: _issuer,
                audience: _audience,
                claims: claims



            );

            


            return new JwtSecurityTokenHandler().WriteToken(data);
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
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.PasswordHash, workFactor: 11) // Hashear la contraseña
            };
            var token = GenerateJwtToken(user.Email!, user.Role.ToString());
            var data = GenerateJwtData(user.Email!, user.Role.ToString()); 

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
                Token = token,
                Data = data
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            {
                return Unauthorized(new { message = "Credenciales incorrectas." });
            }

            // 🔑 Generar y cifrar el token
            var token = GenerateJwtToken(user.Email!, user.Role.ToString());
            var data = GenerateJwtData(user.Email!, user.Role.ToString());
            // var encryptedToken = EncryptToken(token);

            // 🥠 Configurar la cookie HttpOnly
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,  // No accesible desde JavaScript
                Secure = false,    // Solo en HTTPS
                SameSite = SameSiteMode.Strict, // Evita ataques CSRF
                Expires = DateTime.UtcNow.AddDays(7) // Expira en 24 horas
            };

            Console.WriteLine(token);

            Response.Cookies.Append("auth_token", token, cookieOptions);

            var userDTO = new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                Token = token,
                Data = data
            };

            return Ok(userDTO); // Retornar solo los datos del usuario, sin el token
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

        [HttpPost("verify-token")]
        public ActionResult VerifyToken([FromBody] TokenDTO tokenDto)
        {
            try
            {
                if (string.IsNullOrEmpty(tokenDto.Token))
                {
                    return BadRequest(new { message = "Token requerido" });
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecretKey!));

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _issuer,
                    ValidAudience = _audience,
                    IssuerSigningKey = key,
                    ClockSkew = TimeSpan.Zero  // No permitir tiempo extra
                };

                tokenHandler.ValidateToken(tokenDto.Token, validationParameters, out SecurityToken validatedToken);

                return Ok(new { message = "Token válido", isValid = true });
            }
            catch (SecurityTokenExpiredException)
            {
                return Unauthorized(new { message = "Token expirado", isValid = false });
            }
            catch (SecurityTokenException)
            {
                return Unauthorized(new { message = "Token inválido", isValid = false });
            }
        }


        [HttpGet]
        [Authorize]
        [Authorize(Roles = "Admin")]
        public Task<IActionResult> Prueba()
        {
            return Task.FromResult<IActionResult>(Ok("Pruebas Cookies"));
        }

        internal bool VerifyToken(string v)
        {
            throw new NotImplementedException();
        }
    }
}