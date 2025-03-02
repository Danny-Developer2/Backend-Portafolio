using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class AppSkill
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public required int Percentage { get; set; }


        public  required string IconUrl { get; set; }

        public required string Description { get;  set; }

        public List<AppUser> Users { get; set; } = new List<AppUser>();

        public ICollection<UserSkill>? UserSkills { get; set; }

        // Relación con ProyectSkills
        public ICollection<ProyectSkill>? ProyectSkills { get; set; }

        

    }
}