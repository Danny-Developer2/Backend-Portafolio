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
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using API.DTOs;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly DataContext _context;
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

            var secretKey = Encoding.UTF8.GetBytes(_jwtSecretKey!);
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
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var secretKey = Encoding.UTF8.GetBytes(_jwtSecretKey!);
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

        // Método para hashear el token (SHA-256)
        private string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(rawData));
                return Convert.ToBase64String(bytes);
            }
        }

        private async Task StoreSessionFromDtoAsync(ActivateSessionDTO dto, DataContext dbContext)
        {
            var session = new ActiveSession
            {
                UserId = dto.UserId,
                TokenHash = dto.TokenHash,
                Expiration = dto.Expiration ?? DateTime.UtcNow.AddDays(7) // La sesión dura 7 días
            };

            dbContext.ActiveSessions.Add(session);
            await dbContext.SaveChangesAsync();
        }

        // Login del usuario
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

            var hashedToken = ComputeSha256Hash(token);

            // Crear la sesión del usuario
            var sessionDto = new ActivateSessionDTO
            {
                UserId = user.Id,
                TokenHash = hashedToken,
                Expiration = DateTime.UtcNow.AddDays(7) // La sesión dura 7 días
            };

            await StoreSessionFromDtoAsync(sessionDto, _context);

            // 🥠 Configurar la cookie HttpOnly
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,  // No accesible desde JavaScript
                Secure = true,    // Solo en HTTPS
                SameSite = SameSiteMode.Strict, // Evita ataques CSRF
                Expires = DateTime.UtcNow.AddDays(7) // Expira en 7 días
            };

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

            return Ok(userDTO);
        }

        // Registro de usuario
        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(UserDTO model)
        {
            if (_context.Users.Any(u => u.Email == model.Email))
            {
                throw new ApiException(400, "El correo ya está registrado.");
            }

            var user = new AppUser
            {
                Name = model.Name,
                Email = model.Email,
                Role = model.Role,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.PasswordHash, workFactor: 11)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var token = GenerateJwtToken(user.Email!, user.Role.ToString());
            var data = GenerateJwtData(user.Email!, user.Role.ToString());

            var userDTO = new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                Token = token,
                Data = data
            };

            return Ok(userDTO);
        }

        // Verificar si el token es válido
        [HttpPost("verify-token")]
        [AllowAnonymous]
        public async Task<ActionResult> VerifyToken([FromBody] TokenDTO tokenDto)
        {
            try
            {
                if (string.IsNullOrEmpty(tokenDto.Token))
                {
                    return BadRequest(new { message = "Token requerido" });
                }

                // 1. Verificar si el token está presente en la base de datos
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

                // Validar el token
                tokenHandler.ValidateToken(tokenDto.Token, validationParameters, out SecurityToken validatedToken);

                // 2. Si el token está validado correctamente, ahora verificar si está en la base de datos
                var hashedToken = ComputeSha256Hash(tokenDto.Token);  // Hasheamos el token para comparar con la base de datos

                var session = await _context.ActiveSessions
                    .FirstOrDefaultAsync(s => s.TokenHash == hashedToken);

                if (session == null || session.Expiration <= DateTime.UtcNow)
                {
                    // Si no existe o ha expirado, marcar el token como inválido
                    return Unauthorized(new { message = "Token inválido o sesión cerrada", isValid = false });
                }

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


        // Cerrar sesión
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout([FromBody] string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(new { message = "Token requerido para cerrar sesión" });
            }

            // Añadir log para verificar el token recibido
            Console.WriteLine($"Token recibido: {token}");

            await LogoutAsync(token, _context);

            // Eliminar la cookie de sesión
            CookieOptions cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(-1)
            };
            Response.Cookies.Append("auth_token", "", cookieOptions);

            return Ok(new { message = "Sesión cerrada correctamente" });
        }

        private async Task LogoutAsync(string token, DataContext dbContext)
        {
            var hashedToken = ComputeSha256Hash(token);

            // Añadir log para verificar el token hash
            Console.WriteLine($"Token Hash: {hashedToken}");

            var session = await dbContext.ActiveSessions
                .FirstOrDefaultAsync(s => s.TokenHash == hashedToken);

            if (session != null)
            {
                dbContext.ActiveSessions.Remove(session);
                await dbContext.SaveChangesAsync();

                // Log para confirmar que se eliminó la sesión
                Console.WriteLine("Sesión eliminada correctamente.");
            }
            else
            {
                // Log para confirmar que no se encontró la sesión
                Console.WriteLine("Sesión no encontrada.");
            }
        }

        [HttpGet("test")]
        [Authorize]
        [Authorize(Roles = "Admin")]
        public IActionResult Test()
        {
            return Ok("Pruebas Cookies");
        }
    

    }
}
