using Microsoft.AspNetCore.Mvc;
using API.Entities;
using API.DTO;
using API.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using API.Errors;
using API.DTOs;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;  // Asegúrate de incluir el espacio de nombres para ApiException

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserRepository _userRepository;

         private readonly DataContext _context;

        public UsersController(UserRepository userRepository, DataContext context)
        {
            _userRepository = userRepository;

            _context = context;
        }

        // Obtener todos los usuarios
       
        // Obtener usuario por ID
        [HttpGet("{id:int}")]
        public async Task<ActionResult<AppUser>> GetUserById(int id)
        {
            try
            {
                var user = await _userRepository.GetUserById(id);
                if (user == null)
                {
                    throw new ApiException(404, "Usuario no encontrado"); // Lanza ApiException si el usuario no existe
                }
                return Ok(user);
            }
            catch (KeyNotFoundException e)
            {
                throw new ApiException(404, e.Message); // Lanza ApiException si el usuario no se encuentra
            }
            catch (Exception ex)
            {
                throw new ApiException(500, $"Error al obtener el usuario: {ex.Message}"); // Lanza ApiException en caso de error inesperado
            }
        }

        // Crear usuario
        [HttpPost]
        public async Task<ActionResult<AppUser>> CreateUser(UserDTO userDTO)
        {
            try
            {
                if (userDTO == null)
                {
                    throw new ApiException(400, "Los datos del usuario son inválidos."); // Lanza ApiException si los datos son inválidos
                }

                var user = await _userRepository.AddUser(userDTO);
                return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
            }
            catch (InvalidOperationException e)
            {
                throw new ApiException(400, e.Message); // Lanza ApiException si la operación es inválida
            }
            catch (Exception ex)
            {
                throw new ApiException(500, $"Error al crear el usuario: {ex.Message}"); // Lanza ApiException en caso de error inesperado
            }
        }

        // Actualizar usuario
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateUser(int id, UserDTO userDTO)
        {
            if (id != userDTO.Id)
            {
                throw new ApiException(400, "Los IDs no coinciden"); // Lanza ApiException si los IDs no coinciden
            }

            try
            {
                var updatedUser = await _userRepository.UpdateUser(id, userDTO);
                if (updatedUser == null)
                {
                    throw new ApiException(404, "Usuario no encontrado"); // Lanza ApiException si no se encuentra el usuario
                }
                return Ok(updatedUser);
            }
            catch (KeyNotFoundException e)
            {
                throw new ApiException(404, e.Message); // Lanza ApiException si el usuario no se encuentra
            }
            catch (InvalidOperationException e)
            {
                throw new ApiException(400, e.Message); // Lanza ApiException si la operación es inválida
            }
            catch (Exception ex)
            {
                throw new ApiException(500, $"Error al actualizar el usuario: {ex.Message}"); // Lanza ApiException en caso de error inesperado
            }
        }

        // Eliminar usuario
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var user = await _userRepository.GetUserById(id);
                if (user == null)
                {
                    throw new ApiException(404, "Usuario no encontrado"); // Lanza ApiException si no se encuentra el usuario
                }

                await _userRepository.DeleteUser(id);
                return NoContent(); // 204 No Content
            }
            catch (KeyNotFoundException e)
            {
                throw new ApiException(404, e.Message); // Lanza ApiException si el usuario no se encuentra
            }
            catch (ApiException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message); // Manejo de errores controlados
            }
            catch (Exception ex)
            {
                throw new ApiException(500, $"Error al eliminar el usuario: {ex.Message}"); // Lanza ApiException en caso de error inesperado
            }
        }
    }
}
