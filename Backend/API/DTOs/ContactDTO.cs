using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class ContactDTO
    {
       

         [Required(ErrorMessage = 
         "El Name es requerido")]
         [StringLength(100, ErrorMessage = 
         "El Name no puede exceder los 100 caracteres")]

    
        public required string Name { get; set; }

        [Required(ErrorMessage = 
        "El Email es requerido")]
        [EmailAddress(ErrorMessage = 
        "El formato del Email no es válido")]
        [StringLength(255, ErrorMessage = 
        "El Email no puede exceder los 255 caracteres")]

        public required string Email { get; set; }

        [Required(ErrorMessage = 
        "El Phone es requerido")]
        [Phone(ErrorMessage = 
        "El formato del Phone no es válido")]

        public required string Phone { get; set; }

        [Required(ErrorMessage = 
        "El Message es requerido")]
        [StringLength(500, ErrorMessage = 
        "El Message no puede exceder los 500 caracteres")]

        public required string Message { get; set; }

    }
}