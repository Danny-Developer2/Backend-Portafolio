using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Entities;
using API.DTOs;
using API.Repositories;
using API.Errors;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using API.Data;
using Microsoft.EntityFrameworkCore;  // Asegúrate de incluir el espacio de nombres para ApiException

namespace API.Controllers
{   [Authorize] 
    [ApiController]
    [Route("api/[controller]")]
    public class ExperienceController : ControllerBase
    {
        private readonly ExperienceRepository _experienceRepository;

        private readonly DataContext _context;

        public ExperienceController(ExperienceRepository experienceRepository, DataContext context)
        {
            _experienceRepository = experienceRepository;
            _context = context;
        }

        // Obtener todas las experiencias
        // [Authorize(Roles = "User,Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppExperience>>> GetExperiences()
        {
            try
            {
                var experiences = await _experienceRepository.GetExperiencesAsync();
                return Ok(experiences);
            }
            catch (System.Exception ex)
            {
                throw new ApiException(500, $"Error al obtener las experiencias: {ex.Message}"); // Lanza una ApiException en caso de error
            }
        }

        // Crear una nueva experiencia
        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        public async Task<ActionResult<AppExperience>> CreateExperience(ExperienceDTO newExperience)
        {
            try
            {
                // Obtener el email del usuario desde el token
                var userEmail = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userEmail))
                {
                    return BadRequest("El email del usuario no se encuentra en el token.");
                }

                // Buscar el usuario en la base de datos
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
                if (user == null)
                {
                    return BadRequest("Usuario no encontrado en la base de datos.");
                }

                // Convertir el DTO a la entidad AppExperience y asignar el UserId
                var experience = new AppExperience
                {
                    CompanyName = newExperience.CompanyName,
                    Position = newExperience.Position,
                    StartDate = newExperience.StartDate,
                    EndDate = newExperience.EndDate,
                    Description = newExperience.Description,
                    UserId = user.Id // Asigna el UserId desde el usuario autenticado
                };

                // Insertar la experiencia en la base de datos
                var createdExperience = await _experienceRepository.AddExperienceAsync(experience);

                // Retorna la URL del recurso creado
                return CreatedAtAction(nameof(GetExperienceById), new { id = createdExperience.Id }, createdExperience);
            }
            catch (System.Exception ex)
            {
                throw new ApiException(500, $"Error al crear la experiencia: {ex.Message}"); // Lanza una ApiException en caso de error
            }
        }

        // Obtener una experiencia por su ID
        // [Authorize(Roles = "User")]
        [HttpGet("by-id")]
        public async Task<ActionResult<AppExperience>> GetExperienceById([FromQuery]int id)
        {
            try
            {
                var experience = await _experienceRepository.GetExperienceByIdAsync(id);



                if (experience == null)
                {
                    // throw new ApiException(404, "La experiencia no existe."); 
                    return NotFound($"La experiencia con ID {id}, No existe.");
                }

                return Ok(experience); // Retorna la experiencia encontrada
            }
            catch (System.Exception ex)
            {
                throw new ApiException(500, $"Error al obtener la experiencia: {ex.Message}"); // Lanza una ApiException en caso de error inesperado
            }
        }

        // Actualizar una experiencia
        [Authorize(Roles = "Admin")]
        [HttpPut("by-id")]
        public async Task<IActionResult> UpdateExperience([FromQuery]int id, ExperienceDTO updatedExperience)
        {
            try
            {
                if (updatedExperience == null || id != updatedExperience.Id)
                {
                    throw new ApiException(400, "Los datos enviados son inválidos.");
                }

                // Buscar la experiencia en la base de datos antes de actualizar
                var existingExperience = await _experienceRepository.GetExperienceByIdAsync(id);
                if (existingExperience == null)
                {
                    throw new ApiException(404, $"La experiencia con ID, {id} no existe.");
                }

                // Actualizar los datos de la experiencia
                existingExperience.CompanyName = updatedExperience.CompanyName;
                existingExperience.Position = updatedExperience.Position;
                existingExperience.StartDate = updatedExperience.StartDate;
                existingExperience.EndDate = updatedExperience.EndDate;
                existingExperience.Description = updatedExperience.Description;

                var result = await _experienceRepository.UpdateExperienceAsync(existingExperience);

                if (!result)
                {
                    throw new ApiException(500, "No se pudo actualizar la experiencia.");
                }

                return Ok("Los Datos se actualizaron con exito."); // 200 si todo salió bien
            }
            catch (ApiException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message); // Manejo de errores controlados
            }
            catch (Exception ex)
            {
                throw new ApiException(500, $"Error al actualizar la experiencia: {ex.Message}");
            }
        }

        // Eliminar una experiencia
        [Authorize(Roles = "Admin")]
        [HttpDelete("by-id")]
        public async Task<IActionResult> DeleteExperience([FromQuery]int id)
        {
            try
            {
                var existingExperience = await _experienceRepository.GetExperienceByIdAsync(id);
                if (existingExperience == null)
                {
                    throw new ApiException(404, $"La experiencia con ID {id} no existe.");
                }

                var result = await _experienceRepository.DeleteExperienceAsync(id);

                if (!result)
                {
                    throw new ApiException(500, "No se pudo eliminar la experiencia.");
                }

                return Ok($"La experiencia con el ID {id} fue eliminada con éxito.");
            }
            catch (ApiException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message); // Manejo de errores controlados
            }
            catch (Exception ex)
            {
                throw new ApiException(500, $"Error inesperado al eliminar la experiencia: {ex.Message}");
            }
        }

    }
}
