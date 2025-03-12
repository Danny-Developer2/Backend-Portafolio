using System.ComponentModel.DataAnnotations;
using API.DTOs;
using API.Entities;

namespace API.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        public required string Name { get; set; }

        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
        [StringLength(255, ErrorMessage = "El correo electrónico no puede exceder los 255 caracteres.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(255, MinimumLength = 8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
        public string? PasswordHash { get; set; }

        public UserRole Role { get; set; } = UserRole.User;

        public string? Token { get; set; } 

        public string? Data{ get; set; }

        // IDs de habilidades relacionadas
        public List<int>? SkillIds { get; set; }
        public List<ExperienceDTO> Experiences { get; set; }  = new List<ExperienceDTO>();

        public List<SkillsDTO> Skills { get; set; }  = new List<SkillsDTO>();

         public List<ProyectDTO> Projects { get; set; }  = new List<ProyectDTO>();
        
    }
}
