using Microsoft.AspNetCore.Mvc;
using API.Entities;
using API.DTO;
using API.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using API.Errors;
using API.DTOs;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;  // Asegúrate de incluir el espacio de nombres para ApiException

namespace API.Controllers
{   [Authorize] 
    [ApiController]
    [Route("api/[controller]")]
   

    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // Obtener usuario por ID
       
        [HttpGet("by-id")]
        public async Task<ActionResult<AppUser>> GetUserById(int id)
        {
            try
            {
                var user = await _userRepository.GetUserById(id);
                return Ok(user);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al obtener el usuario: {ex.Message}" });
            }
        }

        // Crear usuario
        [HttpPost]
        public async Task<ActionResult<AppUser>> CreateUser(UserDTO userDTO)
        {
            if (userDTO is null) return BadRequest("Los datos del usuario son inválidos.");

            var user = await _userRepository.AddUser(userDTO);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        // Actualizar usuario
        [HttpPut("by-id")]
        public async Task<IActionResult> UpdateUser(int id, UserDTO userDTO)
        {
            if (id != userDTO.Id) return BadRequest("Los IDs no coinciden");

            var updatedUser = await _userRepository.UpdateUser(id, userDTO);
            return updatedUser is null ? NotFound("Usuario no encontrado") : Ok(updatedUser);
        }

        // Eliminar usuario
        [HttpDelete("by-id")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userRepository.DeleteUser(id);
            if (!result)
            {
                return NotFound(new { message = "Usuario no encontrado" });
            }
            return NoContent(); // ✅ Código 204 si se eliminó correctamente
        }
    }
}
