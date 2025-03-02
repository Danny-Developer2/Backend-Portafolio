using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class AppConcactMessage
    {
        [Key]
        public int MessageId { get; set; }

        public required string Name { get; set; }

        public required string Email { get; set; }

        public required string Phone { get; set; }

        public required string Message { get; set; }
        
    }
}