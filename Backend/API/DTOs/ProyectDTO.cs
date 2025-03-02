using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.DTO;
using API.Entities;

namespace API.DTOs
{
    public class ProyectDTO
    {

        [Required(ErrorMessage =
        "El ID del proyecto es requerido")]
        [Range(1, int.MaxValue, ErrorMessage =
        "El ID del proyecto debe ser un número entero positivo")]
        public int Id { get; set; }
        [Required(ErrorMessage =
        "El nombre del proyecto es requerido")]
        [StringLength(100, ErrorMessage =
        "El nombre del proyecto no puede exceder los 100 caracteres")]
        public required string Name { get; set; }
        [Required(ErrorMessage =
        "La descripción del proyecto es requerida")]
        [StringLength(2000, ErrorMessage =
        "La descripción del proyecto no puede exceder los 2000 caracteres")]
        public required string Description { get; set; }
        [Required(ErrorMessage =
        "La tecnología del proyecto es requerida")]
        [StringLength(100, ErrorMessage =
        "La tecnología del proyecto no puede exceder los 100 caracteres")]

        public required string Technology { get; set; }
        [Required(ErrorMessage =
        "La URL del proyecto es requerida")]
        [Url(ErrorMessage =
        "La URL del proyecto debe ser una URL válida")]

        public required string Url { get; set; }
        [Required(ErrorMessage =
        "La URL de la imagen del proyecto es requerida")]
        [Url(ErrorMessage =
        "La URL de la imagen del proyecto debe ser una URL válida")]

        public required string ImgUrl { get; set; }

        // IDs de habilidades asociadas al proyecto
        public List<int>? SkillIds { get; set; }

        public List<int>? ExperienceIds { get; set; }


        public List<SkillsDTO> Skills { get; set; } = new List<SkillsDTO>();

        // ID del usuario dueño del proyecto
        public int UserId { get; internal set; }

        public AppUser? User { get; set; }

        public List<AppExperience> Experiences { get; set; } = new List<AppExperience>();


        public List<ExperienceDTO> Experience { get; set; } = new List<ExperienceDTO>();

    
        public List<int>? UserIds { get; set; }  // IDs de usuarios

        public List<UserDTO> Users { get; set; } = new List<UserDTO>();

    }
}