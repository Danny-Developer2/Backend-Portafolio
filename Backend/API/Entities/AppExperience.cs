using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class AppExperience
    {
        [Key]
        public int Id { get; set; }

        public required string CompanyName { get; set; }

        public required string Position { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public required string Description { get; set; }

        
        public int UserId { get; set; }  

        public AppUser? User { get; set; } 

        

        


    }
}