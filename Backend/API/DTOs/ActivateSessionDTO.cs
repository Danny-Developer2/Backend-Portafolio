using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class ActivateSessionDTO
    {

        [Required]
        [Key]
        public int? Id { get; set; }

        public int UserId { get; internal set; }

        public string? TokenHash { get; set; }

        public DateTime? Expiration { get; set; }
    }
}