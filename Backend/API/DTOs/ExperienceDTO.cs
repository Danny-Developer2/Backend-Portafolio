using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.OpenApi.MicrosoftExtensions;
using System.ComponentModel.DataAnnotations;
using API.Entities;

namespace API.DTOs
{
    public class ExperienceDTO
    {
        [Required(ErrorMessage =
        "El ID de la experiencia es requerido")]
        [Range(1, int.MaxValue, ErrorMessage =
        "El ID de la experiencia debe ser un número entero positivo")]
        public int Id { get; set; }
        [Required(ErrorMessage =
        "El nombre de la empresa es requerido")]
        [StringLength(100, ErrorMessage =
        "El nombre de la empresa no puede exceder los 100 caracteres")]

        public required string CompanyName { get; set; }
        [Required(ErrorMessage =
        "El puesto del trabajo es requerido")]
        [StringLength(100, ErrorMessage =
        "El puesto del trabajo no puede exceder los 100 caracteres")]

        public required string Position { get; set; }

        [Required(ErrorMessage =
        "La fecha de inicio de la experiencia es requerida")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public required DateTime StartDate { get; set; }
        [DataType(DataType.Date)]

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Range(typeof(DateTime), "01/01/1900", "01/01/2100", ErrorMessage = "La fecha de fin de la experiencia debe ser una fecha válida y no debe ser anterior a la fecha de inicio")]

        public DateTime? EndDate { get; set; }
        [Required(ErrorMessage =
        "La descripción de la experiencia es requerida")]
        [StringLength(2000, ErrorMessage =
        "La descripción de la experiencia no puede exceder los 2000 caracteres")]
      
        public required string Description { get; set; }

    //    public  AppUser? User { get; set; } 


    }
}