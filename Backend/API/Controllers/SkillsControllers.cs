using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Entities;
using API.Repositories;
using API.DTOs;
using API.Errors;
using Microsoft.AspNetCore.Authorization;  // Asegúrate de incluir el espacio de nombres para ApiException

namespace API.Controllers
{   [Authorize] 
    [ApiController]
    [Route("api/[controller]")]
    public class SkillsController : ControllerBase
    {
        private readonly SkillRepository _skillRepository;
        

        public SkillsController(SkillRepository skillRepository)
        {
            _skillRepository = skillRepository;
        }

        // Obtener todas las habilidades
        // [Authorize(Roles = "User,Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppSkill>>> GetSkills()
        {
            try
            {
                var skills = await _skillRepository.GetSkillsAsync();
                return Ok(skills);
            }
            catch (Exception ex)
            {
                throw new ApiException(500, $"Error interno del servidor: {ex.Message}"); // Lanza ApiException en caso de error
            }
        }

        // Crear una nueva habilidad
        
        [HttpPost]
        public async Task<ActionResult<AppSkill>> CreateSkill(SkillsDTO skillsDTO)
        {
            // Validación básica de los datos recibidos
            if (skillsDTO == null || string.IsNullOrWhiteSpace(skillsDTO.Name))
            {
                throw new ApiException(400, "Datos inválidos."); // Lanza ApiException si los datos son inválidos
            }

            try
            {
                var skill = new AppSkill
                {
                    Name = skillsDTO.Name,
                    Description = skillsDTO.Description,
                    IconUrl = skillsDTO.IconUrl,
                    Percentage = skillsDTO.Percentage
                };

                // Crear la habilidad y retornarla con el código 201
                var createdSkill = await _skillRepository.AddSkillAsync(skill);
                return CreatedAtAction(nameof(GetSkillById), new { id = createdSkill.Id }, createdSkill);
            }
            catch (Exception ex)
            {
                throw new ApiException(500, $"Error al crear la habilidad: {ex.Message}"); // Lanza ApiException en caso de error
            }
        }

        // Obtener una habilidad por su ID
        
        [HttpGet("by-id")]
        public async Task<ActionResult<AppSkill>> GetSkillById([FromQuery]int id)
        {
            Console.WriteLine($"{id}");
            try
            {
                var skill = await _skillRepository.GetSkillByIdAsync(id);
                if (skill == null)
                {
                    throw new ApiException(404, $"La habilidad con ID {id} no existe."); // Lanza ApiException si no se encuentra
                    
                }
                return Ok(skill);
            }
            catch (ApiException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message); // Manejo de errores controlados
            }
            catch (Exception ex)
            {
                throw new ApiException(500, $"Error interno del servidor: {ex.Message}"); // Lanza ApiException en caso de error inesperado
            }
        }

        // Actualizar una habilidad existente
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSkill(int id, SkillsDTO skillsDTO)
        {
            // Verificación de id válido y datos no nulos
            if (id <= 0 || skillsDTO == null)
            {
                throw new ApiException(400, "Datos enviados son inválidos."); // Lanza ApiException si los datos son inválidos
            }

            try
            {
                var existingSkill = await _skillRepository.GetSkillByIdAsync(id);
                if (existingSkill == null)
                {
                    throw new ApiException(404, $"La habilidad con ID {id} no existe."); // Lanza ApiException si no se encuentra la habilidad
                }

                // Asignación de los nuevos valores
                existingSkill.Name = skillsDTO.Name;
                existingSkill.Description = skillsDTO.Description;
                existingSkill.IconUrl = skillsDTO.IconUrl;
                existingSkill.Percentage = skillsDTO.Percentage;

                // Actualización de la habilidad en el repositorio
                var result = await _skillRepository.UpdateSkillAsync(existingSkill);
                if (!result)
                {
                    throw new ApiException(500, "Error al actualizar la habilidad."); // Lanza ApiException si falla la actualización
                }

                return Ok("La Experiencia se Actualizo con exito."); // 200 No Content
            }
             catch (ApiException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message); // Manejo de errores controlados
            }
            catch (Exception ex)
            {
                throw new ApiException(500, $"Error al actualizar la habilidad: {ex.Message}"); // Lanza ApiException en caso de error inesperado
            }
        }

        // Eliminar una habilidad
        [Authorize(Roles = "Admin")]
        [HttpDelete("by-id")]
        public async Task<IActionResult> DeleteSkill([FromQuery]int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ApiException(400, "El id debe ser mayor que 0."); // Lanza ApiException si el id es inválido
                }

                var result = await _skillRepository.DeleteSkillAsync(id);
                if (!result)
                {
                    throw new ApiException(404, $"La habilidad con ID {id}  no existe."); // Lanza ApiException si no se encuentra la habilidad
                }

                return Ok("La experiencia se elimino con exito."); // 204 No Content
            }
             catch (ApiException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message); // Manejo de errores controlados
            }
            catch (Exception ex)
            {
                throw new ApiException(500, $"Error al eliminar la habilidad: {ex.Message}"); // Lanza ApiException en caso de error inesperado
            }
        }
    }
}
