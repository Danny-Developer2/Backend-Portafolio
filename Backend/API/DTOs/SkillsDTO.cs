using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace API.DTOs
{
    public class SkillsDTO
    {
        public int Id { get; set; }


        [Required(ErrorMessage ="EL nombre de la habilidad es requerido ")]
        [StringLength(100, ErrorMessage ="El nombre de la habilidad no puede exceder los 100 caracteres")]
        [MinLength(3, ErrorMessage ="El nombre de la habilidad debe tener al menos 3 caracteres")]
        public   required string SkillName { get; set; }
        public required string Name { get; set; }

        [Required(ErrorMessage ="La porcentaje de la habilidad es requerido ")]
        [Range(0, 100, ErrorMessage ="La porcentaje de la habilidad debe estar entre 0 y 100")]
        
        public required int Percentage { get; set; }

        [Required(ErrorMessage ="El icono de la habilidad es requerido ")]
        [StringLength(255, ErrorMessage ="El icono de la habilidad no puede exceder los 255 caracteres")]
        [MinLength(10, ErrorMessage ="El icono de la habilidad debe tener al menos 10 caracteres")]
        public  required string IconUrl { get; set; }

        [Required(ErrorMessage ="La descripción de la habilidad es requerida ")]
        [StringLength(2000, ErrorMessage ="La descripción de la habilidad no puede exceder los 2000 caracteres")]
        [MinLength(100, ErrorMessage ="La descripción de la habilidad debe tener al menos 100 caracteres")]
    

        public required string Description { get; set; }

        
    }
}