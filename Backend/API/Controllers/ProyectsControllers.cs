using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Entities;
using API.Data;
using API.DTOs;
using API.Errors;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using API.DTO;  // Asegúrate de incluir el espacio de nombres para ApiException

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly DataContext _context;

        public ProjectsController(DataContext context)
        {
            _context = context;
        }

        // Obtener todos los proyectos

        [Authorize(Roles = "User,Admin")]
        [HttpGet]
        // public async Task<ActionResult<IEnumerable<ProyectDTO>>> GetProjects(int id)
        // {
        //     var user = await _context.Users
        //         .Include(u => u.Projects)
        //         .Include(u => u.Skills)
        //         .Include(u => u.Experiences)
        //         .FirstOrDefaultAsync(u => u.Id == id);

        //     if (user == null)
        //     {
        //         return NotFound(new { message = "Usuario no encontrado." });
        //     }

        //     // Mapea los proyectos a ProyectDTO
        //     var proyectDTOs = user.Projects.Select(p => new ProyectDTO
        //     {
        //         Id = p.Id,
        //         Name = p.Name,
        //         Description = p.Description,
        //         Technology = p.Technology,
        //         Url = p.Url,
        //         ImgUrl = p.ImgUrl
        //     }).ToList();

        //     return Ok(proyectDTOs); // Devuelve la lista de proyectos mapeados
        // }

        public async Task<ActionResult<IEnumerable<ProyectDTO>>> GetAllProjects()
        {
            try
            {
                // Obtener todos los proyectos con detalles de habilidades y experiencias
                var projects = await _context.Projects
                .Select(p => new ProyectDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Technology = p.Technology,
                    Url = p.Url,
                    ImgUrl = p.ImgUrl,

                    // Proyección de habilidades con los detalles completos
                    Skills = p.ProyectSkills!.Any() ? p.ProyectSkills!.Select(ps => new SkillsDTO
                    {
                        Id = ps.Skill!.Id,
                        SkillName = ps.Skill.Name,
                        IconUrl = ps.Skill.IconUrl,
                        Percentage = ps.Skill.Percentage,
                        Name = ps.Skill.Name,
                        Description = ps.Skill.Description
                    }).ToList() : new List<SkillsDTO>(),

                    // Proyección de experiencias con los detalles completos
                    // Proyección de experiencias con los detalles completos (debe proyectarse correctamente)
                    Experience = p.ProyectExperience!.Any() ? p.ProyectExperience!.Select(pe => new ExperienceDTO
                    {
                        Id = pe.Experience!.Id,
                        CompanyName = pe.Experience.CompanyName,
                        Position = pe.Experience.Position,
                        StartDate = pe.Experience.StartDate,
                        EndDate = pe.Experience.EndDate,
                        Description = pe.Experience.Description
                    }).ToList() : new List<ExperienceDTO>(),

                    // Proyección de usuarios asignados con los detalles completos
                    Users = p.UserProyects!.Any() ? p.UserProyects!.Select(up => new UserDTO
                    {
                        Id = up.User!.Id,
                        Name = up.User.Name,
                        Email = up.User.Email
                    }).ToList() : new List<UserDTO>()
                })
                    .ToListAsync(); // Obtener todos los proyectos

                if (projects == null || !projects.Any())
                {
                    return NotFound(new { message = "No hay proyectos disponibles." });
                }

                return Ok(projects); // Devuelve todos los proyectos con los detalles completos
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener los proyectos: {ex.Message}");
            }
        }



        // Obtener un proyecto por ID
        [Authorize(Roles = "User,Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<AppProyect>> GetProjectById(int id)
        {
            try
            {
                // Obtener el proyecto con el id solicitado, junto con los detalles de habilidades y experiencias
                var project = await _context.Projects
                    .Where(p => p.Id == id) // Filtrar por id
                    .Select(p => new ProyectDTO
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Description = p.Description,
                        Technology = p.Technology,
                        Url = p.Url,
                        ImgUrl = p.ImgUrl,

                        // Proyección de habilidades con los detalles completos
                        Skills = p.ProyectSkills!.Any() ? p.ProyectSkills!.Select(ps => new SkillsDTO
                        {
                            Id = ps.Skill!.Id,
                            SkillName = ps.Skill.Name,
                            IconUrl = ps.Skill.IconUrl,
                            Percentage = ps.Skill.Percentage,
                            Name = ps.Skill.Name,
                            Description = ps.Skill.Description
                        }).ToList() : new List<SkillsDTO>(),

                        // Proyección de experiencias con los detalles completos
                        Experience = p.ProyectExperience!.Any() ? p.ProyectExperience!.Select(pe => new ExperienceDTO
                        {
                            Id = pe.Experience!.Id,
                            CompanyName = pe.Experience.CompanyName,
                            Position = pe.Experience.Position,
                            StartDate = pe.Experience.StartDate,
                            EndDate = pe.Experience.EndDate,
                            Description = pe.Experience.Description
                        }).ToList() : new List<ExperienceDTO>(),

                        // Proyección de usuarios asignados con los detalles completos
                        Users = p.UserProyects!.Any() ? p.UserProyects!.Select(up => new UserDTO
                        {
                            Id = up.User!.Id,
                            Name = up.User.Name,
                            Email = up.User.Email
                        }).ToList() : new List<UserDTO>()
                    })
                    .FirstOrDefaultAsync(); // Obtener un solo proyecto con el id solicitado

                if (project == null)
                {
                    return NotFound(new { message = "Proyecto no encontrado." });
                }

                return Ok(project); // Devuelve el proyecto con el id especificado y los detalles completos
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener el proyecto: {ex.Message}");
            }

        }


        // Crear un nuevo proyecto
        [Authorize(Roles = "Admin")]

        [HttpPost]
        public async Task<ActionResult<AppProyect>> CreateProject(ProyectDTO newProject)
        {
            if (newProject == null || string.IsNullOrWhiteSpace(newProject.Name) || string.IsNullOrWhiteSpace(newProject.Description))
            {
                return BadRequest("Datos inválidos. Asegúrate de proporcionar todos los campos necesarios.");
            }

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

                // Crear el nuevo proyecto
                var project = new AppProyect
                {
                    Name = newProject.Name,
                    Description = newProject.Description,
                    Technology = newProject.Technology,
                    Url = newProject.Url,
                    ImgUrl = newProject.ImgUrl
                };

                // Guardar el proyecto en la base de datos
                _context.Projects.Add(project);
                await _context.SaveChangesAsync(); // Guardamos para obtener el ID generado

                // Asignar habilidades al proyecto
                if (newProject.SkillIds != null && newProject.SkillIds.Any())
                {
                    var projectSkills = new List<ProyectSkill>(); // ✅ Usar ProjectSkill en lugar de UserSkill

                    foreach (var skillId in newProject.SkillIds)
                    {
                        var skill = await _context.Skills.FindAsync(skillId);
                        if (skill == null)
                        {
                            return BadRequest($"La habilidad con ID {skillId} no existe.");
                        }

                        // Crear la relación entre proyecto y habilidad
                        projectSkills.Add(new ProyectSkill { ProjectId = project.Id, SkillId = skillId, Skill = skill }); // ✅ Relación correcta
                    }

                    _context.ProjectSkills.AddRange(projectSkills); // ✅ Guardar la relación correcta
                    await _context.SaveChangesAsync();
                }
                // Asignar experiencias al proyecto
                if (newProject.ExperienceIds != null && newProject.ExperienceIds.Any())
                {
                    var projectExperiences = new List<ProyectExperience>();

                    foreach (var experienceId in newProject.ExperienceIds)
                    {
                        var experience = await _context.Experiences.FindAsync(experienceId);
                        if (experience == null)
                        {
                            return BadRequest($"La experiencia con ID {experienceId} no existe.");
                        }

                        // Crear la relación entre proyecto y experiencia
                        projectExperiences.Add(new ProyectExperience { ProjectId = project.Id, ExperienceId = experienceId });
                    }

                    _context.ProyectExperience.AddRange(projectExperiences);
                    await _context.SaveChangesAsync();
                }

                // Asignar el proyecto a los usuarios (si es necesario, se podría agregar una lista de UserIds)
                if (newProject.UserIds != null && newProject.UserIds.Any())
                {
                    var projectUsers = new List<UserProyect>();

                    foreach (var userId in newProject.UserIds)
                    {
                        var assignedUser = await _context.Users.FindAsync(userId);
                        if (assignedUser == null)
                        {
                            return BadRequest($"El usuario con ID {userId} no existe.");
                        }

                        // Relacionar el proyecto con el usuario
                        projectUsers.Add(new UserProyect { ProjectId = project.Id, UserId = userId });
                    }

                    _context.UserProyects.AddRange(projectUsers);
                    await _context.SaveChangesAsync();
                }

                return CreatedAtAction(nameof(GetProjectById), new { id = project.Id }, project);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al crear el proyecto: {ex.Message}");
            }
        }




        // Actualizar un proyecto existente
        // [Authorize(Roles = "User")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, ProyectDTO updatedProject)
        {
            // Validación de los datos
            if (updatedProject == null || id != updatedProject.Id || string.IsNullOrWhiteSpace(updatedProject.Name) || string.IsNullOrWhiteSpace(updatedProject.Description))
            {
                throw new ApiException(400, "Datos inválidos. Asegúrate de proporcionar todos los campos necesarios."); // Lanza ApiException si los datos son inválidos
            }

            try
            {
                var existingProject = await _context.Projects.FindAsync(id);
                if (existingProject == null)
                {
                    throw new ApiException(404, "El proyecto no existe."); // Lanza ApiException si no se encuentra el proyecto
                }

                // Mapear manualmente los valores del DTO a la entidad
                existingProject.Name = updatedProject.Name;
                existingProject.Description = updatedProject.Description;
                existingProject.Technology = updatedProject.Technology;
                existingProject.Url = updatedProject.Url;
                existingProject.ImgUrl = updatedProject.ImgUrl;

                // Guardar los cambios
                await _context.SaveChangesAsync();

                return Ok("Los datos del proyecto se actualizaron con éxito."); // Retorna respuesta exitosa
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new ApiException(409, "Conflicto de concurrencia. Intenta nuevamente."); // Lanza ApiException en caso de conflicto de concurrencia
            }
            catch (ApiException ex)
            {
                // Captura específicamente ApiException y la retorna con el código de estado
                throw new ApiException(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                // Captura otros errores inesperados
                throw new ApiException(500, $"Error al actualizar el proyecto: {ex.Message}"); // Lanza ApiException en caso de error inesperado
            }
        }

        // Eliminar un proyecto
        // [Authorize(Roles = "User")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            try
            {
                var project = await _context.Projects.FindAsync(id);
                if (project == null)
                {
                    throw new ApiException(404, "El proyecto no existe."); // Lanza ApiException si no se encuentra el proyecto
                }

                // Eliminar el proyecto
                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();

                return Ok($"El Proyecto con Id {id}, fue eliminado con éxito.");
            }
            catch (ApiException ex)
            {
                // Captura la ApiException lanzada si no se encuentra el proyecto o algún otro caso que use ApiException
                throw new ApiException(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                // Captura cualquier otra excepción inesperada y lanza ApiException para manejar el error de manera consistente
                throw new ApiException(500, $"Error al eliminar el proyecto: {ex.Message}"); // Lanza ApiException en caso de error
            }
        }

    }
}
